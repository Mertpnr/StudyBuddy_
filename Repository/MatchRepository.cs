using Dapper;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.DTOs.MatchDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class MatchRepository : GenericRepository<Match, int>, IMatchRepository
    {
        private readonly IDbConnectionFactory _context;

        public MatchRepository(IDbConnectionFactory context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MatchAnswerCompareDto>> GetComparableAnswersAsync(int user1Id, int user2Id)
        {
            const string query = @"
SELECT
    a1.QuestionId,
    CAST(o1.[Value] AS decimal(18,4)) AS User1Value,
    CAST(o2.[Value] AS decimal(18,4)) AS User2Value,
    CAST(q.MatchPercent AS decimal(18,4)) AS QuestionWeight
FROM Answers a1
INNER JOIN Answers a2 
    ON a1.QuestionId = a2.QuestionId
INNER JOIN Options o1 
    ON a1.OptionId = o1.OptionId
INNER JOIN Options o2 
    ON a2.OptionId = o2.OptionId
INNER JOIN Questions q 
    ON a1.QuestionId = q.QuestionId
WHERE a1.UserId = @User1Id
  AND a2.UserId = @User2Id;";

            using var con = _context.CreateConnection();
            var result = await con.QueryAsync<MatchAnswerCompareDto>(query, new
            {
                User1Id = user1Id,
                User2Id = user2Id
            });

            return result.ToList();
        }

        public async Task<bool> AreSubjectsMatchingAsync(int user1Id, int user2Id, int subjectQuestionId)
        {
            const string query = @"
SELECT CASE 
         WHEN a1.OptionId = a2.OptionId THEN CAST(1 AS bit)
         ELSE CAST(0 AS bit)
       END
FROM Answers a1
INNER JOIN Answers a2
    ON a1.QuestionId = a2.QuestionId
WHERE a1.UserId = @User1Id
  AND a2.UserId = @User2Id
  AND a1.QuestionId = @SubjectQuestionId;";

            using var con = _context.CreateConnection();
            var result = await con.QueryFirstOrDefaultAsync<bool?>(query, new
            {
                User1Id = user1Id,
                User2Id = user2Id,
                SubjectQuestionId = subjectQuestionId
            });

            return result ?? false;
        }

        public async Task<Match?> GetByUsersAsync(int user1Id, int user2Id)
        {
            const string query = @"
SELECT TOP 1 *
FROM Matches
WHERE User1Id = @User1Id
  AND User2Id = @User2Id;";

            using var con = _context.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Match>(query, new
            {
                User1Id = user1Id,
                User2Id = user2Id
            });
        }
    }
}
