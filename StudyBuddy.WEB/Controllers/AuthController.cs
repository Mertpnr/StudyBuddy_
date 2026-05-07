using Microsoft.AspNetCore.Mvc;
using StudyBuddy.WEB.Models.Auth;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthWebService _authWebService;

        public AuthController(IAuthWebService authWebService)
        {
            _authWebService = authWebService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authWebService.RegisterAsync(model);

            if (!result)
            {
                ModelState.AddModelError("", "Registration failed. Email may already be registered.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authWebService.LoginAsync(model);

            if (result == null || !result.Success)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetString("UserId", result.UserId.ToString());
            HttpContext.Session.SetString("UserGuid", result.UserGuid.ToString());
            HttpContext.Session.SetString("UserName", result.NameSurname);
            HttpContext.Session.SetString("UserEmail", result.Email);

            if (!string.IsNullOrEmpty(result.Token))
            {
                HttpContext.Session.SetString("JwtToken", result.Token);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
