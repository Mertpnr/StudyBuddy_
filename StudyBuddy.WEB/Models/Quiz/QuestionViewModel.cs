namespace StudyBuddy.WEB.Models.Quiz
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }

        public string Question { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public decimal MatchPercent { get; set; }

        public List<OptionViewModel> Options { get; set; } = new();
    }
}
