using Dapper;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class ChatRepository : GenericRepository<Chat, int>, IChatRepository
    {
        private readonly IDbConnectionFactory _context;

        public ChatRepository(IDbConnectionFactory context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Chat>> GetByMatchRequestIdAsync(int matchRequestId)
        {
            const string query = @"
SELECT *
FROM Chat
WHERE MatchRequestId = @MatchRequestId
ORDER BY Date ASC, ChatId ASC";

            using var con = _context.CreateConnection();
            return await con.QueryAsync<Chat>(query, new { MatchRequestId = matchRequestId });
        }

        public async Task<bool> UserCanAccessChatAsync(int matchRequestId, int userId)
        {
            const string query = @"
SELECT COUNT(1)
FROM MatchRequests
WHERE MatchRequestId = @MatchRequestId
  AND (User1Id = @UserId OR User2Id = @UserId)";

            using var con = _context.CreateConnection();

            var count = await con.ExecuteScalarAsync<int>(query, new
            {
                MatchRequestId = matchRequestId,
                UserId = userId
            });

            return count > 0;
        }

        public async Task<int> AddMessageAsync(Chat chat)
        {
            const string query = @"
DECLARE @NewChatId INT;

SELECT @NewChatId = ISNULL(MAX(ChatId), 0) + 1
FROM Chat;

INSERT INTO Chat (ChatId, MatchRequestId, UserId, [Message], [Date])
VALUES (@NewChatId, @MatchRequestId, @UserId, @Message, @Date);

SELECT @NewChatId;";

            using var con = _context.CreateConnection();

            return await con.ExecuteScalarAsync<int>(query, new
            {
                chat.MatchRequestId,
                chat.UserId,
                chat.Message,
                chat.Date
            });
        }

        public async Task<bool> UpdateMessageAsync(Chat chat)
        {
            const string query = @"
UPDATE Chat
SET Message = @Message
WHERE ChatId = @ChatId
  AND UserId = @UserId";

            using var con = _context.CreateConnection();
            var affectedRows = await con.ExecuteAsync(query, chat);

            return affectedRows > 0;
        }
    }
}