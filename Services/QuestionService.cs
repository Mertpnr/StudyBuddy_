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

        public QuestionService(IQuestionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<QuestionListDto>> GetAllQuestionsAsync()
        {
            var list = await _repository.GetAll();

            return list.Select(x => new QuestionListDto
            {
                QuestionId = x.QuestionId,
                Question = x.Question1 ?? string.Empty,
                CategoryId = x.CategoryId,
                MatchPercent = x.MatchPercent
            }).ToList();
        }

        public async Task<QuestionBaseDto?> GetQuestionByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null) return null;

            return new QuestionBaseDto
            {
                Question = entity.Question1 ?? string.Empty,
                CategoryId = entity.CategoryId,
                MatchPercent = entity.MatchPercent
            };
        }

        public async Task<int> CreateQuestionAsync(QuestionCreateRequest request)
        {
            var entity = new Question
            {
                Question1 = request.Question,
                CategoryId = request.CategoryId,
                MatchPercent = request.MatchPercent
            };

            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateQuestionAsync(QuestionUpdateRequest request)
        {
            var existing = await _repository.GetById(request.QuestionId);
            if (existing == null) return false;

            existing.Question1 = request.Question;
            existing.CategoryId = request.CategoryId;
            existing.MatchPercent = request.MatchPercent;

            return await _repository.Update(existing);
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing == null) return false;

            return await _repository.Delete(id);
        }
    }
}
