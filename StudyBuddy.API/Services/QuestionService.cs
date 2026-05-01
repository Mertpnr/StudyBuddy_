using AutoMapper;
using StudyBuddy.API.DTOs.QuestionDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.QuestionRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _repository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<QuestionListDto>> GetAllQuestionsAsync()
        {
            var entities = await _repository.GetAll();
            return _mapper.Map<List<QuestionListDto>>(entities);
        }

        public async Task<QuestionBaseDto?> GetQuestionByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            return entity is null ? null : _mapper.Map<QuestionBaseDto>(entity);
        }

        public async Task<int> CreateQuestionAsync(QuestionCreateRequest request)
        {
            var entity = _mapper.Map<Question>(request);
            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateQuestionAsync(QuestionUpdateRequest request)
        {
            var existing = await _repository.GetById(request.QuestionId);
            if (existing is null) return false;

            var entity = _mapper.Map<Question>(request);
            entity.QuestionId = request.QuestionId;

            return await _repository.Update(entity);
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing is null) return false;

            return await _repository.Delete(id);
        }
    }
}