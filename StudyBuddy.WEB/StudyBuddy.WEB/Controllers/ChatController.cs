using Microsoft.AspNetCore.Mvc;
using StudyBuddy.WEB.Models.Chat;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatWebService _chatWebService;
        private readonly IMatchRequestWebService _matchRequestWebService;
        private readonly IUserWebService _userWebService;

        public ChatController(
            IChatWebService chatWebService,
            IMatchRequestWebService matchRequestWebService,
            IUserWebService userWebService)
        {
            _chatWebService = chatWebService;
            _matchRequestWebService = matchRequestWebService;
            _userWebService = userWebService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var requests = await _matchRequestWebService.GetRequestsByUserIdAsync(userId.Value);
            var acceptedRequests = requests
                .Where(x => x.Status == 1)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            var conversations = new List<ChatConversationViewModel>();

            foreach (var request in acceptedRequests)
            {
                var otherUserId = request.User1Id == userId.Value
                    ? request.User2Id
                    : request.User1Id;

                var otherUser = await _userWebService.GetUserByIdAsync(otherUserId);
                var messages = await _chatWebService.GetMessagesAsync(request.MatchRequestId, userId.Value);
                var lastMessage = messages.LastOrDefault();

                conversations.Add(new ChatConversationViewModel
                {
                    MatchRequestId = request.MatchRequestId,
                    OtherUserId = otherUserId,
                    OtherUserName = otherUser?.NameSurname
                        ?? (request.User1Id == userId.Value ? request.User2Name : request.User1Name)
                        ?? "Unknown user",
                    OtherUserUniversity = otherUser?.University,
                    OtherUserMajor = otherUser?.Major,
                    LastMessage = lastMessage?.Message,
                    LastMessageDate = lastMessage?.Date
                });
            }

            return View(conversations
                .OrderByDescending(x => x.LastMessageDate ?? DateTime.MinValue)
                .ThenBy(x => x.OtherUserName)
                .ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Room(int matchRequestId)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            if (matchRequestId <= 0)
            {
                TempData["ErrorMessage"] = "Chat could not be opened.";
                return RedirectToAction("Index", "MatchRequest");
            }

            var messages = await _chatWebService.GetMessagesAsync(matchRequestId, userId.Value);

            var model = new ChatRoomViewModel
            {
                MatchRequestId = matchRequestId,
                CurrentUserId = userId.Value,
                Messages = messages,
                NewMessage = new ChatCreateViewModel
                {
                    MatchRequestId = matchRequestId,
                    UserId = userId.Value
                }
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Messages(int matchRequestId)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized();

            if (matchRequestId <= 0)
                return BadRequest();

            var messages = await _chatWebService.GetMessagesAsync(matchRequestId, userId.Value);

            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChatCreateViewModel model)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            model.UserId = userId.Value;

            if (model.MatchRequestId <= 0)
            {
                TempData["ErrorMessage"] = "Chat could not be opened.";
                return RedirectToAction("Index", "MatchRequest");
            }

            if (string.IsNullOrWhiteSpace(model.Message))
            {
                TempData["ErrorMessage"] = "Message cannot be empty.";
                return RedirectToAction("Room", new { matchRequestId = model.MatchRequestId });
            }

            var ok = await _chatWebService.SendMessageAsync(model);

            TempData[ok ? "SuccessMessage" : "ErrorMessage"] = ok
                ? "Message sent."
                : "Message could not be sent.";

            return RedirectToAction("Room", new { matchRequestId = model.MatchRequestId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateJson(ChatCreateViewModel model)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return Unauthorized(new { success = false, message = "Please login again." });

            model.UserId = userId.Value;

            if (model.MatchRequestId <= 0)
                return BadRequest(new { success = false, message = "Chat could not be opened." });

            if (string.IsNullOrWhiteSpace(model.Message))
                return BadRequest(new { success = false, message = "Message cannot be empty." });

            var ok = await _chatWebService.SendMessageAsync(model);

            if (!ok)
                return BadRequest(new { success = false, message = "Message could not be sent." });

            var messages = await _chatWebService.GetMessagesAsync(model.MatchRequestId, userId.Value);

            return Json(new { success = true, messages });
        }

        private int? GetCurrentUserId()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return null;

            return int.TryParse(userIdText, out var userId)
                ? userId
                : null;
        }
    }
}