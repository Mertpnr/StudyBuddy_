using StudyBuddy.WEB.Models.User;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Services
{
	public class UserWebService : IUserWebService
	{
		private readonly IApiClientService _apiClientService;

		public UserWebService(IApiClientService apiClientService)
		{
			_apiClientService = apiClientService;
		}

		public async Task<List<UserProfileViewModel>> GetAllUsersAsync()
		{
			return await _apiClientService.GetAsync<List<UserProfileViewModel>>("Users")
				   ?? new List<UserProfileViewModel>();
		}

		public async Task<UserProfileViewModel?> GetUserByIdAsync(int userId)
		{
			return await _apiClientService.GetAsync<UserProfileViewModel>($"Users/{userId}");
		}

		public async Task<UserProfileViewModel?> GetUserByGuidAsync(Guid userGuid)
		{
			return await _apiClientService.GetAsync<UserProfileViewModel>($"Users/guid/{userGuid}");
		}

		public async Task<bool> UpdateUserAsync(UserUpdateViewModel model)
		{
			var request = new
			{
				userId = model.UserId,
				nameSurname = model.NameSurname,
				email = model.Email,
				aboutMe = model.AboutMe,
				university = model.University,
				major = model.Major
			};

			return await _apiClientService.PutAsync("Users", request);
		}
	}
}
