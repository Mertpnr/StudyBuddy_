namespace StudyBuddy.WEB.Models.Auth
{
    public class LoginResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public int UserId { get; set; }
        public Guid UserGuid { get; set; }

        public string NameSurname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string? Token { get; set; }
    }
}
