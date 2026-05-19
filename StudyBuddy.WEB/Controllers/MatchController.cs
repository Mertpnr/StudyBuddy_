using Microsoft.AspNetCore.Mvc;
using StudyBuddy.WEB.Models.Match;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Controllers
{
    public class MatchController : Controller
    {
        private readonly IUserWebService _userWebService;
        private readonly IMatchWebService _matchWebService;

        public MatchController(
            IUserWebService userWebService,
            IMatchWebService matchWebService)
        {
            _userWebService = userWebService;
            _matchWebService = matchWebService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return RedirectToAction("Login", "Auth");

            var currentUserId = int.Parse(userIdText);

            var users = await _userWebService.GetAllUsersAsync();

            var model = new MatchPageViewModel
            {
                Users = users
                    .Where(x => x.UserId != currentUserId)
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(MatchPageViewModel pageModel)
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return RedirectToAction("Login", "Auth");

            var currentUserId = int.Parse(userIdText);

            var request = new CalculateMatchViewModel
            {
                User1Id = currentUserId,
                User2Id = pageModel.SelectedUserId,

                // Change this if your Subject question id is different.
                SubjectQuestionId = 1,

                SubjectMatchMode = 1,
                MinimumSharedQuestions = 1,
                SaveResult = true
            };

            var result = await _matchWebService.CalculateMatchAsync(request);

            var users = await _userWebService.GetAllUsersAsync();

            pageModel.Users = users
                .Where(x => x.UserId != currentUserId)
                .ToList();

            pageModel.Result = result;

            return View("Index", pageModel);
        }
    }
}
