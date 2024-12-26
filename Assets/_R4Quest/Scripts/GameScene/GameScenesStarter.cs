using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class GameScenesStarter : IStartable
{
    private ConfigDataContainer _container;

    public GameScenesStarter(ConfigDataContainer container)
    {
        _container = container;
    }

    public void Start()
    {
        StartAsync().Forget();
    }

    async UniTask StartAsync()
    {
        try
        {
            Debug.Log("start gamescene with addressable setting " + _container.ApplicationSettings.AddressableKey);

            await Addressables.DownloadDependenciesAsync(_container.ApplicationSettings.AddressableKey + "ReferenceImageLibrary", false);
            await Addressables.DownloadDependenciesAsync("ARScene", false);
            await Addressables.DownloadDependenciesAsync("UIScene", false);
            Debug.Log("loadeded dependences ");

            await SceneManager.LoadSceneAsync("ARScene", LoadSceneMode.Single);
            await SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("ARScene"));
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            throw;
        }
    }
}