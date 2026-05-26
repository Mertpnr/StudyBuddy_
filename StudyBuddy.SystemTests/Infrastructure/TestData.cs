namespace StudyBuddy.SystemTests.Infrastructure;

public static class TestData
{
    public const string DefaultPassword = "Test123!";

    public static string UniqueEmail(string prefix)
    {
        return $"{prefix}_{Guid.NewGuid():N}@test.com";
    }
}
