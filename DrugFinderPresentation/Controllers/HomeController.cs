using DrugFinderPresentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrugFinderPresentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SuperAdmin"))
                    return RedirectToAction("Index", "Admin");

                if (User.IsInRole("Pharmacy"))
                    return RedirectToAction("Index", "Pharmacy");

                if (User.IsInRole("Patient"))
                    return RedirectToAction("Index", "Patient");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
