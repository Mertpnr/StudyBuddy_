using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Model;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OptionController : ControllerBase
	{
		private readonly IOptionService _optionService;

		public OptionController(IOptionService optionService)
		{
			_optionService = optionService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllOptions()
		{
			var list = await _optionService.GetAllOptions();
			return Ok(list);
		}

		[HttpGet("GetById")]
		public async Task<IActionResult> GetById(int id)
		{
			var option = await _optionService.GetOptionById(id);
			return Ok(option);
		}

		[HttpPost]
		public async Task<IActionResult> InsertOption(Option option)
		{
			int id = await _optionService.InsertOption(option);
			if (id > 0)
				return Ok();

			return BadRequest();
		}

		[HttpPut]
		public async Task<IActionResult> Update(Option option)
		{
			var updated = await _optionService.UpdateOption(option);
			return Ok(updated);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _optionService.DeleteOption(id);
			return Ok(deleted);
		}
	}
}