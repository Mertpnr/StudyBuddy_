using StudyBuddy.API.Model;

namespace StudyBuddy.API.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User, int> 
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByGuidAsync(Guid userGuid);
    }
}
