using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.API.Model;

namespace StudyBuddy.API.Services.Interface
{
    public interface IQuestionService
    {
        Task<List<Question>> GetAllQuestions();
        Task<Question> GetQuestionById(int id);
        Task<int> InsertQuestion(Question question);
        Task<bool> UpdateQuestion(Question question);
        Task<bool> DeleteQuestion(int id);
    }
}