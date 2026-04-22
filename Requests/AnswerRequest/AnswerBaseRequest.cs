namespace StudyBuddy.API.Requests.AnswerRequest
{
    public class AnswerBaseRequest
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public int UserId { get; set; }
    }
}
