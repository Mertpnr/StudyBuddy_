using StudyBuddy.API.Model;

namespace StudyBuddy.API.Repository.Interface
{
    public interface IMatchRequestRepository : IGenericRepository<MatchRequest, int>
    {
        Task<IEnumerable<MatchRequest>> GetByUserIdAsync(int userId);
        Task<MatchRequest?> GetActiveBetweenUsersAsync(int user1Id, int user2Id);
    }
}