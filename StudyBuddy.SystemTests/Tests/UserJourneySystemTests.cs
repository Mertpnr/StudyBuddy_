using Microsoft.Playwright;
using StudyBuddy.SystemTests.Infrastructure;
using StudyBuddy.SystemTests.Pages;

namespace StudyBuddy.SystemTests.Tests;

public class UserJourneySystemTests : SystemTestBase
{
    [Test]
    public async Task Register_Login_Dashboard_Profile_Quiz_And_Match_Candidates_Should_Work()
    {
        var email = TestData.UniqueEmail("journey");
        var password = TestData.DefaultPassword;
        var name = $"System Test User {DateTime.Now:HHmmss}";

        await new RegisterPage(Page).RegisterAsync(name, email, password);

        await Expect(Page.GetByText("Registration successful")).ToBeVisibleAsync();

        await new LoginPage(Page).LoginAsync(email, password);

        await Expect(Page.GetByText("Dashboard").First).ToBeVisibleAsync();

        await GoToAsync(TestRoutes.Dashboard);
        await Expect(Page.Locator("body")).ToContainTextAsync("Dashboard");

        var updatedName = $"Updated System User {DateTime.Now:HHmmss}";
        await new ProfilePage(Page).UpdateProfileAsync(updatedName);

        await Expect(Page.Locator("body")).ToContainTextAsync(updatedName);

        await GoToAsync(TestRoutes.Quiz);

        await new QuizPage(Page).CompleteQuizAsync();

        await Expect(Page.Locator("body")).ToBeVisibleAsync();

        await new MatchPage(Page).OpenAsync();

        await Expect(Page.GetByText("Find a study buddy")).ToBeVisibleAsync();
        await Expect(Page.Locator("body")).ToContainTextAsync("People you may study well with");
    }
}
