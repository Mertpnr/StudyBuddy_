using Microsoft.Playwright;
using StudyBuddy.SystemTests.Infrastructure;
using StudyBuddy.SystemTests.Pages;

namespace StudyBuddy.SystemTests.Tests;

public class MatchRequestSystemTests : SystemTestBase
{
    [Test]
    public async Task User_Should_View_Match_Candidates_And_Send_Match_Request()
    {
        var workflow = new SystemTestWorkflow(Page);
        var sender = workflow.CreateUser("Playwright Sender");
        var receiver = workflow.CreateUser("Playwright Receiver");
        var quizAnswerSeed = Random.Shared.Next();

        await workflow.RegisterLoginCompleteQuizAndLogoutAsync(sender, quizAnswerSeed);
        await workflow.RegisterLoginCompleteQuizAndLogoutAsync(receiver, quizAnswerSeed);

        await workflow.LoginAsync(sender);

        var matchPage = new MatchPage(Page);
        await matchPage.OpenAsync();

        await Expect(Page.GetByText("Find a study buddy")).ToBeVisibleAsync();

        var sent = await matchPage.SendMatchRequestToUserIfAvailableAsync(receiver.Name);

        Assert.That(sent, Is.True, $"No candidate with an available Send Request button exists for {receiver.Name}.");

        await Expect(Page.Locator("body")).ToBeVisibleAsync();
    }

    [Test]
    public async Task User_Should_Accept_Incoming_Match_Request_When_Available()
    {
        var workflow = new SystemTestWorkflow(Page);
        var scenario = await workflow.CreatePendingMatchRequestAsync();

        await workflow.LoginAsync(scenario.Receiver);

        var requestPage = new MatchRequestPage(Page);
        await requestPage.OpenAsync();

        var accepted = await requestPage.AcceptFirstRequestIfAvailableAsync();

        Assert.That(accepted, Is.True, "No pending incoming match request exists.");

        await Expect(Page.GetByText("Private Chat")).ToBeVisibleAsync();
    }

    [Test]
    public async Task User_Should_Reject_Incoming_Match_Request_When_Available()
    {
        var workflow = new SystemTestWorkflow(Page);
        var scenario = await workflow.CreatePendingMatchRequestAsync();

        await workflow.LoginAsync(scenario.Receiver);

        var requestPage = new MatchRequestPage(Page);
        await requestPage.OpenAsync();

        var rejected = await requestPage.RejectFirstRequestIfAvailableAsync();

        Assert.That(rejected, Is.True, "No pending incoming match request exists.");

        await Expect(Page.GetByText("Manage study invites")).ToBeVisibleAsync();
    }

    [Test]
    public async Task User_Should_Cancel_Sent_Match_Request_When_Available()
    {
        var workflow = new SystemTestWorkflow(Page);
        var scenario = await workflow.CreatePendingMatchRequestAsync();

        await workflow.LoginAsync(scenario.Sender);

        var requestPage = new MatchRequestPage(Page);
        await requestPage.OpenAsync();

        var cancelled = await requestPage.CancelFirstRequestIfAvailableAsync();

        Assert.That(cancelled, Is.True, "No pending sent match request exists.");

        await Expect(Page.GetByText("Manage study invites")).ToBeVisibleAsync();
    }
}
