using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Model;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMatches()
        {
            var list = await _matchService.GetAllMatches();
            return Ok(list);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var match = await _matchService.GetMatchById(id);
            return Ok(match);
        }

        [HttpPost]
        public async Task<IActionResult> InsertMatch(Match match)
        {
            int id = await _matchService.InsertMatch(match);
            if (id > 0)
                return Ok();
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Match match)
        {
            var updated = await _matchService.UpdateMatch(match);
            return Ok(updated);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _matchService.DeleteMatch(id);
            return Ok(deleted);
        }
    }
}