using StudyBuddy.API.DTOs.MatchDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.MatchRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _repository;

        public MatchService(IMatchRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MatchListDto>> GetAllMatchesAsync()
        {
            var list = await _repository.GetAll();

            return list.Select(x => new MatchListDto
            {
                MatchId = x.MatchId,
                User1Id = x.User1Id,
                User2Id = x.User2Id,
                MatchPercent = x.MatchPercent,
                MatchDate = x.MatchDate
            }).ToList();
        }

        public async Task<MatchBaseDto?> GetMatchByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null) return null;

            return new MatchBaseDto
            {
                User1Id = entity.User1Id,
                User2Id = entity.User2Id,
                MatchPercent = entity.MatchPercent,
                MatchDate = entity.MatchDate
            };
        }

        public async Task<int> CreateMatchAsync(MatchCreateRequest request)
        {
            var entity = new Match
            {
                User1Id = request.User1Id,
                User2Id = request.User2Id,
                MatchPercent = request.MatchPercent,
                MatchDate = request.MatchDate
            };

            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateMatchAsync(MatchUpdateRequest request)
        {
            var existing = await _repository.GetById(request.MatchId);
            if (existing == null) return false;

            existing.User1Id = request.User1Id;
            existing.User2Id = request.User2Id;
            existing.MatchPercent = request.MatchPercent;
            existing.MatchDate = request.MatchDate;

            return await _repository.Update(existing);
        }

        public async Task<bool> DeleteMatchAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing == null) return false;

            return await _repository.Delete(id);
        }
    }
}
