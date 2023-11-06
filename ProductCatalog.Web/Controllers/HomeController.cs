using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Authorization", "Authentication");
        }
        public IActionResult Error(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;

            return View();
        }
    }
}
