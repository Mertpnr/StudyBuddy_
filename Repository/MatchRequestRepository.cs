using System.Collections.Generic;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class MatchRequestRepository : IMatchRequestRepository
    {
        private readonly IGenericRepository<MatchRequest> _genericRepository;

        public MatchRequestRepository(IGenericRepository<MatchRequest> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<bool> DeleteMatchRequest(int id)
        {
            return await _genericRepository.Delete(id);
        }

        public async Task<List<MatchRequest>> GetAllMatchRequests()
        {
            return await _genericRepository.GetAll();
        }

        public async Task<MatchRequest> GetMatchRequestById(int id)
        {
            return await _genericRepository.GetById(id);
        }

        public async Task<int> InsertMatchRequest(MatchRequest matchRequest)
        {
            return await _genericRepository.Insert(matchRequest);
        }

        public async Task<bool> UpdateMatchRequest(MatchRequest matchRequest)
        {
            return await _genericRepository.Update(matchRequest);
        }
    }
}