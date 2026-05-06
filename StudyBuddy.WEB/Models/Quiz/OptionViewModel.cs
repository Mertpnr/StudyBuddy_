namespace StudyBuddy.WEB.Models.Quiz
{
    public class OptionViewModel
    {
        public int OptionId { get; set; }

        public int QuestionId { get; set; }

        public string Text { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public int OrderNo { get; set; }
    }
}
