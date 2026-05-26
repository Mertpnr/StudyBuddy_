namespace StudyBuddy.WEB.Models.Chat
{
    public class ChatListViewModel
    {
        public int ChatId { get; set; }
        public int MatchRequestId { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
    }
}