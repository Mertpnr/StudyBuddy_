
using StudyBuddy.API.Requests.CategoryRequest;
using StudyBuddy.DTOs.CategoryDto;

namespace StudyBuddy.API.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryListDto>> GetAllCategoriesAsync();
        Task<CategoryBaseDto?> GetCategoryByIdAsync(int id);
        Task<int> CreateCategoryAsync(CategoryCreateRequest request);
        Task<bool> UpdateCategoryAsync(CategoryUpdateRequest request);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
