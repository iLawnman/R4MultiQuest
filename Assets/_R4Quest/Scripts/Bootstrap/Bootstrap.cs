using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Bootstrap : MonoBehaviour
{
    private ResourcesService _resourcesService;

    async void Start()
    {
        DontDestroyOnLoad(this);
        
        // bind all services
        _resourcesService = new ResourcesService();
    }

    public async void StartApplicationFromSettings(ApplicationSettings settings)
    {
        await _resourcesService.LoadApplicationDataFromSettings(settings);
        
        LoadGameScene();
    }

    void LoadGameScene()
    {
        BootstrapActions.OnShowInfo?.Invoke("Loading GameScene");
        SceneManager.LoadSceneAsync(1);
    }
}