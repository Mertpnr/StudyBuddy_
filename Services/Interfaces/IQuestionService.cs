using StudyBuddy.API.DTOs.QuestionDto;
using StudyBuddy.API.Requests.QuestionRequest;

namespace StudyBuddy.API.Services.Interface
{
    public interface IQuestionService
    {
        Task<List<QuestionListDto>> GetAllQuestionsAsync();
        Task<QuestionBaseDto?> GetQuestionByIdAsync(int id);
        Task<int> CreateQuestionAsync(QuestionCreateRequest request);
        Task<bool> UpdateQuestionAsync(QuestionUpdateRequest request);
        Task<bool> DeleteQuestionAsync(int id);
    }
}
