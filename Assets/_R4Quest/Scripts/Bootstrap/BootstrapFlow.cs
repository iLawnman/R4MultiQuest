using System.Collections;
using System.Threading.Tasks;
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
    
    private async void StartAsync(ApplicationSettings applicationSettings)
    {
        await BindInstanceToRootScope(applicationSettings);

        await LoadSelectedAplicationData(applicationSettings);
    }

    private async Task LoadSelectedAplicationData(ApplicationSettings applicationSettings)
    {
        Debug.Log("start loading data");

        await _loadingService.Loading(applicationSettings, _configDataContainer);
        BootstrapActions.OnShowInfo?.Invoke("ALL SHEETS LOADED");

        Debug.Log("loaded data " + _configDataContainer.ApplicationData.Quests.Count
                                 + " / " + _configDataContainer.ApplicationData.Answers.Count
                                 + " / " + _configDataContainer.ApplicationData.Resources.Count
                                 + " / " + _configDataContainer.ApplicationData.Location
                                 + " / " + _configDataContainer.ApplicationData.SplashScreens.Count
                                 + " / " + _configDataContainer.ApplicationData.IntroScreens.Count
                                 + " / " + _configDataContainer.ApplicationData.OutroScreens.Count);
        await _fileSyncService.Initilize(applicationSettings);
             
        
        await UniTask.Delay(1000);
        BootstrapActions.OnShowInfo?.Invoke(string.Empty);
        
        SceneManager.LoadSceneAsync(1);
    }

    private async UniTask LoadDefaultDataSet()
    {
        Debug.Log("need load startUp data");
        await UniTask.CompletedTask;
    }

    private async UniTask BindInstanceToRootScope(ScriptableObject Instance)
    {
        Debug.Log("try rebind selected appsetting " + Instance);
        _container.Inject(Instance);
        _configDataContainer = _container.Resolve<ConfigDataContainer>();
        _configDataContainer.ApplicationSettings = Instance as ApplicationSettings;
    }
}