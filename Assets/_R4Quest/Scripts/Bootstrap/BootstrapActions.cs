using System;
using UnityEngine;

public class BootstrapActions
{
    public static Action<ApplicationSettings> OnSelectApplication;
    public static Action<string> OnShowInfo;
    public static Action<string> AddApplicationToList;
    public static Action<string> OnSelectApplicationByName;
}

public class AudioActions
{
    public static Action<string, Transform> PlayOneShot3D;
    public static Action<string, Transform> PlayLoop3D;
    public static Action<string> PlayOneShot2D;
    public static Action<string> PlayLoop2D;
    public static Action<string[]> PlaySounds;
}