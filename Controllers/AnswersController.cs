using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Model;
using StudyBuddy.API.Service.Interface;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AnswersController : ControllerBase
	{
		private readonly IAnswerService _answerService;

		public AnswersController(IAnswerService answerService)
		{
			_answerService = answerService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAnswers()
		{
			var list = await _answerService.GetAllAnswers();
			return Ok(list);
		}

		[HttpPost]
		public async Task<IActionResult> InsertAnswer(Answer answer)
		{
			int id = await _answerService.InsertAnswer(answer);
			if (id > 0)
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _answerService.DeleteAnswer(id);
			// istersen deleted kontrolü ekleyebilirsin
			return Ok(deleted);
		}

		[HttpGet("GetById")]
		public async Task<IActionResult> GetById(int id)
		{
			var answer = await _answerService.GetById(id);
			return Ok(answer);
		}

		[HttpPut]
		public async Task<IActionResult> Update(Answer answer)
		{
			var updated = await _answerService.UpdateAnswer(answer);
			return Ok(updated);
		}
	}
}