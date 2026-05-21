namespace StudyBuddy.API.DTOs.QuestionDto
{
    public class QuestionBaseDto
    {
        public string Question { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal MatchPercent { get; set; }
    }
}
