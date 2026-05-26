using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Requests.MatchingRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;

        public MatchingController(IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        [HttpPost("Calculate")]
        public async Task<IActionResult> Calculate([FromBody] CalculateMatchRequest request)
        {
            var result = await _matchingService.CalculateMatchAsync(request);

            if (result.Message.Contains("cannot be matched", StringComparison.OrdinalIgnoreCase) ||
                result.Message.Contains("required", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
