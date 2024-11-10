using UnityEngine;
using VContainer.Unity;

public class UIScenesStarter : IStartable
{
    private ApplicationSettings _applicationSettings;
    private readonly ConfigDataContainer _dataContainer;

    public UIScenesStarter(ConfigDataContainer dataContainer)
    {
        _dataContainer = dataContainer;
    }

    public void Start()
    {
        _applicationSettings = _dataContainer.ApplicationSettings;
        Debug.Log("start uiscene with addressable setting " + _applicationSettings.AddressableKey);
    }
}