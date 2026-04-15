using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Model;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchRequestController : ControllerBase
    {
        private readonly IMatchRequestService _matchRequestService;

        public MatchRequestController(IMatchRequestService matchRequestService)
        {
            _matchRequestService = matchRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMatchRequests()
        {
            var list = await _matchRequestService.GetAllMatchRequests();
            return Ok(list);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _matchRequestService.GetMatchRequestById(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> InsertMatchRequest(MatchRequest matchRequest)
        {
            int id = await _matchRequestService.InsertMatchRequest(matchRequest);
            if (id > 0)
                return Ok();
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(MatchRequest matchRequest)
        {
            var updated = await _matchRequestService.UpdateMatchRequest(matchRequest);
            return Ok(updated);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _matchRequestService.DeleteMatchRequest(id);
            return Ok(deleted);
        }
    }
}