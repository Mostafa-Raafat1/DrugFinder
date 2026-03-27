using Microsoft.AspNetCore.Mvc;

namespace DrugFinderMVC.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult About()
        {
            return View();
        }
    }
}
