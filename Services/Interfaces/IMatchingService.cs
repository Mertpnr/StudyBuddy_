using StudyBuddy.API.DTOs.MatchingDto;
using StudyBuddy.API.Requests.MatchingRequest;

namespace StudyBuddy.Services.Interfaces
{
	public interface IMatchingService
	{
		Task<MatchResultDto> CalculateMatchAsync(CalculateMatchRequest request);
	}
}
