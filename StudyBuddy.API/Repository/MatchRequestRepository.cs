using Dapper;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class MatchRequestRepository : GenericRepository<MatchRequest, int>, IMatchRequestRepository
    {
        public MatchRequestRepository(IDbConnectionFactory context) : base(context)
        {
        }
    }
}