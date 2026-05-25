namespace StudyBuddy.WEB.Models.User
{
    public class UserProfileViewModel
    {
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }

        public string NameSurname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string? AboutMe { get; set; }
        public string? University { get; set; }
        public string? Major { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}