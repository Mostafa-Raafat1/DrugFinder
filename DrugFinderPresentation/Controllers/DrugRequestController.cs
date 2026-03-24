using DrugFinderMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DrugFinderMVC.Controllers
{
    [Authorize(Roles = "Patient")]
    public class DrugRequestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DrugRequestController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateDrugRequestViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDrugRequestViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("DrugFinderAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = model.Drugs.Select(d => new
            {
                drugName = d.DrugName,
                strength = d.Strength,
                form = d.Form,
                quantity = d.Quantity
            });

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("api/DrugRequest/CreateRequest", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Drug request submitted successfully! Nearby pharmacies have been notified.";
                return RedirectToAction("Success");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", string.IsNullOrWhiteSpace(error) ? "Failed to submit request. Please try again." : error);
            return View(model);
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}
