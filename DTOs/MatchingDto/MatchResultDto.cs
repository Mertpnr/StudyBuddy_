namespace StudyBuddy.API.DTOs.MatchingDto
{
    public class MatchResultDto
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public decimal MatchPercent { get; set; }
        public int SharedQuestionCount { get; set; }

        public bool SubjectMatched { get; set; }
        public string SubjectMatchMode { get; set; } = string.Empty;

        public bool SavedToDatabase { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}