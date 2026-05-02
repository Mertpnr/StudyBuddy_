namespace StudyBuddy.API.Requests.OptionRequest
{
    public class OptionBaseRequest
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int OrderNo { get; set; }
    }
}
