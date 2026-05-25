namespace StudyBuddy.API.Requests.ChatRequest
{
    public class ChatBaseRequest
    {
        public int MatchRequestId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}