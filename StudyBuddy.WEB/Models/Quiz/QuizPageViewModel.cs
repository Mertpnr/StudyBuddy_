namespace StudyBuddy.WEB.Models.Quiz
{
    public class QuizPageViewModel
    {
        public List<QuestionViewModel> Questions { get; set; } = new();

        public Dictionary<int, int> SelectedOptions { get; set; } = new();
    }
}
