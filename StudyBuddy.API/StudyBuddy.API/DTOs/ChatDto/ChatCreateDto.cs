namespace StudyBuddy.API.DTOs.ChatDto
{
    public class ChatCreateDto
    {
        public int MatchRequestId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}