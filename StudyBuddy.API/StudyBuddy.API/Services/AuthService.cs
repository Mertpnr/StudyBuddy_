using AutoMapper;
using StudyBuddy.API.DTOs.AuthDto;
using StudyBuddy.API.DTOs.UserDto;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.AuthRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Email is required."
                };
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Password is required."
                };
            }

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetByEmailAsync(normalizedEmail);

            if (user == null)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            return new LoginResponseDto
            {
                Success = true,
                Message = "Login successful.",
                User = _mapper.Map<UserBaseDto>(user)
            };
        }
    }
}