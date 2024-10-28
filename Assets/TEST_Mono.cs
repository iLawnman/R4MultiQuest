using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using VContainer;
using Object = UnityEngine.Object;

public class TEST_Mono : MonoBehaviour
{
    [SerializeField] private ApplicationSettings settins;
    private ApplicationRemoteSettings remoteSettins;
    [SerializeField] private AssetReference _assetReference;
    [Inject] private GoogleSheetDataLoadingService _loadingService;
    private GoogleSheetLoadUnit<QuestData> data;
    private ConfigDataContainer _configDataContainer = new ConfigDataContainer();
    [SerializeField] private KeyCode keyCode;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyUp(keyCode))
        {
            GetRemoteSprite();
        }
    }

    private void GetRemoteSprite()
    {
        //get Gtable
        //get data resources from table
        //fill repository
    }
}