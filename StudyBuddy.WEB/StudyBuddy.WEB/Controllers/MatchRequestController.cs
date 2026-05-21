using Microsoft.AspNetCore.Mvc;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Controllers
{
    public class MatchRequestController : Controller
    {
        private readonly IMatchRequestWebService _matchRequestWebService;

        public MatchRequestController(IMatchRequestWebService matchRequestWebService)
        {
            _matchRequestWebService = matchRequestWebService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return RedirectToAction("Login", "Auth");

            var userId = int.Parse(userIdText);

            var requests = await _matchRequestWebService.GetRequestsByUserIdAsync(userId);

            ViewBag.CurrentUserId = userId;

            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var requests = await _matchRequestWebService.GetRequestsByUserIdAsync(userId.Value);
            var request = requests.FirstOrDefault(x => x.MatchRequestId == id);

            if (request == null)
                return RedirectToAction("Index");

            await _matchRequestWebService.AcceptRequestAsync(request);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var requests = await _matchRequestWebService.GetRequestsByUserIdAsync(userId.Value);
            var request = requests.FirstOrDefault(x => x.MatchRequestId == id);

            if (request == null)
                return RedirectToAction("Index");

            await _matchRequestWebService.RejectRequestAsync(request);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var requests = await _matchRequestWebService.GetRequestsByUserIdAsync(userId.Value);
            var request = requests.FirstOrDefault(x => x.MatchRequestId == id);

            if (request == null)
                return RedirectToAction("Index");

            await _matchRequestWebService.CancelRequestAsync(request);

            return RedirectToAction("Index");
        }

        private int? GetCurrentUserId()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return null;

            return int.Parse(userIdText);
        }
    }
}
