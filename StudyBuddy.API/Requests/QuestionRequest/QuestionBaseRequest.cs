namespace StudyBuddy.API.Requests.QuestionRequest
{
    public class QuestionBaseRequest
    {
        public string Question { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal MatchPercent { get; set; }
    }
}
