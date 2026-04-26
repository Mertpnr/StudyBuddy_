using AutoMapper;
using StudyBuddy.API.DTOs.OptionDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.OptionRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class OptionService : IOptionService
    {
        private readonly IOptionRepository _repository;
        private readonly IMapper _mapper;

        public OptionService(IOptionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OptionListDto>> GetAllOptionsAsync()
        {
            var entities = await _repository.GetAll();
            return _mapper.Map<List<OptionListDto>>(entities);
        }

        public async Task<OptionBaseDto?> GetOptionByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            return entity is null ? null : _mapper.Map<OptionBaseDto>(entity);
        }

        public async Task<int> CreateOptionAsync(OptionCreateRequest request)
        {
            var entity = _mapper.Map<Option>(request);
            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateOptionAsync(OptionUpdateRequest request)
        {
            var existing = await _repository.GetById(request.OptionId);
            if (existing is null) return false;

            var entity = _mapper.Map<Option>(request);
            entity.OptionId = request.OptionId;

            return await _repository.Update(entity);
        }

        public async Task<bool> DeleteOptionAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing is null) return false;

            return await _repository.Delete(id);
        }
    }
}