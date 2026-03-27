using Microsoft.AspNetCore.Mvc;

namespace DrugFinderPresentation.Controllers
{
    public class SuperAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
