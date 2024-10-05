using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    private AsyncOperationHandle preloadOp;

    async void Start()
    {
        var settings = FindObjectOfType<Bootstrap>().GetSettings();
        Debug.Log("start " + settings.AddressableKey);
        
        List<string> keys = new List<string> { "ARScene", "Pirates"};
        
        Addressables.LoadResourceLocationsAsync(settings.AddressableKey, typeof(UnityEngine.ResourceManagement.ResourceProviders.SceneInstance))
            .Completed += OnLocationsLoaded;
        
        // var sceneHandler = Addressables.LoadSceneAsync("ARScene");
        // while (!sceneHandler.IsDone)
        // {
        //     BootstrapActions.OnShowInfo?.Invoke("Loading Scene\n" + (sceneHandler.PercentComplete * 100).ToString("F0"));
        //     await UniTask.Yield();
        // }
    }

    void OnLocationsLoaded(AsyncOperationHandle<IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var locations = handle.Result;

            // Проверяем, есть ли доступные сцены с меткой "Pirates"
            if (locations.Count > 0)
            {
                // Загружаем первую найденную сцену с меткой "Pirates"
                Addressables.LoadSceneAsync(locations[0], LoadSceneMode.Single).Completed += OnSceneLoaded;
            }
            else
            {
                Debug.LogError("No scenes with label 'Pirates' found.");
            }
        }
        else
        {
            Debug.LogError($"Failed to find resources with label 'Pirates': {handle.OperationException}");
        }
    }

    // Callback по завершении загрузки сцены
    void OnSceneLoaded(AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Scene loaded successfully.");
        }
        else
        {
            Debug.LogError($"Failed to load scene: {handle.OperationException}");
        }
    }
}