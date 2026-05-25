using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Requests.ChatRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _service;

        public ChatController(IChatService service)
        {
            _service = service;
        }

        [HttpGet("GetMessages/{matchRequestId:int}/{userId:int}")]
        public async Task<IActionResult> GetMessages(int matchRequestId, int userId)
        {
            var messages = await _service.GetMessagesAsync(matchRequestId, userId);
            return Ok(messages);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ChatCreateRequest request)
        {
            var id = await _service.CreateMessageAsync(request);

            if (id == null)
                return BadRequest("Message could not be sent.");

            return Ok(new { id });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ChatUpdateRequest request)
        {
            var ok = await _service.UpdateMessageAsync(request);

            if (!ok)
                return BadRequest("Message could not be updated.");

            return Ok();
        }

        [HttpDelete("Delete/{chatId:int}")]
        public async Task<IActionResult> Delete(int chatId)
        {
            var ok = await _service.DeleteMessageAsync(chatId);

            if (!ok)
                return NotFound();

            return Ok();
        }
    }
}