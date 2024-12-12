using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSceneActions
{
    public static Action<QuestData> OnWaitRecognitionImage;
    public static Action<string, Transform> OnARTrackedImage;
    public static Action OnARSession;
    public static Action<bool> OnReadyForTracking;
}