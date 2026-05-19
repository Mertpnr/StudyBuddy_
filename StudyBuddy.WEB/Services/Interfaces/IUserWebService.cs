using StudyBuddy.WEB.Models.User;

namespace StudyBuddy.WEB.Services.Interfaces
{
    public interface IUserWebService
    {
        Task<List<UserProfileViewModel>> GetAllUsersAsync();

        Task<UserProfileViewModel?> GetUserByIdAsync(int userId);

        Task<UserProfileViewModel?> GetUserByGuidAsync(Guid userGuid);

        Task<bool> UpdateUserAsync(UserUpdateViewModel model);
    }
}
