using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        public UserRepository(IDbConnectionFactory context) : base(context)
        {
        }
    }
}
