using Microsoft.AspNetCore.Mvc;

namespace StudyBuddy.WEB.Controllers
{
    public class MatchRequestController : Controller
    {
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }
}
