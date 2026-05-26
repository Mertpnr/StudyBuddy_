using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace StudyBuddy.SystemTests.Infrastructure;

public class SystemTestBase : PageTest
{
    protected string WebUrl =>
        Environment.GetEnvironmentVariable("STUDYBUDDY_WEB_URL")
        ?? "https://localhost:57925";

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            BaseURL = WebUrl,
            IgnoreHTTPSErrors = true,
            ViewportSize = new ViewportSize
            {
                Width = 1440,
                Height = 1000
            }
        };
    }

    protected async Task GoToAsync(string path)
    {
        await Page.GotoAsync($"{WebUrl}{path}");
    }
}
