using StudyBuddy.API.DbConnecitionFactory;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class MatchRepository : GenericRepository<Match, int>, IMatchRepository
    {
        public MatchRepository(IDbConnectionFactory context) : base(context)
        {
        }
    }
}
