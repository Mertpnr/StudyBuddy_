using StudyBuddy.API.DTOs.UserDto;

namespace StudyBuddy.API.DTOs.AuthDto
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public UserBaseDto? User { get; set; }
    }
}