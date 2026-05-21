namespace StudyBuddy.WEB.Models.MatchRequest
{
    public class MatchRequestListViewModel
    {
        public int MatchRequestId { get; set; }

        public int SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }

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

        public DateTime UpdatedDate { get; set; }

        public string? SenderName { get; set; }

        public string? ReceiverName { get; set; }
    }
}
