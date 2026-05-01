namespace StudyBuddy.API.DTOs.UserDto
{
    public class UserBaseDto
    {
        public string NameSurname { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? AboutMe { get; set; }
        public string University { get; set; } = string.Empty;
        public string? Major { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

