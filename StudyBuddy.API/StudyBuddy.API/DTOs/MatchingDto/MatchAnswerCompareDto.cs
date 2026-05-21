namespace StudyBuddy.API.DTOs.MatchingDto
{
    public class MatchAnswerCompareDto
    {
        public int QuestionId { get; set; }
        public decimal User1Value { get; set; }
        public decimal User2Value { get; set; }
        public decimal QuestionWeight { get; set; }
    }
}