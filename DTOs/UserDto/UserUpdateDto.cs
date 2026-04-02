namespace StudyBuddy.API.DTOs
{
    public class UserUpdateDto : UserBaseDto
    {
        public int UserId { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
