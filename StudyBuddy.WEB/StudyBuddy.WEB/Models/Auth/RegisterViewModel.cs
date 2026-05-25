using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.WEB.Models.Auth
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string NameSurname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).+$", ErrorMessage = "Password must contain at least one uppercase letter and one special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? AboutMe { get; set; }

        public string? University { get; set; }

        public string? Major { get; set; }
    }
}