using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.API.Model;

namespace StudyBuddy.API.Services.Interface
{
	public interface IMatchService
	{
		Task<List<Match>> GetAllMatches();
		Task<Match> GetMatchById(int id);
		Task<int> InsertMatch(Match match);
		Task<bool> UpdateMatch(Match match);
		Task<bool> DeleteMatch(int id);
	}
}