namespace StudyBuddy.API.DTOs.MatchDto
{
    public class MatchBaseDto
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public decimal MatchPercent { get; set; }
        public DateTime MatchDate { get; set; }
    }
}
