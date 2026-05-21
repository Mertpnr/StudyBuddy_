using Microsoft.AspNetCore.Mvc;

namespace StudyBuddy.WEB.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View();
        }
    }
}
