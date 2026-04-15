using System.Collections.Generic;
using StudyBuddy.API.Model;

namespace StudyBuddy.API.Repository.Interface
{
    public interface IMatchRepository
    {
        Task<List<Match>> GetAllMatches();
        Task<Match> GetMatchById(int id);
        Task<int> InsertMatch(Match match);
        Task<bool> UpdateMatch(Match match);
        Task<bool> DeleteMatch(int id);
    }
}