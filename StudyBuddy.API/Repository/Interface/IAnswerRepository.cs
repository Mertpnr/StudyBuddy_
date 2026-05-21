using StudyBuddy.API.Model;

namespace StudyBuddy.API.Repository.Interface
{
    public interface IAnswerRepository: IGenericRepository<Answer, int>
    {
        Task<Answer?> GetByUserAndQuestionAsync(int userId, int questionId);
    }
}