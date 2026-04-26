using StudyBuddy.API.DTOs.AnswerDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.AnswerRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _repository;

        public AnswerService(IAnswerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AnswerListDto>> GetAllAnswersAsync()
        {
            var list = await _repository.GetAll();

            return list.Select(x => new AnswerListDto
            {
                AnswerId = x.AnswerId,
                QuestionId = x.QuestionId,
                OptionId = x.OptionId,
                UserId = x.UserId
            }).ToList();
        }

        public async Task<AnswerBaseDto?> GetAnswerByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null) return null;

            return new AnswerBaseDto
            {
                QuestionId = entity.QuestionId,
                OptionId = entity.OptionId,
                UserId = entity.UserId
            };
        }

        public async Task<int> CreateAnswerAsync(AnswerCreateRequest request)
        {
            var entity = new Answer
            {
                QuestionId = request.QuestionId,
                OptionId = request.OptionId,
                UserId = request.UserId
            };

            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateAnswerAsync(AnswerUpdateRequest request)
        {
            var existing = await _repository.GetById(request.AnswerId);
            if (existing == null) return false;

            existing.QuestionId = request.QuestionId;
            existing.OptionId = request.OptionId;
            existing.UserId = request.UserId;

            return await _repository.Update(existing);
        }

        public async Task<bool> DeleteAnswerAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing == null) return false;

            return await _repository.Delete(id);
        }
    }
}
