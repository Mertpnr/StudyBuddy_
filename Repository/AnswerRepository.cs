using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class AnswerRepository : GenericRepository<Answer, int>, IAnswerRepository
    {
        public AnswerRepository(IDbConnectionFactory context) : base(context)
        {
        }
    }
}