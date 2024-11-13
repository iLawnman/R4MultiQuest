using System;

public partial class GameActions
{
    public static Action OnLoadFinish;
    public static Action<string, string> OnShowStartQuestPanel;
    public static Action<string, bool> OnQuestComplete;
    public static Action<string> OnQuestStart;
    public static Action CallQuestStart;
}