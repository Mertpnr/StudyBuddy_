using StudyBuddy.API.Enums;

namespace StudyBuddy.API.Requests.MatchingRequest
{
	public class CalculateMatchRequest
	{
		public int User1Id { get; set; }
		public int User2Id { get; set; }

		// The QuestionId of the "Subject" question
		public int SubjectQuestionId { get; set; }

		public SubjectMatchMode SubjectMatchMode { get; set; } = SubjectMatchMode.Preferred;

		public int MinimumSharedQuestions { get; set; } = 1;

		public bool SaveResult { get; set; } = true;
	}
}
