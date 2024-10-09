using DataSakura.Runtime.Utilities;
using DataSakura.Runtime.Utilities.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

public class BootstrapFlow : IStartable
{
    private readonly LoadingService _loadingService;
    private readonly DataContainer _dataContainer;

    public BootstrapFlow(LoadingService loadingService, DataContainer dataContainer)
    {
        _loadingService = loadingService;
        _dataContainer = dataContainer;
    }

    public async void Start()
    {
        Log.Default.D("BootstrapFlow.Start()");
        await _loadingService.BeginLoading(_dataContainer);
        Debug.Log("loaded data " + _dataContainer.ApplicationData.Quests.Count + " / " + _dataContainer.ApplicationData.Answers.Count);
    }
}