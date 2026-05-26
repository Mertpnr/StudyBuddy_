using StudyBuddy.WEB.Models.Chat;

namespace StudyBuddy.WEB.Models.Dashboard
{
    public class DashboardViewModel
    {
        public string UserName { get; set; } = "StudyBuddy";

        public int ActiveChatCount { get; set; }

        public int PendingRequestCount { get; set; }

        public int AcceptedMatchCount { get; set; }

        public List<ChatConversationViewModel> RecentChats { get; set; } = new();
    }
}