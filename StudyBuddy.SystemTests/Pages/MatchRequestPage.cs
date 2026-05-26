using Microsoft.Playwright;

namespace StudyBuddy.SystemTests.Pages;

public class MatchRequestPage
{
    private readonly IPage _page;

    public MatchRequestPage(IPage page)
    {
        _page = page;
    }

    public async Task OpenAsync()
    {
        await _page.GotoAsync("/MatchRequest");
    }

    public async Task<bool> AcceptFirstRequestIfAvailableAsync()
    {
        var button = _page.GetByRole(AriaRole.Button, new() { Name = "Accept" }).First;

        if (!await button.IsVisibleAsync())
            return false;

        await button.ClickAsync();
        return true;
    }

    public async Task<bool> RejectFirstRequestIfAvailableAsync()
    {
        var button = _page.GetByRole(AriaRole.Button, new() { Name = "Decline" }).First;

        if (!await button.IsVisibleAsync())
            return false;

        await button.ClickAsync();
        return true;
    }

    public async Task<bool> CancelFirstRequestIfAvailableAsync()
    {
        var button = _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).First;

        if (!await button.IsVisibleAsync())
            return false;

        await button.ClickAsync();
        return true;
    }

    public async Task<bool> OpenFirstChatIfAvailableAsync()
    {
        var link = _page.GetByRole(AriaRole.Link, new() { Name = "Open Chat" }).First;

        if (!await link.IsVisibleAsync())
            return false;

        await link.ClickAsync();
        return true;
    }

    public async Task<bool> OpenChatWithUserIfAvailableAsync(string userName)
    {
        var requestCard = _page
            .Locator(".sb-request-card")
            .Filter(new() { HasText = userName })
            .First;

        if (!await requestCard.IsVisibleAsync())
            return false;

        var link = requestCard.GetByRole(AriaRole.Link, new() { Name = "Open Chat" });

        if (!await link.IsVisibleAsync())
            return false;

        await link.ClickAsync();
        return true;
    }
}
