using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAll();
            return categories.ToList();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public async Task<int> InsertCategory(Category category)
        {
            return await _categoryRepository.InsertReturnId(category);
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            return await _categoryRepository.Update(category);
        }

        public async Task<bool> DeleteCategory(int id)
        {
            return await _categoryRepository.Delete(id);
        }
    }
}