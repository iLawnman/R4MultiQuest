using System;
using UnityEngine.XR.ARFoundation;

public class ARSceneActions
{
    public static Action<string> OnARTrackedImage;
    public static Action OnARSession;
    public static Action<bool> OnReadyForTracking;
}