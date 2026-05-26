using Microsoft.Playwright;

namespace StudyBuddy.SystemTests.Pages;

public class QuizPage
{
    private readonly IPage _page;

    public QuizPage(IPage page)
    {
        _page = page;
    }

    public async Task CompleteQuizAsync(int? seed = null)
    {
        await _page.GotoAsync("/Quiz");

        var random = seed.HasValue
            ? new Random(seed.Value)
            : Random.Shared;

        var optionGroups = await _page.Locator("input[type='radio']").EvaluateAllAsync<string[]>(
            "nodes => [...new Set(nodes.map(n => n.getAttribute('name')))].filter(Boolean)");

        foreach (var group in optionGroups)
        {
            var options = _page.Locator($"input[type='radio'][name=\"{group}\"]");
            var optionCount = await options.CountAsync();

            if (optionCount == 0)
                continue;

            var selectedIndex = random.Next(optionCount);

            await options.Nth(selectedIndex).CheckAsync(new()
            {
                Force = true
            });
        }

        await _page.GetByRole(AriaRole.Button, new() { Name = "Save Answers" }).ClickAsync();
    }

    public async Task SubmitWithoutAnswersAsync()
    {
        await _page.GotoAsync("/Quiz");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Save Answers" }).ClickAsync();
    }
}
