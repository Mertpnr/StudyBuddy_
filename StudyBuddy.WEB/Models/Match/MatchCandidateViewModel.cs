namespace StudyBuddy.WEB.Models.Match
{
	public class MatchCandidateViewModel
	{
		public int UserId { get; set; }

		public Guid UserGuid { get; set; }

		public string NameSurname { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string? University { get; set; }

		public string? Major { get; set; }

		public string? AboutMe { get; set; }

		public decimal? MatchPercent { get; set; }
	}
}
