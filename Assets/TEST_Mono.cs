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
        await LoadGoogleTable();
        //CheckTableEditedTime();
    }

    private async Task LoadGoogleTable()
    {
        _loadingService = new LoadingService();
        GoogleSheetLoadUnit<QuestData> a = new GoogleSheetLoadUnit<QuestData>(
            settins.GoogleSheet, 
            settins.GoogleSheetQuestTable);
        await _loadingService.BeginLoading(a);
        
        Debug.Log("a " + a.Data);
    }

    private void CheckTableEditedTime()
    {
        string fileId = "1-P_YElXhGf6H2NfwVIPiq8xQV55LbWsCmF_D4cc8EFw"; // Идентификатор файла Google Sheets
        string apiKey = "AIzaSyACKRzVQ-koaSkqmdRFFEjWDkt7GbHT0IM"; // Ваш API ключ Google
        string url = $"https://www.googleapis.com/drive/v3/files/{fileId}?fields=modifiedTime&key={apiKey}";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";

        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd();
                Debug.Log("Response: " + result);
            }
        }
        catch (WebException ex)
        {
            Debug.Log("Error: " + ex.Message);
        }
    }
}