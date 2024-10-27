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

    private void Update()
    {
        if (Input.GetKeyUp(keyCode))
        {
            //_loadingService.Loading(_configDataContainer);
            // ReadGoogleSheets.FillData<QuestData>(settins.GoogleSheet,
            //     settins.GoogleSheetQuestTable,
            //     list =>
            //     {
            //         _configDataContainer.Quests = list;
            //         Debug.Log("q " + _configDataContainer.Quests.Count);
            //     });
        }
    }

    [ContextMenu("DO")]
    public void DO()
    {
        //_loadingService = new LoadingService();
        //LoadGoogleTable();
        //CheckTableEditedTime();
    }

    private async UniTask CheckTableEditedTime()
    {
        string fileId = "1-P_YElXhGf6H2NfwVIPiq8xQV55LbWsCmF_D4cc8EFw"; // Идентификатор файла Google Sheets
        string apiKey = "AIzaSyACKRzVQ-koaSkqmdRFFEjWDkt7GbHT0IM"; // Ваш API ключ Google
        string url = $"https://www.googleapis.com/drive/v3/files/{fileId}?fields=modifiedTime&key={apiKey}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield(); // Ожидание завершения запроса
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                string sheetData = request.downloadHandler.text;
                Debug.Log("data " + sheetData);  // Обработка полученных данных
            }
            else
            {
                Debug.LogError("Ошибка при загрузке данных с Google Sheet: " + request.error);
            }
        }
    }
}