using System.Collections;
using System.Runtime.InteropServices;
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
    private readonly IObjectResolver _resolver;
    private FileSyncService _fileSyncService;

    public BootstrapFlow(IObjectResolver resolver,
        GoogleSheetDataLoadingService loadingService,
        FileSyncService fileSyncService)
    {
        _resolver = resolver;
        _loadingService = loadingService;
        _fileSyncService = fileSyncService;
    }

    public async void Start()
    {
        //return;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("no internet");
            BootstrapActions.OnShowInfo?.Invoke("NO INTERNET CONNECTION\nAPPLICATION WILL CLOSE");
            await UniTask.WaitUntil(() => Input.touchCount > 0);
            //await LoadDefaultDataSet();
        }
        else
            BootstrapActions.OnSelectApplication += StartAsync;
    }
    
    private async void StartAsync(ApplicationSettings applicationSettings)
    {
        BootstrapActions.OnSelectApplication -= StartAsync;
        
        await BindInstanceToRootScope(applicationSettings);

        await LoadSelectedAplicationData(applicationSettings);
    }

    private async Task LoadSelectedAplicationData(ApplicationSettings applicationSettings)
    {
        Debug.Log("start loading data");

        await _loadingService.Loading(applicationSettings, _configDataContainer);
        BootstrapActions.OnShowInfo?.Invoke("ALL SHEETS LOADED");

        Debug.Log("loaded data : QST/" + _configDataContainer.ApplicationData.Quests.Count
                                 + " ANS/" + _configDataContainer.ApplicationData.Answers.Count
                                 + " RES/" + _configDataContainer.ApplicationData.Resources.Count
                                 + " LOC/" + _configDataContainer.ApplicationData.Location
                                 + " SPL/" + _configDataContainer.ApplicationData.SplashScreens.Count
                                 + " INT/ " + _configDataContainer.ApplicationData.IntroScreens.Count
                                 + " OUT/ " + _configDataContainer.ApplicationData.OutroScreens.Count
                                 + " SCP/ " + _configDataContainer.ApplicationData.Scripts
                                 + " SKN/ " + _configDataContainer.ApplicationData.BasePrefabSkin.Count);
        
        await _fileSyncService.Initilize(applicationSettings);
        
        GameActions.OnLoadFinish?.Invoke();
        BootstrapActions.OnShowInfo.Invoke(string.Empty);
        await UniTask.Delay(1000);
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
        _resolver.Inject(Instance);
        _configDataContainer = _resolver.Resolve<ConfigDataContainer>();
        _configDataContainer.ApplicationSettings = Instance as ApplicationSettings;

        await UniTask.CompletedTask;
    }
}