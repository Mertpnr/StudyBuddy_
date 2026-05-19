using StudyBuddy.WEB.Models.Match;

namespace StudyBuddy.WEB.Services.Interfaces
{
    public interface IMatchWebService
    {
        Task<MatchResultViewModel?> CalculateMatchAsync(CalculateMatchViewModel model);
    }
}
