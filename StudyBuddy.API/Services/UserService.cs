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
                UserGuid = x.UserGuid,
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

            if (entity == null)
                return null;

            return new UserBaseDto
            {
                UserId = entity.UserId,
                UserGuid = entity.UserGuid,
                NameSurname = entity.NameSurname,
                Email = entity.Email,
                AboutMe = entity.AboutMe,
                University = entity.University,
                Major = entity.Major,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public async Task<UserBaseDto?> GetUserByGuidAsync(Guid userGuid)
        {
            var entity = await _repository.GetByGuidAsync(userGuid);

            if (entity == null)
                return null;

            return new UserBaseDto
            {
                UserId = entity.UserId,
                UserGuid = entity.UserGuid,
                NameSurname = entity.NameSurname,
                Email = entity.Email,
                AboutMe = entity.AboutMe,
                University = entity.University,
                Major = entity.Major,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public async Task<int> CreateUserAsync(UserCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NameSurname))
                throw new Exception("Name surname is required.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new Exception("Email is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("Password is required.");

            if (string.IsNullOrWhiteSpace(request.University))
                throw new Exception("University is required.");

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var existingUser = await _repository.GetByEmailAsync(normalizedEmail);

            if (existingUser != null)
                throw new Exception("A user with this email already exists.");

            var entity = new User
            {
                UserGuid = Guid.NewGuid(),
                NameSurname = request.NameSurname,
                Email = normalizedEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                AboutMe = request.AboutMe,
                University = request.University,
                Major = request.Major,
                CreatedDate = DateTime.Now,
                UpdatedDate = null
            };

            return await _repository.InsertReturnId(entity);
        }

        public async Task<bool> UpdateUserAsync(UserUpdateRequest request)
        {
            var existing = await _repository.GetById(request.UserId);

            if (existing == null)
                return false;

            if (string.IsNullOrWhiteSpace(request.NameSurname))
                throw new Exception("Name surname is required.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new Exception("Email is required.");

            if (string.IsNullOrWhiteSpace(request.University))
                throw new Exception("University is required.");

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var emailOwner = await _repository.GetByEmailAsync(normalizedEmail);

            if (emailOwner != null && emailOwner.UserId != request.UserId)
                throw new Exception("A user with this email already exists.");

            existing.NameSurname = request.NameSurname;
            existing.Email = normalizedEmail;
            existing.AboutMe = request.AboutMe;
            existing.University = request.University;
            existing.Major = request.Major;
            existing.UpdatedDate = DateTime.Now;

            // Important:
            // We do not update UserGuid, PasswordHash, or CreatedDate here.
            // Password change should be handled with a separate ChangePassword endpoint later.

            return await _repository.Update(existing);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var existing = await _repository.GetById(id);

            if (existing == null)
                return false;

            return await _repository.Delete(id);
        }
    }
}