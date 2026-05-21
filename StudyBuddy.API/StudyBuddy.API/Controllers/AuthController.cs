using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Requests.AuthRequest;
using StudyBuddy.API.Requests.UserRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserCreateRequest request)
        {
            var userId = await _userService.CreateUserAsync(request);

            return Ok(new
            {
                Message = "User registered successfully.",
                UserId = userId
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}