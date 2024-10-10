using UnityEngine;
using VContainer.Unity;

public class UIScenesStarter : IStartable
{
    private readonly ApplicationSettings _applicationSettings;

    public UIScenesStarter(ApplicationSettings applicationSettings)
    {
        _applicationSettings = applicationSettings;
    }

    public void Start()
    {
        Debug.Log("start uiscene with addressable setting " + _applicationSettings.AddressableKey);
    }
}