using StudyBuddy.API.DTOs.MatchingDto;
using StudyBuddy.API.Requests.MatchingRequest;

namespace StudyBuddy.API.Services.Interface
{
    public interface IMatchingService
    {
        Task<MatchResultDto> CalculateMatchAsync(CalculateMatchRequest request);
    }
}