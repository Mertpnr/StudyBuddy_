using System.Collections.Generic;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private readonly IGenericRepository<Match> _genericRepository;

        public MatchRepository(IGenericRepository<Match> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<bool> DeleteMatch(int id)
        {
            return await _genericRepository.Delete(id);
        }

        public async Task<List<Match>> GetAllMatches()
        {
            return await _genericRepository.GetAll();
        }

        public async Task<Match> GetMatchById(int id)
        {
            return await _genericRepository.GetById(id);
        }

        public async Task<int> InsertMatch(Match match)
        {
            return await _genericRepository.Insert(match);
        }

        public async Task<bool> UpdateMatch(Match match)
        {
            return await _genericRepository.Update(match);
        }
    }
}