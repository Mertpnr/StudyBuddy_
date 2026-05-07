using StudyBuddy.WEB.Models.Auth;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Services
{
	public class AuthWebService : IAuthWebService
	{
		private readonly IApiClientService _apiClientService;

		public AuthWebService(IApiClientService apiClientService)
		{
			_apiClientService = apiClientService;
		}

		public async Task<bool> RegisterAsync(RegisterViewModel model)
		{
			var request = new
			{
				nameSurname = model.NameSurname,
				email = model.Email,
				password = model.Password,
				aboutMe = model.AboutMe,
				university = model.University,
				major = model.Major
			};

			var createdId = await _apiClientService.PostAsync<object, int?>("Users", request);

			return createdId.HasValue && createdId.Value > 0;
		}

		public async Task<LoginResponseViewModel?> LoginAsync(LoginViewModel model)
		{
			var request = new
			{
				email = model.Email,
				password = model.Password
			};

			return await _apiClientService.PostAsync<object, LoginResponseViewModel>(
				"Auth/login",
				request
			);
		}
	}
}
