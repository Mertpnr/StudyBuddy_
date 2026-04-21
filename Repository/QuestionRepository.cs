using StudyBuddy.API.DbConnecitionFactory;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class QuestionRepository : GenericRepository<Question, int>, IQuestionRepository
    {
        public QuestionRepository(IDbConnectionFactory context) : base(context)
        {
        }
    }
}
