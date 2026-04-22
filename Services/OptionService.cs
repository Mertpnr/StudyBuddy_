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

        public OptionService(IOptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OptionListDto>> GetAllOptionsAsync()
        {
            var list = await _repository.GetAll();

            return list.Select(x => new OptionListDto
            {
                OptionId = x.OptionId,
                QuestionId = x.QuestionId,
                Text = x.Text ?? string.Empty,
                Value = x.Value,
                OrderNo = x.OrderNo
            }).ToList();
        }

        public async Task<OptionBaseDto?> GetOptionByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null) return null;

            return new OptionBaseDto
            {
                QuestionId = entity.QuestionId,
                Text = xSafe(entity.Text),
                Value = entity.Value,
                OrderNo = entity.OrderNo
            };
        }

        public async Task<int> CreateOptionAsync(OptionCreateRequest request)
        {
            var entity = new Option
            {
                QuestionId = request.QuestionId,
                Text = request.Text,
                Value = request.Value,
                OrderNo = request.OrderNo
            };

            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateOptionAsync(OptionUpdateRequest request)
        {
            var existing = await _repository.GetById(request.OptionId);
            if (existing == null) return false;

            existing.QuestionId = request.QuestionId;
            existing.Text = request.Text;
            existing.Value = request.Value;
            existing.OrderNo = request.OrderNo;

            return await _repository.Update(existing);
        }

        public async Task<bool> DeleteOptionAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing == null) return false;

            return await _repository.Delete(id);
        }

        private static string xSafe(string? value) => value ?? string.Empty;
    }
}
