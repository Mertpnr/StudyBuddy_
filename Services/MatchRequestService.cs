using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Services.Interface;
using StudyBuddy.Repository.Interface;

namespace StudyBuddy.API.Services
{
    public class MatchRequestService : IMatchRequestService
    {
        private readonly IMatchRequestRepository _matchRequestRepository;

        public MatchRequestService(IMatchRequestRepository matchRequestRepository)
        {
            _matchRequestRepository = matchRequestRepository;
        }

        public async Task<List<MatchRequest>> GetAllMatchRequests()
        {
            return await _matchRequestRepository.GetAllMatchRequests();
        }

        public async Task<MatchRequest> GetMatchRequestById(int id)
        {
            return await _matchRequestRepository.GetMatchRequestById(id);
        }

        public async Task<int> InsertMatchRequest(MatchRequest matchRequest)
        {
            var id = await _matchRequestRepository.InsertMatchRequest(matchRequest);
            return id;
        }

        public async Task<bool> UpdateMatchRequest(MatchRequest matchRequest)
        {
            var updated = await _matchRequestRepository.UpdateMatchRequest(matchRequest);
            return updated;
        }

        public async Task<bool> DeleteMatchRequest(int id)
        {
            var deleted = await _matchRequestRepository.DeleteMatchRequest(id);
            return deleted;
        }
    }
}