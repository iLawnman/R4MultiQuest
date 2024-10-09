using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

    public class DataContainer :  ILoadUnit
    {
        public ApplicationData ApplicationData = new ApplicationData();
        [Inject] private readonly ApplicationSettings _applicationSettings;
        [Inject] private LoadingService _loadingService;

        public async UniTask Load()
        {
            var status = Application.internetReachability;
            
            if (status == NetworkReachability.NotReachable)
                return;

            if (TableEdited())
            {
                BootstrapActions.OnShowInfo?.Invoke("Internet Reachable");

                await Task.Delay(2000);

                await LoadDependencies();

                BootstrapActions.OnShowInfo?.Invoke("Loading Data");

                GoogleSheetLoadUnit<QuestData> quests = new GoogleSheetLoadUnit<QuestData>(
                    _applicationSettings.GoogleSheet,
                    _applicationSettings.GoogleSheetQuestTable);
                
                GoogleSheetLoadUnit<AnswersData> answers = new GoogleSheetLoadUnit<AnswersData>(
                    _applicationSettings.GoogleSheet,
                    _applicationSettings.GoogleSheetAnswersTable);
                

                await _loadingService.BeginLoading(quests);
                await _loadingService.BeginLoading(answers);

                ApplicationData.Quests = quests.Data as List<QuestData>;
                ApplicationData.Answers = answers.Data as List<AnswersData>;
                
                BootstrapActions.OnShowInfo?.Invoke("Loaded Dependencies");

                await Task.Delay(2000);
                
                BootstrapActions.OnShowInfo?.Invoke(string.Empty);

            }
        }

        private async UniTask LoadDependencies()
        {
            await Addressables.InitializeAsync();
            
            var _depHandler = Addressables.DownloadDependenciesAsync(_applicationSettings.AddressableKey);
            
            while (!_depHandler.IsDone)
            {
                BootstrapActions.OnShowInfo?.Invoke("Loading Dependencies\n" + (_depHandler.PercentComplete * 100).ToString("F0"));
                await UniTask.Yield();
            }
        }
        
        private bool TableEdited()
        {
            return true;
            
            string fileId = "1-P_YElXhGf6H2NfwVIPiq8xQV55LbWsCmF_D4cc8EFw"; // Идентификатор файла Google Sheets
            string apiKey = "your_api_key"; // Ваш API ключ Google
            string url = $"https://www.googleapis.com/drive/v3/files/{fileId}?fields=modifiedTime";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine("Response: " + result);
                    if (result != PlayerPrefs.GetString("GoogleSheetEdited"))
                    {
                        PlayerPrefs.SetString("GoogleSheetEdited", result);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false;
        }
    }