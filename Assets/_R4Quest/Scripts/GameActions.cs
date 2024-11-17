using System;

public static class GameActions
{
    public static Action OnLoadFinish;
    public static Action CallQuestStart;
    public static Action<string> OnQuestStart;
    public static Action<bool, string> SetReadyForTracking;
    public static Action<string, bool> OnQuestComplete;
}