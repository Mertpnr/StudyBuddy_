using Microsoft.AspNetCore.Mvc;
using StudyBuddy.WEB.Models.User;
using StudyBuddy.WEB.Services.Interfaces;

namespace StudyBuddy.WEB.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserWebService _userWebService;

        public ProfileController(IUserWebService userWebService)
        {
            _userWebService = userWebService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return RedirectToAction("Login", "Auth");

            var userId = int.Parse(userIdText);

            var user = await _userWebService.GetUserByIdAsync(userId);

            if (user == null)
                return RedirectToAction("Login", "Auth");

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userIdText = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdText))
                return RedirectToAction("Login", "Auth");

            var userId = int.Parse(userIdText);

            var user = await _userWebService.GetUserByIdAsync(userId);

            if (user == null)
                return RedirectToAction("Index");

            var model = new UserUpdateViewModel
            {
                UserId = user.UserId,
                NameSurname = user.NameSurname,
                Email = user.Email,
                AboutMe = user.AboutMe,
                University = user.University,
                Major = user.Major
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userWebService.UpdateUserAsync(model);

            if (!result)
            {
                ModelState.AddModelError("", "Profile update failed.");
                return View(model);
            }

            HttpContext.Session.SetString("UserName", model.NameSurname);
            HttpContext.Session.SetString("UserEmail", model.Email);

            TempData["SuccessMessage"] = "Profile updated successfully.";

            return RedirectToAction("Index");
        }
    }
}
