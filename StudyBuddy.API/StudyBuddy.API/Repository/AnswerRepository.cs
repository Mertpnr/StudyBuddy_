using Dapper;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class AnswerRepository : GenericRepository<Answer, int>, IAnswerRepository
    {
        private readonly IDbConnectionFactory _context;

        public AnswerRepository(IDbConnectionFactory context) : base(context)
        {
            _context = context;
        }

        public async Task<Answer?> GetByUserAndQuestionAsync(int userId, int questionId)
        {
            const string query = @"
SELECT TOP 1 *
FROM Answers
WHERE UserId = @UserId
  AND QuestionId = @QuestionId
ORDER BY AnswerId DESC;";

            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Answer>(query, new
            {
                UserId = userId,
                QuestionId = questionId
            });
        }
    }
}
