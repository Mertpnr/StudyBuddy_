using StudyBuddy.API.DTOs.UserDto;
using StudyBuddy.API.Requests.UserRequest;

namespace StudyBuddy.API.Services.Interface
{
    public interface IUserService
    {
        Task<List<UserListDto>> GetAllUsersAsync();

        Task<UserBaseDto?> GetUserByIdAsync(int id);

        Task<UserBaseDto?> GetUserByGuidAsync(Guid userGuid);

        Task<int> CreateUserAsync(UserCreateRequest request);

        Task<bool> UpdateUserAsync(UserUpdateRequest request);

        Task<bool> DeleteUserAsync(int id);
    }
}