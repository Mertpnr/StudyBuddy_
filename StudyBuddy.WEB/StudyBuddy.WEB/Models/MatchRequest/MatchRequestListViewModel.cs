namespace StudyBuddy.WEB.Models.MatchRequest
{
    public class MatchRequestListViewModel
    {
        public int MatchRequestId { get; set; }

        public int User1Id { get; set; }

        public int User2Id { get; set; }

        public int Status { get; set; }

        public string StatusText
        {
            get
            {
                return Status switch
                {
                    0 => "Pending",
                    1 => "Accepted",
                    2 => "Rejected",
                    3 => "Cancelled",
                    _ => "Unknown"
                };
            }
        }

        public DateTime CreatedDate { get; set; }

        public string? Message { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? User1Name { get; set; }

        public string? User2Name { get; set; }
    }
}