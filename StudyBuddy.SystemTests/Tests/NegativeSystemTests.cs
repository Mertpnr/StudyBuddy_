using Microsoft.Playwright;
using StudyBuddy.SystemTests.Infrastructure;
using StudyBuddy.SystemTests.Pages;

namespace StudyBuddy.SystemTests.Tests;

public class NegativeSystemTests : SystemTestBase
{
    [Test]
    public async Task Password_And_Confirm_Password_Mismatch_Should_Show_Validation_Error()
    {
        await new RegisterPage(Page).RegisterAsync(
            "Mismatch User",
            TestData.UniqueEmail("mismatch"),
            TestData.DefaultPassword,
            confirmPassword: "Different123!");

        await Expect(Page.Locator("body")).ToContainTextAsync("do not match");
    }

    [Test]
    public async Task Weak_Password_Should_Show_Validation_Error()
    {
        await new RegisterPage(Page).RegisterAsync(
            "Weak Password User",
            TestData.UniqueEmail("weak.password"),
            "weak",
            confirmPassword: "weak");

        await Expect(Page.Locator("body")).ToContainTextAsync("Password must contain at least one uppercase letter and one special character.");
    }

    [Test]
    public async Task Register_With_Duplicate_Email_Should_Fail()
    {
        var email = TestData.UniqueEmail("duplicate.email");

        await new RegisterPage(Page).RegisterAsync(
            "First Duplicate User",
            email,
            TestData.DefaultPassword);

        await Expect(Page.GetByText("Registration successful")).ToBeVisibleAsync();

        await new RegisterPage(Page).RegisterAsync(
            "Second Duplicate User",
            email,
            TestData.DefaultPassword);

        await Expect(Page.Locator("body")).ToContainTextAsync("Registration failed. Email may already be registered.");
    }

    [Test]
    public async Task Invalid_Login_Should_Show_Error()
    {
        await new LoginPage(Page).LoginAsync(
            TestData.UniqueEmail("missing.user"),
            "Wrong123!");

        await Expect(Page.GetByText("Invalid email or password.")).ToBeVisibleAsync();
    }

    [Test]
    public async Task Login_With_Empty_Form_Should_Show_Validation_Errors()
    {
        await Page.GotoAsync("/Auth/Login");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

        await Expect(Page.Locator("body")).ToContainTextAsync("The Email field is required.");
        await Expect(Page.Locator("body")).ToContainTextAsync("The Password field is required.");
    }

    [Test]
    public async Task Empty_Register_Form_Should_Show_Validation_Errors()
    {
        await Page.GotoAsync("/Auth/Register");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();

        await Expect(Page.Locator("body")).ToContainTextAsync("The Full Name field is required.");
        await Expect(Page.Locator("body")).ToContainTextAsync("The Email field is required.");
        await Expect(Page.Locator("body")).ToContainTextAsync("The Password field is required.");
    }

    [Test]
    public async Task User_Without_Login_Should_Be_Redirected_From_Dashboard_To_Login()
    {
        await Page.GotoAsync("/Dashboard");

        await Expect(Page.GetByText("Login to StudyBuddy")).ToBeVisibleAsync();
    }

    [Test]
    public async Task User_Should_Not_Access_MatchRequest_Page_Without_Login()
    {
        await Page.GotoAsync("/MatchRequest");

        await Expect(Page.GetByText("Login to StudyBuddy")).ToBeVisibleAsync();
    }

    [Test]
    public async Task User_Should_Not_Access_Quiz_Page_Without_Login()
    {
        await Page.GotoAsync("/Quiz");

        await Expect(Page.GetByText("Login to StudyBuddy")).ToBeVisibleAsync();
    }

    [Test]
    public async Task User_Should_Not_Access_Match_Page_Without_Login()
    {
        await Page.GotoAsync("/Match");

        await Expect(Page.GetByText("Login to StudyBuddy")).ToBeVisibleAsync();
    }

    [Test]
    public async Task Quiz_Save_Without_Answers_Should_Show_Error_Or_Stay_On_Quiz_Page()
    {
        var workflow = new SystemTestWorkflow(Page);
        var user = workflow.CreateUser("No Quiz Answers");

        await new RegisterPage(Page).RegisterAsync(user.Name, user.Email, user.Password);
        await workflow.LoginAsync(user);

        await new QuizPage(Page).SubmitWithoutAnswersAsync();

        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Save Answers" })).ToBeVisibleAsync();
        await Expect(Page.GetByText("Your study rhythm")).ToBeVisibleAsync();
    }

    [Test]
    public async Task Chat_Room_With_Invalid_MatchRequestId_Should_Redirect_To_MatchRequest()
    {
        var workflow = new SystemTestWorkflow(Page);
        var user = workflow.CreateUser("Invalid Chat Room");

        await workflow.RegisterLoginCompleteQuizAndLogoutAsync(user);
        await workflow.LoginAsync(user);

        await Page.GotoAsync("/Chat/Room?matchRequestId=0");

        await Expect(Page.GetByText("Manage study invites")).ToBeVisibleAsync();
        await Expect(Page.GetByText("Chat could not be opened.")).ToBeVisibleAsync();
    }

    [Test]
    public async Task Duplicate_Match_Request_Should_Not_Be_Allowed()
    {
        var workflow = new SystemTestWorkflow(Page);
        var sender = workflow.CreateUser("Duplicate Sender");
        var receiver = workflow.CreateUser("Duplicate Receiver");

        await workflow.RegisterLoginCompleteQuizAndLogoutAsync(sender);
        await workflow.RegisterLoginCompleteQuizAndLogoutAsync(receiver);
        await workflow.LoginAsync(sender);

        var matchPage = new MatchPage(Page);
        await matchPage.OpenAsync();

        Assert.That(
            await matchPage.SendMatchRequestToUserIfAvailableAsync(receiver.Name),
            Is.True,
            $"Could not send the first match request to {receiver.Name}.");

        await matchPage.OpenAsync();

        Assert.That(
            await matchPage.HasSendRequestForUserAsync(receiver.Name),
            Is.False,
            "The same receiver should not keep an available Send Request button after a request was sent.");
    }

    [Test]
    public async Task Rejected_Match_Should_Not_Expose_Open_Chat()
    {
        var workflow = new SystemTestWorkflow(Page);
        var scenario = await workflow.CreatePendingMatchRequestAsync();

        await workflow.LoginAsync(scenario.Receiver);

        var requestPage = new MatchRequestPage(Page);
        await requestPage.OpenAsync();

        Assert.That(
            await requestPage.RejectFirstRequestIfAvailableAsync(),
            Is.True,
            "No pending incoming match request exists.");

        await requestPage.OpenAsync();

        Assert.That(
            await requestPage.OpenFirstChatIfAvailableAsync(),
            Is.False,
            "A rejected match request should not expose an Open Chat link.");
    }

    [Test]
    public async Task Cancelled_Match_Should_Not_Expose_Open_Chat()
    {
        var workflow = new SystemTestWorkflow(Page);
        var scenario = await workflow.CreatePendingMatchRequestAsync();

        await workflow.LoginAsync(scenario.Sender);

        var requestPage = new MatchRequestPage(Page);
        await requestPage.OpenAsync();

        Assert.That(
            await requestPage.CancelFirstRequestIfAvailableAsync(),
            Is.True,
            "No pending sent match request exists.");

        await requestPage.OpenAsync();

        Assert.That(
            await requestPage.OpenFirstChatIfAvailableAsync(),
            Is.False,
            "A cancelled match request should not expose an Open Chat link.");
    }

    [Test]
    public async Task Pending_Match_Should_Not_Open_Chat()
    {
        var workflow = new SystemTestWorkflow(Page);
        var scenario = await workflow.CreatePendingMatchRequestAsync();

        await workflow.LoginAsync(scenario.Sender);

        var requestPage = new MatchRequestPage(Page);
        await requestPage.OpenAsync();

        Assert.That(
            await requestPage.OpenFirstChatIfAvailableAsync(),
            Is.False,
            "A pending match request should not expose an Open Chat link.");
    }

    [Test]
    public async Task Empty_Chat_Message_Should_Not_Be_Sent()
    {
        var workflow = new SystemTestWorkflow(Page);
        var scenario = await workflow.CreatePendingMatchRequestAsync();

        await workflow.LoginAsync(scenario.Receiver);

        var requestPage = new MatchRequestPage(Page);
        await requestPage.OpenAsync();

        Assert.That(
            await requestPage.AcceptFirstRequestIfAvailableAsync(),
            Is.True,
            "No pending incoming match request exists.");

        await Expect(Page.GetByText("Private Chat")).ToBeVisibleAsync();

        await new ChatPage(Page).SendMessageAsync(" ");

        await Expect(Page.GetByText("Message cannot be empty.")).ToBeVisibleAsync();
    }

    [Test]
    public async Task Profile_Update_With_Empty_Required_Fields_Should_Fail()
    {
        var workflow = new SystemTestWorkflow(Page);
        var user = workflow.CreateUser("Invalid Profile");

        await workflow.RegisterLoginCompleteQuizAndLogoutAsync(user);
        await workflow.LoginAsync(user);

        await new ProfilePage(Page).SubmitEmptyRequiredFieldsAsync();

        await Expect(Page.GetByText("Make your StudyBuddy profile easier to trust.")).ToBeVisibleAsync();
        await Expect(Page.Locator("body")).ToContainTextAsync("The Full Name field is required.");
        await Expect(Page.Locator("body")).ToContainTextAsync("The Email field is required.");
    }
}
