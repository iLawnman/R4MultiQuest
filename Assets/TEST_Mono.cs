using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public class TEST_Mono : MonoBehaviour
{
    [SerializeField] private ApplicationSettings settins;
    private ApplicationRemoteSettings remoteSettins;
    [SerializeField] private AssetReference _assetReference;
    private LoadingService _loadingService;
    private GoogleSheetLoadUnit<QuestData> data;


    [ContextMenu("DO")]
    public async void DO()
    {
        _loadingService = new LoadingService();
        GoogleSheetLoadUnit<QuestData> a = new GoogleSheetLoadUnit<QuestData>(settins.GoogleSheet, settins.GoogleSheetQuestTable);
        await _loadingService.BeginLoading(a);
        Debug.Log("a " + a.Data.ToString());
    }
}