using Microsoft.Playwright;
using StudyBuddy.SystemTests.Infrastructure;
using StudyBuddy.SystemTests.Pages;

namespace StudyBuddy.SystemTests.Tests;

public class ChatSystemTests : SystemTestBase
{
    [Test]
    public async Task User_Should_Open_Chat_After_Accepted_Match_And_Send_Message()
    {
        var workflow = new SystemTestWorkflow(Page);
        var scenario = await workflow.CreatePendingMatchRequestAsync();

        await workflow.LoginAsync(scenario.Receiver);

        var requestPage = new MatchRequestPage(Page);
        await requestPage.OpenAsync();

        var accepted = await requestPage.AcceptFirstRequestIfAvailableAsync();
        Assert.That(accepted, Is.True, "No pending incoming match request exists.");


        await Expect(Page.GetByText("Private Chat")).ToBeVisibleAsync();

        var message = $"Playwright chat message {DateTime.Now:HHmmss}";
        await new ChatPage(Page).SendMessageAsync(message);

        await Expect(Page.GetByText(message)).ToBeVisibleAsync();
    }

    [Test]
    public async Task Matched_Users_Should_See_Each_Others_Chat_Messages()
    {
        var message = $"Two user message {DateTime.Now:HHmmss}";
        var workflow = new SystemTestWorkflow(Page);
        var scenario = await workflow.CreatePendingMatchRequestAsync();

        await workflow.LoginAsync(scenario.Receiver);

        var receiverRequestPage = new MatchRequestPage(Page);
        await receiverRequestPage.OpenAsync();

        Assert.That(await receiverRequestPage.AcceptFirstRequestIfAvailableAsync(), Is.True, "No pending incoming match request exists.");

        await new ChatPage(Page).SendMessageAsync(message);
        await Expect(Page.GetByText(message)).ToBeVisibleAsync();
        await workflow.LogoutAsync();

        await using var secondContext = await Browser.NewContextAsync(new()
        {
            IgnoreHTTPSErrors = true
        });

        var secondPage = await secondContext.NewPageAsync();

        await secondPage.GotoAsync($"{WebUrl}/Auth/Login");
        await secondPage.Locator("input[name='Email']").FillAsync(scenario.Sender.Email);
        await secondPage.Locator("input[name='Password']").FillAsync(scenario.Sender.Password);
        await secondPage.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

        await secondPage.GotoAsync($"{WebUrl}/MatchRequest");

        var secondOpenChat = secondPage
            .Locator(".sb-request-card")
            .Filter(new() { HasText = scenario.Receiver.Name })
            .GetByRole(AriaRole.Link, new() { Name = "Open Chat" })
            .First;

        Assert.That(await secondOpenChat.IsVisibleAsync(), Is.True, "Second user has no accepted match request with Open Chat.");

        await secondOpenChat.ClickAsync();

        await Expect(secondPage.GetByText(message)).ToBeVisibleAsync();
    }
}
