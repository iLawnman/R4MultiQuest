using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

public class GoogleSheetDataLoadingService
{
    public async UniTask Loading(ApplicationSettings applicationSettings, ConfigDataContainer configDataContainer)
    {
        Debug.Log("loading application " + applicationSettings.applicationName);
        bool updated = await ReadGoogleSheets.CheckTableEditedAsync(applicationSettings.GoogleSheet,
            applicationSettings.GoogleSheetQuestTable);
        
        if (!updated)
        {
            Debug.Log("no need update, but for DEBUG continue loading");
            //return;
        }
        
        Debug.Log("loading data");
        BootstrapActions.OnShowInfo?.Invoke("Loading Quests");
        configDataContainer.ApplicationData.Quests = await ReadGoogleSheets.FillDataAsync<QuestData>(
            applicationSettings.GoogleSheet,
            applicationSettings.GoogleSheetQuestTable);
        
        BootstrapActions.OnShowInfo?.Invoke("Loading Answers");
        configDataContainer.ApplicationData.Answers = await ReadGoogleSheets.FillDataAsync<AnswersData>(
            applicationSettings.GoogleSheet,
            applicationSettings.GoogleSheetAnswersTable);
        
        BootstrapActions.OnShowInfo?.Invoke("Loading Resources");
        configDataContainer.ApplicationData.Resources = await ReadGoogleSheets.FillDataAsync<ResourcesData>(
            applicationSettings.GoogleSheet,
            applicationSettings.GoogleSheetResourcesTable);
        
         BootstrapActions.OnShowInfo?.Invoke("Loading Splash Screen");
         configDataContainer.ApplicationData.SplashScreens = await ReadGoogleSheets.FillDataAsync<SplashScreenData>(
             applicationSettings.GoogleSheet,
             applicationSettings.GoogleSheetSplashTable);

         BootstrapActions.OnShowInfo?.Invoke("Loading Intro Screens");
         configDataContainer.ApplicationData.IntroScreens = await ReadGoogleSheets.FillDataAsync<IntroScreenData>(
             applicationSettings.GoogleSheet,
             applicationSettings.GoogleSheetIntroTable);
         
         BootstrapActions.OnShowInfo?.Invoke("Loading Outro Screen");
         configDataContainer.ApplicationData.OutroScreens = await ReadGoogleSheets.FillDataAsync<IntroScreenData>(
             applicationSettings.GoogleSheet,
             applicationSettings.GoogleSheetOutroTable);
         
         BootstrapActions.OnShowInfo?.Invoke("Loading BasePrefab Skin");
         configDataContainer.ApplicationData.BasePrefabSkin = await ReadGoogleSheets.FillDataAsync<BasePrefabSkinData>(
             applicationSettings.GoogleSheet,
             applicationSettings.GoogleSheetBasePrefab);
    }
    
    private async UniTask CheckQuestSheetTable( string id = "1r126nBWT0kMIyZIJONteKrGTDidYMU867MbnnAR19-0", string gridId = "0")
    {
        string apiKey = "AIzaSyACKRzVQ-koaSkqmdRFFEjWDkt7GbHT0IM"; // Ваш API ключ Google
        //string modifiedTimeurl = $"https://www.googleapis.com/drive/v3/files/{fileId}?fields=modifiedTime&key={apiKey}";
        string url = $@"https://docs.google.com/spreadsheet/ccc?key={id}&usp=sharing&output=csv&id=KEY&gid={gridId}";


        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await UniTask.Yield(); // Ожидание завершения запроса
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                string sheetData = request.downloadHandler.text;
                Debug.Log("data " + sheetData); // Обработка полученных данных
                
                string[] lines = sheetData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                for (var index = 1; index < lines.Length; index++)
                {
                    var line = lines[index];
                    string[] parts = line.Split(',');
                    //BootstrapActions.AddApplicationToList.Invoke(line);
                }
            }
            else
            {
                Debug.LogError("Ошибка при загрузке данных с Google Sheet: " + request.error);
            }
        }
    }
}