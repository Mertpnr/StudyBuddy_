using Microsoft.AspNetCore.Mvc;
using StudyBuddy.WEB.Models.Chat;
using StudyBuddy.WEB.Models.Dashboard;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IChatWebService _chatWebService;
        private readonly IMatchRequestWebService _matchRequestWebService;
        private readonly IUserWebService _userWebService;

        public DashboardController(
            IChatWebService chatWebService,
            IMatchRequestWebService matchRequestWebService,
            IUserWebService userWebService)
        {
            _chatWebService = chatWebService;
            _matchRequestWebService = matchRequestWebService;
            _userWebService = userWebService;
        }

        public async Task<IActionResult> Index()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText) || !int.TryParse(userIdText, out var userId))
                return RedirectToAction("Login", "Auth");

            var requests = await _matchRequestWebService.GetRequestsByUserIdAsync(userId);
            var acceptedRequests = requests
                .Where(x => x.Status == 1)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
            var conversations = new List<ChatConversationViewModel>();

            foreach (var request in acceptedRequests.Take(3))
            {
                var otherUserId = request.User1Id == userId
                    ? request.User2Id
                    : request.User1Id;
                var otherUser = await _userWebService.GetUserByIdAsync(otherUserId);
                var messages = await _chatWebService.GetMessagesAsync(request.MatchRequestId, userId);
                var lastMessage = messages.LastOrDefault();

                conversations.Add(new ChatConversationViewModel
                {
                    MatchRequestId = request.MatchRequestId,
                    OtherUserId = otherUserId,
                    OtherUserName = otherUser?.NameSurname
                        ?? (request.User1Id == userId ? request.User2Name : request.User1Name)
                        ?? "Study buddy",
                    OtherUserUniversity = otherUser?.University,
                    OtherUserMajor = otherUser?.Major,
                    LastMessage = lastMessage?.Message,
                    LastMessageDate = lastMessage?.Date
                });
            }

            var model = new DashboardViewModel
            {
                UserName = HttpContext.Session.GetString("UserName") ?? "StudyBuddy",
                ActiveChatCount = acceptedRequests.Count,
                AcceptedMatchCount = acceptedRequests.Count,
                PendingRequestCount = requests.Count(x => x.Status == 0),
                RecentChats = conversations
                    .OrderByDescending(x => x.LastMessageDate ?? DateTime.MinValue)
                    .ThenBy(x => x.OtherUserName)
                    .ToList()
            };

            return View(model);
        }
    }
}