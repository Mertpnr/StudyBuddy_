using System.Collections.Generic;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IGenericRepository<Question> _genericRepository;

        public QuestionRepository(IGenericRepository<Question> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<bool> DeleteQuestion(int id)
        {
            return await _genericRepository.Delete(id);
        }

        public async Task<List<Question>> GetAllQuestions()
        {
            return await _genericRepository.GetAll();
        }

        public async Task<Question> GetQuestionById(int id)
        {
            return await _genericRepository.GetById(id);
        }

        public async Task<int> InsertQuestion(Question question)
        {
            return await _genericRepository.Insert(question);
        }

        public async Task<bool> UpdateQuestion(Question question)
        {
            return await _genericRepository.Update(question);
        }
    }
}