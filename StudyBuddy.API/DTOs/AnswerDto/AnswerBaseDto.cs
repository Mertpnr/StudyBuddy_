namespace StudyBuddy.API.DTOs.AnswerDto
{
    public class AnswerBaseDto
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public int UserId { get; set; }
    }
}
