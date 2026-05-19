using StudyBuddy.WEB.Models.Match;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Services
{
    public class MatchWebService : IMatchWebService
    {
        private readonly IApiClientService _apiClientService;

        public MatchWebService(IApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<MatchResultViewModel?> CalculateMatchAsync(CalculateMatchViewModel model)
        {
            return await _apiClientService.PostAsync<CalculateMatchViewModel, MatchResultViewModel>(
                "Matching/Calculate",
                model
            );
        }
    }
}
