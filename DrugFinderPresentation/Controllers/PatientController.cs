using DrugFinderMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace DrugFinderMVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PatientController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

<<<<<<< HEAD
=======

        public IActionResult Index()
        {
            return View();
        }

>>>>>>> origin/Mostafa
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterPatientViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("DrugFinderAPI");

            var payload = new
            {
                fName = model.FName,
                sName = model.SName,
                email = model.Email,
                password = model.Password,
                confirmPassword = model.ConfirmPassword,
<<<<<<< HEAD
                latitude = model.Latitude,
                longitude = model.Longitude
=======
                latitude = model.Latitude!.Value,   // safe: Required ensures non-null here
                longitude = model.Longitude!.Value
>>>>>>> origin/Mostafa
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("api/Patient", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login", "Account");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", string.IsNullOrWhiteSpace(error) ? "Registration failed. Please try again." : error);
            return View(model);
        }
    }
}
