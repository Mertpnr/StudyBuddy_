using Microsoft.Playwright;

namespace StudyBuddy.SystemTests.Pages;

public class ChatPage
{
    private readonly IPage _page;

    public ChatPage(IPage page)
    {
        _page = page;
    }

    public async Task SendMessageAsync(string message)
    {
        await _page.GetByPlaceholder("Write your message...").FillAsync(message);
        await _page.GetByRole(AriaRole.Button, new() { Name = "Send" }).ClickAsync();
    }
}
