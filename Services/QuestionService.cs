using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<bool> DeleteQuestion(int id)
        {
            return await _questionRepository.DeleteQuestion(id);
        }

        public async Task<List<Question>> GetAllQuestions()
        {
            return await _questionRepository.GetAllQuestions();
        }

        public async Task<Question> GetQuestionById(int id)
        {
            return await _questionRepository.GetQuestionById(id);
        }

        public async Task<int> InsertQuestion(Question question)
        {
            return await _questionRepository.InsertQuestion(question);
        }

        public async Task<bool> UpdateQuestion(Question question)
        {
            return await _questionRepository.UpdateQuestion(question);
        }
    }
}