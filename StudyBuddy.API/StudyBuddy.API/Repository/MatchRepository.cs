using Dapper;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.DTOs.MatchingDto;
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
WITH LatestAnswers AS
(
    SELECT
        AnswerId,
        UserId,
        QuestionId,
        OptionId,
        ROW_NUMBER() OVER (PARTITION BY UserId, QuestionId ORDER BY AnswerId DESC) AS RowNo
    FROM Answers
    WHERE UserId IN (@User1Id, @User2Id)
),
OptionRanges AS
(
    SELECT
        QuestionId,
        MIN(CAST([Value] AS decimal(18,4))) AS MinValue,
        MAX(CAST([Value] AS decimal(18,4))) AS MaxValue
    FROM Options
    GROUP BY QuestionId
)
SELECT
    a1.QuestionId,
    CASE
        WHEN r.MaxValue = r.MinValue THEN 1
        ELSE (CAST(o1.[Value] AS decimal(18,4)) - r.MinValue) / NULLIF(r.MaxValue - r.MinValue, 0)
    END AS User1Value,
    CASE
        WHEN r.MaxValue = r.MinValue THEN 1
        ELSE (CAST(o2.[Value] AS decimal(18,4)) - r.MinValue) / NULLIF(r.MaxValue - r.MinValue, 0)
    END AS User2Value,
    COALESCE(NULLIF(CAST(q.MatchPercent AS decimal(18,4)), 0), 1) AS QuestionWeight
FROM LatestAnswers a1
INNER JOIN LatestAnswers a2
    ON a1.QuestionId = a2.QuestionId
INNER JOIN Options o1 
    ON a1.OptionId = o1.OptionId
INNER JOIN Options o2 
    ON a2.OptionId = o2.OptionId
INNER JOIN Questions q 
    ON a1.QuestionId = q.QuestionId
INNER JOIN OptionRanges r
    ON a1.QuestionId = r.QuestionId
WHERE a1.UserId = @User1Id
  AND a2.UserId = @User2Id
  AND a1.RowNo = 1
  AND a2.RowNo = 1;";

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
WITH LatestAnswers AS
(
    SELECT
        AnswerId,
        UserId,
        QuestionId,
        OptionId,
        ROW_NUMBER() OVER (PARTITION BY UserId, QuestionId ORDER BY AnswerId DESC) AS RowNo
    FROM Answers
    WHERE UserId IN (@User1Id, @User2Id)
)
SELECT CASE 
         WHEN a1.OptionId = a2.OptionId THEN CAST(1 AS bit)
         ELSE CAST(0 AS bit)
       END
FROM LatestAnswers a1
INNER JOIN LatestAnswers a2
    ON a1.QuestionId = a2.QuestionId
WHERE a1.UserId = @User1Id
  AND a2.UserId = @User2Id
  AND a1.RowNo = 1
  AND a2.RowNo = 1
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
