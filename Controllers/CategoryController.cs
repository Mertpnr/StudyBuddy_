using Microsoft.AspNetCore.Mvc;
using StudyBuddy.API.Requests.CategoryRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllCategoriesAsync();
            return Ok(list);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetCategoryByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            var id = await _service.CreateCategoryAsync(request);
            return Ok(new { id });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateRequest request)
        {
            var ok = await _service.UpdateCategoryAsync(request);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteCategoryAsync(id);
            return ok ? Ok() : NotFound();
        }
    }
}
