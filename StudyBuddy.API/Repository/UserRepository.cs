using Dapper;
using StudyBuddy.API.DbConnectionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;

namespace StudyBuddy.API.Repository
{
    public class UserRepository : GenericRepository<User, int>, IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string query = @"
                SELECT *
                FROM Users
                WHERE LOWER(Email) = LOWER(@Email)";

            using var connection = _dbConnectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<User>(query, new
            {
                Email = email
            });
        }

        public async Task<User?> GetByGuidAsync(Guid userGuid)
        {
            const string query = @"
                SELECT *
                FROM Users
                WHERE UserGuid = @UserGuid";

            using var connection = _dbConnectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<User>(query, new
            {
                UserGuid = userGuid
            });
        }
    }
}