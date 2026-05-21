using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Requests.MatchRequestRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchRequestController : ControllerBase
    {
        private readonly IMatchRequestService _service;

        public MatchRequestController(IMatchRequestService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllMatchRequestsAsync();
            return Ok(list);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetMatchRequestByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MatchRequestCreateRequest request)
        {
            var id = await _service.CreateMatchRequestAsync(request);
            return Ok(new { id });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] MatchRequestUpdateRequest request)
        {
            var ok = await _service.UpdateMatchRequestAsync(request);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteMatchRequestAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}
