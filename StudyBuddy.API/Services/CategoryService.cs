using AutoMapper;
using StudyBuddy.API.DTOs.CategoryDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.CategoryRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CategoryListDto>> GetAllCategoriesAsync()
        {
            var entities = await _repository.GetAll();
            return _mapper.Map<List<CategoryListDto>>(entities);
        }

        public async Task<CategoryBaseDto?> GetCategoryByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            return entity is null ? null : _mapper.Map<CategoryBaseDto>(entity);
        }

        public async Task<int> CreateCategoryAsync(CategoryCreateRequest request)
        {
            var entity = _mapper.Map<Category>(request);
            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateCategoryAsync(CategoryUpdateRequest request)
        {
            var existing = await _repository.GetById(request.CategoryId);
            if (existing is null) return false;

            var entity = _mapper.Map<Category>(request);
            entity.CategoryId = request.CategoryId;

            return await _repository.Update(entity);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing is null) return false;

            return await _repository.Delete(id);
        }
    }
}