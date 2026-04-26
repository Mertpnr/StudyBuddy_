using StudyBuddy.API.DTOs.UserDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Repository.Interface;
using StudyBuddy.API.Requests.UserRequest;
using StudyBuddy.API.Services.Interface;

namespace StudyBuddy.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UserListDto>> GetAllUsersAsync()
        {
            var list = await _repository.GetAll();

            return list.Select(x => new UserListDto
            {
                UserId = x.UserId,
                NameSurname = x.NameSurname,
                Email = x.Email,
                AboutMe = x.AboutMe,
                University = x.University,
                Major = x.Major,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            }).ToList();
        }

        public async Task<UserBaseDto?> GetUserByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null) return null;

            return new UserBaseDto
            {
                NameSurname = entity.NameSurname,
                Email = entity.Email,
                Password = entity.Password,
                AboutMe = entity.AboutMe,
                University = entity.University,
                Major = entity.Major,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public async Task<int> CreateUserAsync(UserCreateRequest request)
        {
            var entity = new User
            {
                NameSurname = request.NameSurname,
                Email = request.Email ?? string.Empty,
                Password = request.Password,
                AboutMe = request.AboutMe ?? string.Empty,
                University = request.University,
                Major = request.Major ?? string.Empty,
                CreatedDate = request.CreatedDate,
                UpdatedDate = request.UpdatedDate ?? DateTime.Now
            };

            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateUserAsync(UserUpdateRequest request)
        {
            var existing = await _repository.GetById(request.UserId);
            if (existing == null) return false;

            existing.NameSurname = request.NameSurname;
            existing.Email = request.Email ?? string.Empty;
            existing.Password = request.Password;
            existing.AboutMe = request.AboutMe ?? string.Empty;
            existing.University = request.University;
            existing.Major = request.Major ?? string.Empty;
            existing.UpdatedDate = request.UpdatedDate ?? DateTime.Now;

            return await _repository.Update(existing);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var existing = await _repository.GetById(id);
            if (existing == null) return false;

            return await _repository.Delete(id);
        }
    }
}