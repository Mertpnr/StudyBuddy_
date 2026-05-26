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
        public async Task<IActionResult> Create(int user2Id, string? message)
        {
            var user1Id = GetCurrentUserId();

            if (user1Id == null)
                return RedirectToAction("Login", "Auth");

            if (user2Id <= 0 || user2Id == user1Id.Value)
            {
                TempData["ErrorMessage"] = "Match request could not be sent.";
                return RedirectToAction("Index", "Match");
            }

            var ok = await _matchRequestWebService.SendRequestAsync(
                user1Id.Value,
                user2Id,
                message);

            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = ok
                ? "Match request sent successfully."
                : "You already have a pending request or active chat with this user.";

            return RedirectToAction("Index", "Match");
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var requests = await _matchRequestWebService.GetRequestsByUserIdAsync(userId.Value);
            var request = requests.FirstOrDefault(x => x.MatchRequestId == id);

            if (request == null || request.User2Id != userId.Value || request.Status != 0)
            {
                TempData["ErrorMessage"] = "Match request could not be accepted.";
                return RedirectToAction("Index");
            }

            var ok = await _matchRequestWebService.AcceptRequestAsync(request);

            if (ok)
            {
                TempData["SuccessMessage"] = "Match request accepted.";
                return RedirectToAction("Room", "Chat", new { matchRequestId = request.MatchRequestId });
            }

            TempData["ErrorMessage"] = "Match request could not be accepted.";
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

            if (request == null || request.User2Id != userId.Value || request.Status != 0)
            {
                TempData["ErrorMessage"] = "Match request could not be declined.";
                return RedirectToAction("Index");
            }

            var ok = await _matchRequestWebService.RejectRequestAsync(request);

            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = ok
                ? "Match request declined."
                : "Match request could not be declined.";

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

            if (request == null || request.User1Id != userId.Value || request.Status != 0)
            {
                TempData["ErrorMessage"] = "Match request could not be cancelled.";
                return RedirectToAction("Index");
            }

            var ok = await _matchRequestWebService.CancelRequestAsync(request);

            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = ok
                ? "Match request cancelled."
                : "Match request could not be cancelled.";

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