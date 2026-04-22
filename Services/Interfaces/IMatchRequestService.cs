using StudyBuddy.API.DTOs.MatchRequestDto;
using StudyBuddy.API.Requests.MatchRequestRequest;

namespace StudyBuddy.API.Services.Interface
{
    public interface IMatchRequestService
    {
        Task<List<MatchRequestListDto>> GetAllMatchRequestsAsync();
        Task<MatchRequestBaseDto?> GetMatchRequestByIdAsync(int id);
        Task<int> CreateMatchRequestAsync(MatchRequestCreateRequest request);
        Task<bool> UpdateMatchRequestAsync(MatchRequestUpdateRequest request);
        Task<bool> DeleteMatchRequestAsync(int id);
    }
}
