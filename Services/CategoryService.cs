using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.CategoryRequest;
using StudyBuddy.API.Services.Interface;
using StudyBuddy.DTOs.CategoryDto;

namespace StudyBuddy.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryListDto>> GetAllCategoriesAsync()
        {
            var list = await _repository.GetAll();

            return list.Select(x => new CategoryListDto
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName ?? string.Empty
            }).ToList();
        }

        public async Task<CategoryBaseDto?> GetCategoryByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null) return null;

            return new CategoryBaseDto
            {
                CategoryName = entity.CategoryName ?? string.Empty
            };
        }

        public async Task<int> CreateCategoryAsync(CategoryCreateRequest request)
        {
            var entity = new Category
            {
                CategoryName = request.CategoryName
            };

            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateCategoryAsync(CategoryUpdateRequest request)
        {
            var existing = await _repository.GetById(request.CategoryId);
            if (existing == null) return false;

            existing.CategoryName = request.CategoryName;

            return await _repository.Update(existing);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing == null) return false;

            return await _repository.Delete(id);
        }
    }
}
