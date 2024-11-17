using System;
using UnityEngine.XR.ARFoundation;

public class UIActions
{
    public static Action CallQuestStart;
    public static Action<string, bool> OnQuestComplete;
    public static Action<QuestData, ARTrackedImage> OnQuestStart;
}