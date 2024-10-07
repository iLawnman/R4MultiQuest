using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ApplicationSettings _applicationSettings;
    private ResourcesService _resourcesService;

    async void Start()
    {
        DontDestroyOnLoad(this);
        
        // bind all services
        _resourcesService = new ResourcesService();
    }

    public async void StartApplicationFromSettings(ApplicationSettings settings)
    {
        _applicationSettings = settings;
        await _resourcesService.LoadApplicationDataFromSettings(settings);
        
        LoadGameScene();
    }

    void LoadGameScene()
    {
        BootstrapActions.OnShowInfo?.Invoke("Loading GameScene");
        SceneManager.LoadSceneAsync(1);
    }

    public ApplicationSettings GetSettings()
    {
        return _applicationSettings;
    }
}