namespace StudyBuddy.API.Requests.UserRequest
{
    public class UserUpdateRequest : UserBaseRequest
    {
        public int UserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
