namespace StudyBuddy.WEB.Models.Chat
{
    public class ChatConversationViewModel
    {
        public int MatchRequestId { get; set; }

        public int OtherUserId { get; set; }

        public string OtherUserName { get; set; } = "Unknown user";

        public string? OtherUserUniversity { get; set; }

        public string? OtherUserMajor { get; set; }

        public string? LastMessage { get; set; }

        public DateTime? LastMessageDate { get; set; }
    }
}