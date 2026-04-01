using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.API.Model;

namespace StudyBuddy.API.Services.Interface
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task<int> InsertCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }
}