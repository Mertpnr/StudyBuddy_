using StudyBuddy.API.DTOs.AnswerDto;
using StudyBuddy.API.Requests.AnswerRequest;

namespace StudyBuddy.API.Services.Interface
{
    public interface IAnswerService
    {
        Task<List<AnswerListDto>> GetAllAnswersAsync();
        Task<AnswerBaseDto?> GetAnswerByIdAsync(int id);
        Task<int> CreateAnswerAsync(AnswerCreateRequest request);
        Task<bool> UpdateAnswerAsync(AnswerUpdateRequest request);
        Task<bool> DeleteAnswerAsync(int id);
    }
}
