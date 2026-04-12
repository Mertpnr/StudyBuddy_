using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            //return _userRepository.GetAllUsers();
            var a = await _userService.GetAllUsers();
            return Ok(a);

        }

        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> InsertUser([FromForm] User user)
        {
            int a = await _userService.InsertUser(user);
            if (a > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            //return _userRepository.InsertUser(user);

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var deletes = await _userService.DeleteUser(id);
            return Ok();

        }
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var getid = await _userService.GetUserById(id);
            return Ok(getid);
        }

        [HttpPut("Update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] User user)
        {
            var updated = await _userService.UpdateUser(user);

            return Ok(updated);
        }
    }
}
