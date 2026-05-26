namespace StudyBuddy.API.DTOs.ChatDto
{
    public class ChatBaseDto
    {
        public int MatchRequestId { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
    }
}