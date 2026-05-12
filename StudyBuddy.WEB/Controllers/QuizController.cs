using Microsoft.AspNetCore.Mvc;

namespace StudyBuddy.WEB.Controllers
{
    public class QuizController : Controller
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
