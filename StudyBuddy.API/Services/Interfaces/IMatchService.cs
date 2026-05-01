using StudyBuddy.API.DTOs.MatchDto;
using StudyBuddy.API.Requests.MatchRequest;

namespace StudyBuddy.API.Services.Interface
{
    public interface IMatchService
    {
        Task<List<MatchListDto>> GetAllMatchesAsync();
        Task<MatchBaseDto?> GetMatchByIdAsync(int id);
        Task<int> CreateMatchAsync(MatchCreateRequest request);
        Task<bool> UpdateMatchAsync(MatchUpdateRequest request);
        Task<bool> DeleteMatchAsync(int id);
    }
}