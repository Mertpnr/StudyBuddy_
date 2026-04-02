using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Model;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var list = await _questionService.GetAllQuestions();
            return Ok(list);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _questionService.GetQuestionById(id);
            return Ok(question);
        }

        [HttpPost]
        public async Task<IActionResult> InsertQuestion([FromBody] Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _questionService.InsertQuestion(question);

            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> Update(Question question)
        {
            var updated = await _questionService.UpdateQuestion(question);
            return Ok(updated);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _questionService.DeleteQuestion(id);
            return Ok(deleted);
        }
    }
}