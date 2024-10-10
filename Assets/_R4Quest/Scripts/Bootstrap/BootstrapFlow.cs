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
    private readonly ConfigDataContainer _configDataContainer;
    private readonly ApplicationSettings _applicationSettings;

    public BootstrapFlow(GoogleSheetDataLoadingService loadingService, 
        ConfigDataContainer configDataContainer,
        ApplicationSettings applicationSettings)
    {
        _loadingService = loadingService;
        _configDataContainer = configDataContainer;
        _applicationSettings = applicationSettings;
    }

    public async void Start()
    {
        //return;
        
         if (Application.internetReachability == NetworkReachability.NotReachable)
         {
             Debug.Log("no internet");
             // load SO data from defoult data repo
             await LoadDefaultDataSet();
             //return;
         }

         else
         {
             Debug.Log("start loading data");
             await _loadingService.Loading(_configDataContainer);
             Debug.Log("loaded data " + _configDataContainer.ApplicationData.Quests.Count
                                      + " / " + _configDataContainer.ApplicationData.Answers.Count
                                      + " / " + _configDataContainer.ApplicationData.Resources.Count);

             BindInstanceToRootScope(_applicationSettings);
         }

         SceneManager.LoadSceneAsync(1);
    }

    private async UniTask LoadDefaultDataSet()
    {
        Debug.Log("need load startUp data");
        await UniTask.CompletedTask;
    }

    private void BindInstanceToRootScope<T>(T Instance)
    {
        Debug.Log("try rebind selected appsetting " + _applicationSettings.AddressableKey);
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterInstance(Instance).As<T>();
        //RootScope.RootContainer.Inject(containerBuilder);
    }
}