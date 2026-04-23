using StudyBuddy.API.DTOs.MatchingDto;
using StudyBuddy.API.Model;

namespace StudyBuddy.API.Repository.Interface
{
    public interface IMatchRepository : IGenericRepository<Match, int>
    {
        Task<List<MatchAnswerCompareDto>> GetComparableAnswersAsync(int user1Id, int user2Id);
        Task<bool> AreSubjectsMatchingAsync(int user1Id, int user2Id, int subjectQuestionId);
        Task<Match?> GetByUsersAsync(int user1Id, int user2Id);
    }
}
