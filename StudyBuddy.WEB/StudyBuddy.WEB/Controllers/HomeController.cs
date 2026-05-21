using Microsoft.AspNetCore.Mvc;

namespace StudyBuddy.WEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
