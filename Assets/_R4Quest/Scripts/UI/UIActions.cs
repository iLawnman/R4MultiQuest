using System;
using UnityEngine.XR.ARFoundation;

public class UIActions
{
    public static Action OnQuestPanelCurrent;
    public static Action CallQuestStart;
    public static Action<string, bool> OnQuestComplete;
    public static Action<QuestData, string> OnQuestStart;
    public static Action<bool, int> OnShowScenFX;
    public static Action<string, string> OnQuestPanel;
}