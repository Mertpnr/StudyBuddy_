namespace StudyBuddy.WEB.Models.Auth
{
    public class LoginResponseViewModel
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? Token { get; set; }

        public LoginUserViewModel? User { get; set; }

        public int UserId => User?.UserId ?? 0;

        public Guid UserGuid => User?.UserGuid ?? Guid.Empty;

        public string NameSurname => User?.NameSurname ?? string.Empty;

        public string Email => User?.Email ?? string.Empty;
    }

    public class LoginUserViewModel
    {
        public int UserId { get; set; }

        public Guid UserGuid { get; set; }

        public string NameSurname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? University { get; set; }

        public string? Major { get; set; }

        public string? AboutMe { get; set; }
    }
}