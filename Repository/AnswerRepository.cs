using System.Collections.Generic;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
namespace StudyBuddy.API.Repository
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly IGenericRepository<Answer> _genericRepository;
        public AnswerRepository(IGenericRepository<Answer> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<bool> DeleteAnswer(int answerId)
        {
            return await _genericRepository.Delete(answerId);
        }

        public async Task<List<Answer>> GetAllAswers()
        {
            return await _genericRepository.GetAll();
        }

        public async Task<Answer> GetAnswerId(int answerId)
        {
            return await _genericRepository.GetById(answerId);
        }

        public async Task<int> InsertAnswer(Answer answer)
        {
            return await _genericRepository.Insert(answer);
        }

        public async Task<bool> UpdateAnswer(Answer answer)
        {
            return await _genericRepository.Update(answer);
        }
    }
}