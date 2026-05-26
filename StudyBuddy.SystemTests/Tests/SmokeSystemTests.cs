using Microsoft.Playwright;
using StudyBuddy.SystemTests.Infrastructure;

namespace StudyBuddy.SystemTests.Tests;

public class SmokeSystemTests : SystemTestBase
{
    [TestCase(TestRoutes.Home)]
    [TestCase(TestRoutes.Register)]
    [TestCase(TestRoutes.Login)]
    public async Task Public_Pages_Should_Load(string route)
    {
        await GoToAsync(route);

        await Expect(Page.Locator("body")).ToBeVisibleAsync();
    }

    [Test]
    public async Task Home_Page_Should_Show_StudyBuddy_Brand()
    {
        await GoToAsync(TestRoutes.Home);

        await Expect(Page.GetByText("StudyBuddy").First).ToBeVisibleAsync();
    }
}
