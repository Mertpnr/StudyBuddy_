namespace StudyBuddy.WEB.Models.Match
{
	public class MatchResultViewModel
	{
		public int User1Id { get; set; }

		public int User2Id { get; set; }

		public decimal MatchPercent { get; set; }

		public string Message { get; set; } = string.Empty;
	}
}
