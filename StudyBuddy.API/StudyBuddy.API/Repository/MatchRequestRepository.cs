using Dapper;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class MatchRequestRepository : GenericRepository<MatchRequest, int>, IMatchRequestRepository
    {
        private readonly IDbConnectionFactory _context;

        public MatchRequestRepository(IDbConnectionFactory context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MatchRequest>> GetByUserIdAsync(int userId)
        {
            const string query = @"
SELECT *
FROM MatchRequests
WHERE User1Id = @UserId OR User2Id = @UserId
ORDER BY CreatedDate DESC, MatchRequestId DESC";

            using var con = _context.CreateConnection();
            return await con.QueryAsync<MatchRequest>(query, new { UserId = userId });
        }

        public async Task<MatchRequest?> GetActiveBetweenUsersAsync(int user1Id, int user2Id)
        {
            const string query = @"
SELECT TOP 1 *
FROM MatchRequests
WHERE Status IN (0, 1)
  AND (
      (User1Id = @User1Id AND User2Id = @User2Id)
      OR (User1Id = @User2Id AND User2Id = @User1Id)
  )
ORDER BY
    CASE WHEN Status = 1 THEN 0 ELSE 1 END,
    CreatedDate DESC,
    MatchRequestId DESC";

            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<MatchRequest>(query, new
            {
                User1Id = user1Id,
                User2Id = user2Id
            });
        }
    }
}