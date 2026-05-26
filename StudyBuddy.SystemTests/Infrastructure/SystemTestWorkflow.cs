using Microsoft.Playwright;
using StudyBuddy.SystemTests.Pages;

namespace StudyBuddy.SystemTests.Infrastructure;

public sealed record TestUser(string Name, string Email, string Password);

public sealed record PendingRequestScenario(TestUser Sender, TestUser Receiver);

public sealed class SystemTestWorkflow
{
    private readonly IPage _page;

    public SystemTestWorkflow(IPage page)
    {
        _page = page;
    }

    public TestUser CreateUser(string prefix)
    {
        var stamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        return new TestUser(
            $"{prefix} {stamp}",
            TestData.UniqueEmail(prefix.ToLowerInvariant().Replace(" ", ".")),
            TestData.DefaultPassword);
    }

    public async Task RegisterLoginCompleteQuizAndLogoutAsync(TestUser user, int? quizAnswerSeed = null)
    {
        await new RegisterPage(_page).RegisterAsync(user.Name, user.Email, user.Password);
        await new LoginPage(_page).LoginAsync(user.Email, user.Password);

        await Assertions.Expect(_page.GetByText("Dashboard").First).ToBeVisibleAsync();

        await new QuizPage(_page).CompleteQuizAsync(quizAnswerSeed);
        await LogoutAsync();
    }

    public async Task LoginAsync(TestUser user)
    {
        await new LoginPage(_page).LoginAsync(user.Email, user.Password);
        await Assertions.Expect(_page.GetByText("Dashboard").First).ToBeVisibleAsync();
    }

    public async Task<PendingRequestScenario> CreatePendingMatchRequestAsync()
    {
        var sender = CreateUser("Playwright Sender");
        var receiver = CreateUser("Playwright Receiver");
        var quizAnswerSeed = Random.Shared.Next();

        await RegisterLoginCompleteQuizAndLogoutAsync(sender, quizAnswerSeed);
        await RegisterLoginCompleteQuizAndLogoutAsync(receiver, quizAnswerSeed);

        await LoginAsync(sender);

        var matchPage = new MatchPage(_page);
        await matchPage.OpenAsync();

        var sent = await matchPage.SendMatchRequestToUserIfAvailableAsync(receiver.Name);
        Assert.That(sent, Is.True, $"No available Send Request button was found for {receiver.Name}.");

        await LogoutAsync();

        return new PendingRequestScenario(sender, receiver);
    }

    public async Task LogoutAsync()
    {
        await _page.GotoAsync("/Auth/Logout");
    }
}
