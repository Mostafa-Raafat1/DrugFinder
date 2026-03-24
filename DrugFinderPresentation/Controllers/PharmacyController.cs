using DrugFinderMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace DrugFinderMVC.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PharmacyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
                name = model.Name,
                address = model.Address,
                email = model.Email,
                password = model.Password,
                confirmPassword = model.ConfirmPassword,
                liscenceNumber = model.LiscenceNumber,
                latitude = model.Latitude,
                longitude = model.Longitude
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
            ModelState.AddModelError("", string.IsNullOrWhiteSpace(error) ? "Registration failed. Please try again." : error);
            return View(model);
        }
    }
}
