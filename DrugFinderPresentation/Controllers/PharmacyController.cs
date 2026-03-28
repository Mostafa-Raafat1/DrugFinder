using DrugFinderMVC.Models;
using DrugFinderPresentation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DrugFinderMVC.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PharmacyController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles = "Pharmacy")]
        public async Task<IActionResult> Index()
        {
            // ✅ Session is guaranteed to have the token because
            // OnValidatePrincipal restores it on every authenticated request
            var token = HttpContext.Session.GetString("JwtToken");

            // This should now never happen, but keep as a safety net
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5080/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("api/Pharmacy/GetNearByDrugRequests");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load nearby requests.";
                return View(new List<RequestVM>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var vm = JsonSerializer.Deserialize<List<RequestVM>>(json, options)
                     ?? new List<RequestVM>();

            return View(vm);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterPharmacyViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("DrugFinderAPI");

            var payload = new
            {
                name             = model.Name,
                address          = model.Address,
                email            = model.Email,
                password         = model.Password,
                confirmPassword  = model.ConfirmPassword,
                liscenceNumber   = model.LiscenceNumber,
                latitude         = model.Latitude!.Value,
                longitude        = model.Longitude!.Value
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("api/Pharmacy", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Pharmacy registered successfully! You can now log in.";
                return RedirectToAction("Login", "Account");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", string.IsNullOrWhiteSpace(error)
                ? "Registration failed. Please try again." : error);
            return View(model);
        }

        // ── Notifications ─────────────────────────────────────────
        [Authorize(Roles = "Pharmacy")]
        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("DrugFinderAPI");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // Note: matches the exact (typo'd) route on the API
            var response = await client.GetAsync("api/Pharmacy/GetNotfications");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load notifications. Please try again.";
                return View(new List<NotificationViewModel>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var notifications = JsonSerializer.Deserialize<List<NotificationViewModel>>(json, options)
                                ?? new List<NotificationViewModel>();

            return View(notifications);
        }

        // ── Response ─────────────────────────────────────────

        [Authorize(Roles = "Pharmacy")]
        [HttpGet]
        public async Task<IActionResult> Respond(int id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            // Fetch the original request so we can pre-populate drug names in the form
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5080/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var requestResponse = await client.GetAsync($"api/Pharmacy/GetNearByDrugRequests");

            List<RequestVM> allRequests = new();

            if (requestResponse.IsSuccessStatusCode)
            {
                var json = await requestResponse.Content.ReadAsStringAsync();
                allRequests = JsonSerializer.Deserialize<List<RequestVM>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? new();
            }

            var request = allRequests.FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                TempData["Error"] = "Request not found.";
                return RedirectToAction("Index");
            }

            // Build the ViewModel pre-filled with drug names from the original request
            var vm = new RespondToRequestVM
            {
                RequestId = id,
                PatientName = request.PatientName,
                Items = request.Drugs.Select(d => new RespondItemVM
                {
                    DrugName = d.Name,
                    Strength = d.Strength,   
                    Form = d.Form,       
                    Quantity = d.Quantity,   
                    Available = false,
                    Price = null
                }).ToList()
            };

            return View(vm);
        }

        [Authorize(Roles = "Pharmacy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(RespondToRequestVM model)
        {

            for (int i = 0; i < model.Items.Count; i++)
            {
                if (model.Items[i].Available && model.Items[i].Price == null)
                {
                    ModelState.AddModelError(
                        $"Items[{i}].Price",
                        $"Please enter a price for {model.Items[i].DrugName}.");
                }
            }

            if (!ModelState.IsValid)
                return View(model);

            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5080/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var payload = new
            {
                requestId = model.RequestId,
                respondedAt = DateTime.UtcNow,
                items = model.Items.Select(i => new
                {
                    drugName = i.DrugName,
                    available = i.Available,
                    price = i.Price
                })
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "api/PharmacyResponse/CreatePharmacyResponse", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = $"Response for {model.PatientName} sent successfully!";
                return RedirectToAction("Index");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", string.IsNullOrWhiteSpace(error)
                ? "Failed to submit response. Please try again."
                : error);

            return View(model);
        }
    }
}
