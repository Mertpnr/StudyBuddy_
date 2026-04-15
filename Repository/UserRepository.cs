using Dapper;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using StudyBuddy.API.DbConnecitionFactory;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using System.Data.SqlClient;

namespace StudyBuddy.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly GenericRepository<User> _repository;
        private readonly IConfiguration _configuration;
        private readonly IDbConnectionFactory _context;
        public UserRepository(IDbConnectionFactory context, IConfiguration configuration, GenericRepository<User> repository)
        {
            _context = context;
            _configuration = configuration;
            _repository = repository;
        }
        public async Task<bool> DeleteUser(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _repository.GetAll();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _repository.GetById(id);
        }
         
        public async Task<int> InsertUser(User user)
        {
            return await _repository.Insert(user);
        }

        public async Task<bool> UpdateUser(User user)
        {
            return await _repository.Update(user);
        }
    }
}