namespace StudyBuddy.WEB.Models.Match
{
	public class MatchListViewModel
	{
		public int MatchId { get; set; }

		public int User1Id { get; set; }

		public int User2Id { get; set; }

		public decimal MatchPercent { get; set; }

		public DateTime CreatedDate { get; set; }

		public string? OtherUserName { get; set; }

		public string? OtherUserUniversity { get; set; }

		public string? OtherUserMajor { get; set; }
	}
}
