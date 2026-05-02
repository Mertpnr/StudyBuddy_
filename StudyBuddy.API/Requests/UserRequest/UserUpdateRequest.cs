namespace StudyBuddy.API.Requests.UserRequest
{
    public class UserUpdateRequest
    {
        public int UserId { get; set; }

        public string NameSurname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? AboutMe { get; set; }

        public string University { get; set; } = string.Empty;

        public string? Major { get; set; }
    }
}