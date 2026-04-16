using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
	public class MatchService : IMatchService
	{
		private readonly IMatchRepository _matchRepository;

		public MatchService(IMatchRepository matchRepository)
		{
			_matchRepository = matchRepository;
		}

		public async Task<List<Match>> GetAllMatches()
		{
			return await _matchRepository.GetAllMatches();
		}

		public async Task<Match> GetMatchById(int id)
		{
			return await _matchRepository.GetMatchById(id);
		}

		public async Task<int> InsertMatch(Match match)
		{
			var id = await _matchRepository.InsertMatch(match);
			return id;
		}

		public async Task<bool> UpdateMatch(Match match)
		{
			var updated = await _matchRepository.UpdateMatch(match);
			return updated;
		}

		public async Task<bool> DeleteMatch(int id)
		{
			var deleted = await _matchRepository.DeleteMatch(id);
			return deleted;
		}
	}
}