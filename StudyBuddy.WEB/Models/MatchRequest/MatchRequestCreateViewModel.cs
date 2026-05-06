namespace StudyBuddy.WEB.Models.MatchRequest
{
	public class MatchRequestCreateViewModel
	{
		public int SenderUserId { get; set; }

		public int ReceiverUserId { get; set; }

		public int Status { get; set; } = 0;
	}
}
