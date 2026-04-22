using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Requests.OptionRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptionController : ControllerBase
    {
        private readonly IOptionService _service;

        public OptionController(IOptionService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllOptionsAsync();
            return Ok(list);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetOptionByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OptionCreateRequest request)
        {
            var id = await _service.CreateOptionAsync(request);
            return Ok(new { id });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] OptionUpdateRequest request)
        {
            var ok = await _service.UpdateOptionAsync(request);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteOptionAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}
