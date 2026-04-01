using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Model;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> InsertCategory([FromBody] Category category)
        {
            int result = await _categoryService.InsertCategory(category);

            if (result > 0)
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
            var deleted = await _categoryService.DeleteCategory(id);
            return Ok(deleted);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Category category)
        {
            var updated = await _categoryService.UpdateCategory(category);
            return Ok(updated);
        }
    }
}