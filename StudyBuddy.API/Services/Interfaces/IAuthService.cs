using StudyBuddy.API.DTOs.AuthDto;
using StudyBuddy.API.Requests.AuthRequest;

namespace StudyBuddy.API.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequest request);
    }
}