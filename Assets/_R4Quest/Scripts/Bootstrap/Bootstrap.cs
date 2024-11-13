using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private List<GameObject> applicationPrefab = new List<GameObject>();
    [Inject] private ConfigDataContainer container;

    void Start()
    {
        DontDestroyOnLoad(this);
        
        container.ApplicationData.Prefabs?.AddRange(applicationPrefab);
        applicationPrefab.Clear();
    }
}