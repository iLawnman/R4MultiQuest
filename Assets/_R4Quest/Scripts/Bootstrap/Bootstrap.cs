using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private List<GameObject> applicationPrefab = new List<GameObject>();
    [Inject] private ConfigDataContainer container;
    [Inject] private AudioService _audioService;

    void Start()
    {
        DontDestroyOnLoad(this);
        
        _audioService.PlayLoop("Waiting.mp3");

        container.ApplicationData.Prefabs?.AddRange(applicationPrefab);
        applicationPrefab.Clear();
    }
}