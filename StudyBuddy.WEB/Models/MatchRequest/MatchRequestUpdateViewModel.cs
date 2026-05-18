namespace StudyBuddy.WEB.Models.MatchRequest
{
    public class MatchRequestUpdateViewModel
    {
        public int Id { get; set; }

        public Guid User1 { get; set; }

        public Guid User2 { get; set; }

        public int Status { get; set; }
    }
}