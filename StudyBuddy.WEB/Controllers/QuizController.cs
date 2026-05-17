using Microsoft.AspNetCore.Mvc;
using StudyBuddy.WEB.Models.Quiz;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizWebService _quizWebService;

        public QuizController(IQuizWebService quizWebService)
        {
            _quizWebService = quizWebService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return RedirectToAction("Login", "Auth");

            var model = await _quizWebService.GetQuizAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(QuizPageViewModel model)
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return RedirectToAction("Login", "Auth");

            var userId = int.Parse(userIdText);

            var ok = await _quizWebService.SubmitAnswersAsync(userId, model.SelectedOptions);

            if (!ok)
            {
                TempData["ErrorMessage"] = "Answers could not be saved.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Your answers were saved successfully.";
            return RedirectToAction("Index", "Match");
        }
    }
}
