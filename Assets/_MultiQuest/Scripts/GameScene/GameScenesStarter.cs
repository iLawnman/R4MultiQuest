using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

public class GameScenesStarter : MonoBehaviour
{
    void Start()
    {
        var settings = FindObjectOfType<Bootstrap>().GetSettings();
        Debug.Log("start " + settings.AddressableKey);
        
        Addressables.LoadResourceLocationsAsync(settings.AddressableKey, 
                typeof(UnityEngine.ResourceManagement.ResourceProviders.SceneInstance))
            .Completed += OnLocationsLoaded;
    }

    void OnLocationsLoaded(AsyncOperationHandle<IList<IResourceLocation>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var locations = handle.Result;
            if (locations.Count > 0)
            {
                for (int i = 0; i < locations.Count; i++)
                {
                    BootstrapActions.OnShowInfo?.Invoke("Loading Scene " + locations[i].PrimaryKey);
                    var loadMode = i == 0 ? LoadSceneMode.Single :  LoadSceneMode.Additive;
                    Addressables.LoadSceneAsync(locations[i], loadMode).Completed += OnSceneLoaded;
                }
            }
            else
            {
                Debug.LogError("No scenes with label found.");
            }
        }
        else
        {
            Debug.LogError($"Failed to find {handle.DebugName} with label : {handle.OperationException}");
        }
    }

    void OnSceneLoaded(AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            BootstrapActions.OnShowInfo?.Invoke("Loaded " + handle.DebugName);
            Debug.Log(handle.DebugName + " loaded successfully.");
        }
        else
        {
            Debug.LogError($"Failed to load {handle.DebugName} with {handle.OperationException}");
        }
    }
}