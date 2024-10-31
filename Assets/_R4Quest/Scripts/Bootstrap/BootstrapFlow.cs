using System.Collections;
using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using DataSakura.Runtime.Utilities.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class BootstrapFlow : IStartable
{
    private readonly GoogleSheetDataLoadingService _loadingService;
    private ConfigDataContainer _configDataContainer;
    private readonly IObjectResolver _container;
    private FileSyncService _fileSyncService;

    public BootstrapFlow(IObjectResolver container,
        GoogleSheetDataLoadingService loadingService,
        FileSyncService fileSyncService)
    {
        _container = container;
        _loadingService = loadingService;
        _fileSyncService = fileSyncService;
    }

    public async void Start()
    {
        //return;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("no internet");
            await LoadDefaultDataSet();
        }
        else
            BootstrapActions.OnSelectApplication += StartAsync;
    }
    
    public async void StartAsync(ApplicationSettings applicationSettings)
    {
             BindInstanceToRootScope(applicationSettings);
             
             //await _fileSyncService.Initilize(applicationSettings);
             //
             // Debug.Log("start loading data");
             // await _loadingService.Loading(applicationSettings, _configDataContainer);
             // Debug.Log("loaded data " + _configDataContainer.ApplicationData.Quests.Count
             //                          + " / " + _configDataContainer.ApplicationData.Answers.Count
             //                          + " / " + _configDataContainer.ApplicationData.Resources.Count);
             //
             // SceneManager.LoadSceneAsync(1);
    }

    private async UniTask LoadDefaultDataSet()
    {
        Debug.Log("need load startUp data");
        await UniTask.CompletedTask;
    }

    private void BindInstanceToRootScope<T>(T Instance)
    {
        Debug.Log("try rebind selected appsetting " + Instance);
        _container.Inject(Instance);
        _configDataContainer = _container.Resolve<ConfigDataContainer>();
    }
}