namespace StudyBuddy.API.DTOs.MatchRequestDto
{
    public class MatchRequestBaseDto
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public byte Status { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}