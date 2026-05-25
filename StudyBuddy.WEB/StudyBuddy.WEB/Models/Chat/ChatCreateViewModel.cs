namespace StudyBuddy.WEB.Models.Chat
{
    public class ChatCreateViewModel
    {
        public int MatchRequestId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}