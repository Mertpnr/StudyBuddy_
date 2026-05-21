namespace StudyBuddy.WEB.Models.Match
{
    public class CalculateMatchViewModel
    {
        public int User1Id { get; set; }

        public int User2Id { get; set; }

        public int SubjectQuestionId { get; set; } = 1;

        public int SubjectMatchMode { get; set; } = 1;

        public int MinimumSharedQuestions { get; set; } = 1;

        public bool SaveResult { get; set; } = true;
    }
}
