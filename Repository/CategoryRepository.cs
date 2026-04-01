using StudyBuddy.API.DbConnecitionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(IDbConnectionFactory context) : base(context)
        {
        }
    }
}