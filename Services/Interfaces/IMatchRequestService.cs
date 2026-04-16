using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.API.Model;

namespace StudyBuddy.API.Services.Interface
{
    public interface IMatchRequestService
    {
        Task<List<MatchRequest>> GetAllMatchRequests();
        Task<MatchRequest> GetMatchRequestById(int id);
        Task<int> InsertMatchRequest(MatchRequest matchRequest);
        Task<bool> UpdateMatchRequest(MatchRequest matchRequest);
        Task<bool> DeleteMatchRequest(int id);
    }
}