using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.WEB.Models.User
{
    public class UserUpdateViewModel
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string NameSurname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? AboutMe { get; set; }
        public string? University { get; set; }
        public string? Major { get; set; }
    }
}
