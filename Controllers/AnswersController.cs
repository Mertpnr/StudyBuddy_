using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Requests.AnswerRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerService _service;

        public AnswersController(IAnswerService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAnswersAsync();
            return Ok(list);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetAnswerByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AnswerCreateRequest request)
        {
            var id = await _service.CreateAnswerAsync(request);
            return Ok(new { id });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AnswerUpdateRequest request)
        {
            var ok = await _service.UpdateAnswerAsync(request);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAnswerAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}
