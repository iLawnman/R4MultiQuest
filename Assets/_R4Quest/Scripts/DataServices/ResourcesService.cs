using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using VContainer;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ResourcesService
{
    //[Inject] private LoadingService _loadingService;

    private ApplicationSettings currentSetting;
    private List<ApplicationRemoteSettings> currentRemoteSettings;

    private int timeOut => Random.Range(2500, 3500);

    public async Task LoadApplicationDataFromSettings(ApplicationSettings settings)
    {
        currentSetting = settings;
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            await CheckRemoteSettings();

            await LoadDependencies();
        }
        else
        {
            BootstrapActions.OnShowInfo?.Invoke("NO INTERNET CONNECTION");
        }
    }

    private async UniTask LoadDependencies()
    {
        //await _loadingService.BeginLoading( new DependenciesLoadUnit(currentSetting.AddressableKey) );
        
        var _depHandler = Addressables.DownloadDependenciesAsync(currentSetting.AddressableKey);
        
        while (!_depHandler.IsDone)
        {
            BootstrapActions.OnShowInfo?.Invoke("Loading Dependencies\n" + (_depHandler.PercentComplete * 100).ToString("F0"));
            await UniTask.Yield();
        }
        //_depHandler.Release();
        
        //GetAllKeys();
        
        //await LoadResourceByKeyLabel("ARScene", "Pirates");
        //Debug.Log(resources);
        //await LoadResourcesLocators(currentSetting.AddressableKey);
        // Загрузка ассетов с прогрессо
        // var objHandler = Addressables.LoadAssetsAsync<Object>(currentSetting.AddressableKey, o => { });
        //
        //  while (!objHandler.IsDone)
        //  {
        //      BootstrapActions.OnShowInfo?.Invoke("Loading Scene Objects\n" + (objHandler.PercentComplete * 100).ToString("F0"));
        //      await UniTask.Yield();
        //  }
        // Использование загруженных ассетов (например, инстанцирование)
        // foreach (var asset in assets)
        // {
        //     Instantiate(asset);
        // }
    }
    
    async Task LoadResourceByKeyLabel(string resourceKey, string currentLabel)
    {
        List<string> keys = new List<string> { resourceKey, currentLabel };
        Addressables.LoadAssetsAsync<Object>(keys, OnResourceLoaded, Addressables.MergeMode.None);//.Completed += OnAssetsLoaded;
    }

    private void OnAssetsLoaded(AsyncOperationHandle<IList<Object>> obj)
    {
        Debug.Log("asset loaded " + obj.Result.Count);
    }

    private void OnResourceLoaded(object obj)
    {
        Debug.Log("res loaded " + obj);
    }

    void GetAllKeys()
    {
        List<object> allKeys = new List<object>();

        // Проходим через все локаторы, доступные в Addressables
        foreach (var locator in Addressables.ResourceLocators)
        {
            // Добавляем все ключи локатора в общий список
            allKeys.AddRange(locator.Keys);
        }

        // Выводим все ключи в консоль
        foreach (var key in allKeys)
        {
            Debug.Log($"Key: {key}");
        }
    }

    private static async Task LoadResourcesLocators(string key)
    {
        AsyncOperationHandle<IList<IResourceLocation>> handle
            = Addressables.LoadResourceLocationsAsync(
                new string[] {key},
                Addressables.MergeMode.Intersection);

        await handle;
        
        foreach (var resource in handle.Result)
        {
            Debug.Log("res  " + resource);
        }
        
        //handle.Release();
    }

    async Task CheckRemoteSettings()
    {
        BootstrapActions.OnShowInfo?.Invoke("Check Remote Settings");
        // ReadGoogleSheets.FillData<ApplicationRemoteSettings>(currentSetting.GoogleSheet, currentSetting.RemoteSettings, list =>
        // {
        //     currentRemoteSettings = list;
        // });
        await Task.Delay(timeOut);
    }

    async Task CheckResourcesInCache()
    {
        BootstrapActions.OnShowInfo?.Invoke("Check Resources In Cache");
    }

    async Task LoadGameSceneData()
    {
        BootstrapActions.OnShowInfo?.Invoke("Loading GameScene Data");
        await Task.Delay(timeOut);
    }
}