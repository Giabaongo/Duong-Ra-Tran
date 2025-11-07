public static class StoryPayload
{
    public static string TargetScene { get; private set; }
    public static string Title { get; private set; }
    public static string Description { get; private set; }

    public static void Set(string targetScene, string title, string description)
    {
        TargetScene = targetScene;
        Title = title;
        Description = description;
    }

    public static string ConsumeTargetScene(string fallbackScene)
    {
        string result = string.IsNullOrEmpty(TargetScene) ? fallbackScene : TargetScene;
        Clear();
        return result;
    }

    public static void Clear()
    {
        TargetScene = null;
        Title = null;
        Description = null;
    }
}
