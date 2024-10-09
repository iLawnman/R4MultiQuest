using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataSakura.Runtime.Utilities;
using UnityEngine;
using VContainer;

    public class ConfigDataContainer :  ILoadUnit
    {
        public ApplicationData ApplicationData = new ApplicationData();
        [Inject] private readonly ApplicationSettings _applicationSettings;
        //[Inject] private LoadingService _loadingService;

        public async UniTask Load()
        {
            var status = Application.internetReachability;
            
            if (status == NetworkReachability.NotReachable)
                return;

            if (TableEdited())
            {
                BootstrapActions.OnShowInfo?.Invoke("Loading Data");

                //await _loadingService.BeginLoading(new DependencesLoadUnit(_applicationSettings.AddressableKey));
                
                GoogleSheetLoadUnit<QuestData> quests = new GoogleSheetLoadUnit<QuestData>(
                    _applicationSettings.GoogleSheet,
                    _applicationSettings.GoogleSheetQuestTable);
                
                GoogleSheetLoadUnit<AnswersData> answers = new GoogleSheetLoadUnit<AnswersData>(
                    _applicationSettings.GoogleSheet,
                    _applicationSettings.GoogleSheetAnswersTable);
                
                //await _loadingService.BeginLoading(answers);

                GoogleSheetLoadUnit<ResourcesData> resources = new GoogleSheetLoadUnit<ResourcesData>(
                    _applicationSettings.GoogleSheet,
                    _applicationSettings.GoogleSheetResourcesTable);
                
                //await _loadingService.BeginLoading(resources);

                ApplicationData.Quests = quests.Data as List<QuestData>;
                ApplicationData.Answers = answers.Data as List<AnswersData>;
                ApplicationData.Resources = resources.Data as List<ResourcesData>;
                
                BootstrapActions.OnShowInfo?.Invoke(string.Empty);
            }
        }
        
        private bool TableEdited()
        {
            return true;
            
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
                    if (PlayerPrefs.HasKey("GoogleSheetEdited") && result != PlayerPrefs.GetString("GoogleSheetEdited"))
                    {
                        PlayerPrefs.SetString("GoogleSheetEdited", result);
                        Debug.Log("GS data edited at " + result + " and need update");
                        return true;
                    }
                    
                    Debug.Log("GS data don't changed");
                    return false;
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false;
        }
    }