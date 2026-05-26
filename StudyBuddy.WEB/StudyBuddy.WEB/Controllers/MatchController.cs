using Microsoft.AspNetCore.Mvc;
using StudyBuddy.WEB.Models.Match;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Controllers
{
    public class MatchController : Controller
    {
        private readonly IUserWebService _userWebService;
        private readonly IMatchWebService _matchWebService;
        private readonly IMatchRequestWebService _matchRequestWebService;

        public MatchController(
            IUserWebService userWebService,
            IMatchWebService matchWebService,
            IMatchRequestWebService matchRequestWebService)
        {
            _userWebService = userWebService;
            _matchWebService = matchWebService;
            _matchRequestWebService = matchRequestWebService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return RedirectToAction("Login", "Auth");

            var currentUserId = int.Parse(userIdText);

            var model = await BuildMatchPageAsync(currentUserId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(MatchPageViewModel pageModel)
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return RedirectToAction("Login", "Auth");

            var currentUserId = int.Parse(userIdText);

            if (pageModel.SelectedUserId <= 0)
            {
                TempData["ErrorMessage"] = "Please select a user to calculate match.";
                return RedirectToAction("Index");
            }

            var request = new CalculateMatchViewModel
            {
                User1Id = currentUserId,
                User2Id = pageModel.SelectedUserId,
                SubjectQuestionId = 1,
                SubjectMatchMode = 1,
                MinimumSharedQuestions = 1,
                SaveResult = true
            };

            var result = await _matchWebService.CalculateMatchAsync(request);

            pageModel = await BuildMatchPageAsync(currentUserId);

            pageModel.Result = result;

            return View("Index", pageModel);
        }

        private async Task<MatchPageViewModel> BuildMatchPageAsync(int currentUserId)
        {
            var users = (await _userWebService.GetAllUsersAsync())
                .Where(x => x.UserId != currentUserId)
                .ToList();
            var activeRequests = (await _matchRequestWebService.GetRequestsByUserIdAsync(currentUserId))
                .Where(x => x.Status == 0 || x.Status == 1)
                .ToList();

            var candidates = new List<MatchCandidateViewModel>();

            foreach (var user in users)
            {
                var activeRequest = activeRequests.FirstOrDefault(x =>
                    (x.User1Id == currentUserId && x.User2Id == user.UserId)
                    || (x.User1Id == user.UserId && x.User2Id == currentUserId));

                var result = await _matchWebService.CalculateMatchAsync(new CalculateMatchViewModel
                {
                    User1Id = currentUserId,
                    User2Id = user.UserId,
                    SubjectQuestionId = 1,
                    SubjectMatchMode = 1,
                    MinimumSharedQuestions = 1,
                    SaveResult = true
                });

                candidates.Add(new MatchCandidateViewModel
                {
                    UserId = user.UserId,
                    UserGuid = user.UserGuid,
                    NameSurname = user.NameSurname,
                    Email = user.Email,
                    University = user.University,
                    Major = user.Major,
                    AboutMe = user.AboutMe,
                    MatchPercent = result?.MatchPercent,
                    ActiveMatchRequestId = activeRequest?.MatchRequestId,
                    ActiveMatchRequestStatus = activeRequest?.Status
                });
            }

            return new MatchPageViewModel
            {
                Users = users,
                Candidates = candidates
                    .OrderByDescending(x => x.MatchPercent ?? 0m)
                    .ThenBy(x => x.NameSurname)
                    .ToList()
            };
        }
    }
}