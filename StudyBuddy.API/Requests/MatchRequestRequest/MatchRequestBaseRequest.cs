namespace StudyBuddy.API.Requests.MatchRequestRequest
{
    public class MatchRequestBaseRequest
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public byte Status { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
