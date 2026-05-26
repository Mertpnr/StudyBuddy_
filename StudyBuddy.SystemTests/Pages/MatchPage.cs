using Microsoft.Playwright;

namespace StudyBuddy.SystemTests.Pages;

public class MatchPage
{
    private readonly IPage _page;

    public MatchPage(IPage page)
    {
        _page = page;
    }

    public async Task OpenAsync()
    {
        await _page.GotoAsync("/Match");
    }

    public async Task<bool> SendFirstMatchRequestIfAvailableAsync()
    {
        var sendButton = _page.GetByRole(AriaRole.Button, new() { Name = "Send Request" }).First;

        if (!await sendButton.IsVisibleAsync())
            return false;

        await sendButton.ClickAsync();
        return true;
    }

    public async Task<bool> SendMatchRequestToUserIfAvailableAsync(string userName)
    {
        var candidateCard = CandidateCard(userName);

        if (!await candidateCard.IsVisibleAsync())
            return false;

        var sendButton = candidateCard.GetByRole(AriaRole.Button, new() { Name = "Send Request" });

        if (!await sendButton.IsVisibleAsync())
            return false;

        await sendButton.ClickAsync();
        return true;
    }

    public async Task<bool> HasSendRequestForUserAsync(string userName)
    {
        var candidateCard = CandidateCard(userName);

        if (!await candidateCard.IsVisibleAsync())
            return false;

        return await candidateCard
            .GetByRole(AriaRole.Button, new() { Name = "Send Request" })
            .IsVisibleAsync();
    }

    private ILocator CandidateCard(string userName)
    {
        return _page
            .Locator(".sb-candidate-card")
            .Filter(new() { HasText = userName })
            .First;
    }
}
