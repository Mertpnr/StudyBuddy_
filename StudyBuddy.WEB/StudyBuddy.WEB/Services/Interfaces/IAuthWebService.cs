using StudyBuddy.WEB.Models.Auth;

namespace StudyBuddy.WEB.Services.Interfaces
{
	public interface IAuthWebService
	{
		Task<bool> RegisterAsync(RegisterViewModel model);

		Task<LoginResponseViewModel?> LoginAsync(LoginViewModel model);
	}
}
