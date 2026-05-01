using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class OptionRepository : GenericRepository<Option, int>, IOptionRepository
    {
        public OptionRepository(IDbConnectionFactory context) : base(context)
        {
        }
    }
}
