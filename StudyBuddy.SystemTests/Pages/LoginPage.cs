using Microsoft.Playwright;

namespace StudyBuddy.SystemTests.Pages;

public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    public async Task LoginAsync(string email, string password)
    {
        await _page.GotoAsync("/Auth/Login");

        await _page.Locator("input[name='Email']").FillAsync(email);
        await _page.Locator("input[name='Password']").FillAsync(password);
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
    }
}
