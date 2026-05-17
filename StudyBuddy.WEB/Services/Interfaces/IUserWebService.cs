using StudyBuddy.WEB.Models.Quiz;

namespace StudyBuddy.WEB.Services.Interfaces
{
    public interface IQuizWebService
    {
        Task<QuizPageViewModel> GetQuizAsync();

        Task<bool> SubmitAnswersAsync(int userId, Dictionary<int, int> selectedOptions);
    }
}
