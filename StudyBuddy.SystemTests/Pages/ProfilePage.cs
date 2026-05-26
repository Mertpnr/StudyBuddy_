using Microsoft.Playwright;

namespace StudyBuddy.SystemTests.Pages;

public class ProfilePage
{
    private readonly IPage _page;

    public ProfilePage(IPage page)
    {
        _page = page;
    }

    public async Task UpdateProfileAsync(string displayName)
    {
        await _page.GotoAsync("/Profile/Edit");

        await _page.Locator("input[name='NameSurname']").FillAsync(displayName);
        await _page.Locator("input[name='University']").FillAsync("KTU");
        await _page.Locator("input[name='Major']").FillAsync("Software Engineering");
        await _page.Locator("textarea[name='AboutMe']").FillAsync("Updated by Playwright system tests.");

        await _page.GetByRole(AriaRole.Button, new() { Name = "Save Changes" }).ClickAsync();
    }

    public async Task SubmitEmptyRequiredFieldsAsync()
    {
        await _page.GotoAsync("/Profile/Edit");

        await _page.Locator("input[name='NameSurname']").FillAsync("");
        await _page.Locator("input[name='Email']").FillAsync("");

        await _page.GetByRole(AriaRole.Button, new() { Name = "Save Changes" }).ClickAsync();
    }
}
