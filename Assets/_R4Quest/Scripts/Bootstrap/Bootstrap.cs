using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

public class Bootstrap : MonoBehaviour
{
    [Header("Base Shared Prefabs")]
    [SerializeField] private List<GameObject> basePrefabs = new List<GameObject>();
    
    //TO-DO : move to application data
    [Header("Application Prefabs")]
    [SerializeField] private List<GameObject> applicationPrefab = new List<GameObject>();
    [SerializeField] private LazyLoadReference<GameObject> applicationPrefabLazy = new LazyLoadReference<GameObject>();
    [Inject] private ConfigDataContainer container;
    [Inject] private AudioService _audioService;
    [SerializeField] private AudioClip waitingClip;

    void Start()
    {
        Application.runInBackground = true;
        _audioService.PlayLoop2D(waitingClip);
        
        DontDestroyOnLoad(this);
        
        container.ApplicationData.Prefabs?.AddRange(basePrefabs);
        container.ApplicationData.Prefabs?.AddRange(applicationPrefab);
        basePrefabs.Clear();
        applicationPrefab.Clear();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("application focus " + hasFocus);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("application pause " + pauseStatus);
    }
}