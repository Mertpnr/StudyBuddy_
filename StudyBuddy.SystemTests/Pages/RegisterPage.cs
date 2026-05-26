using Microsoft.Playwright;

namespace StudyBuddy.SystemTests.Pages;

public class RegisterPage
{
    private readonly IPage _page;

    public RegisterPage(IPage page)
    {
        _page = page;
    }

    public async Task RegisterAsync(
        string nameSurname,
        string email,
        string password,
        string? confirmPassword = null,
        string university = "KTU",
        string major = "Software Engineering",
        string aboutMe = "Created by Playwright system tests.")
    {
        await _page.GotoAsync("/Auth/Register");

        await _page.Locator("input[name='NameSurname']").FillAsync(nameSurname);
        await _page.Locator("input[name='Email']").FillAsync(email);
        await _page.Locator("input[name='Password']").FillAsync(password);
        await _page.Locator("input[name='ConfirmPassword']").FillAsync(confirmPassword ?? password);
        await _page.Locator("input[name='University']").FillAsync(university);
        await _page.Locator("input[name='Major']").FillAsync(major);
        await _page.Locator("textarea[name='AboutMe']").FillAsync(aboutMe);

        await _page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
    }
}
