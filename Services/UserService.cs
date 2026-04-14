using StudyBuddy.API.Model;

using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> DeleteUser(int id)
        {
            return await _userRepository.DeleteUser(id);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }


        public async Task<int> InsertUser(User user)
        {
            return await _userRepository.InsertUser(user);
        }

        public async Task<bool> UpdateUser(User user)
        {
            return await _userRepository.UpdateUser(user);
        }
    }
}
