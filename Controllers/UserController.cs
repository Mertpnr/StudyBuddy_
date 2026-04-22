using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Requests.UserRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllUsersAsync();
            return Ok(list);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetUserByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
        {
            var id = await _service.CreateUserAsync(request);
            return Ok(new { id });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest request)
        {
            var ok = await _service.UpdateUserAsync(request);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteUserAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}
