using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _R4Quest.Scripts.GoogleSheets
{
    public class GoogleSheetsMono : MonoBehaviour
    {
        [SerializeField] private string credentialsPath;
        [SerializeField] private TextAsset credentials;
        [SerializeField] private string spreadsheetId;
        [SerializeField] private string sheetName;

        void OnEnable()
        {
            LoadImages();
        }

        [ContextMenu("DO")]
        public void Do()
        {
            //LoadImages();
            FetchImagesFromGoogleSheet();
        }

        async Task FetchImagesFromGoogleSheet()
        {
            //https://script.google.com/macros/s/AKfycbzNBMSd4FzfaSPr389vnHsKTivDyv1Ow64ilmrE_zSDS9WjLy3nd3t4xBFOeKZcoRM-0A/exec
            var scriptUrl =
                "https://script.google.com/macros/s/AKfycbzNBMSd4FzfaSPr389vnHsKTivDyv1Ow64ilmrE_zSDS9WjLy3nd3t4xBFOeKZcoRM-0A/exec";
            //string scriptUrl = "https://script.google.com/macros/s/YOUR_SCRIPT_ID/exec";
        
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(scriptUrl);
            request.Method = "GET";
            request.ContentType = "application/json";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    //Debug.Log("resBody " +result);
                    List<string> imageLinks = JsonConvert.DeserializeObject<List<string>>(result);
                    
                    foreach (var link in imageLinks)
                    {
                        Debug.Log("link " +link);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка: {e.Message}");
            }
        }
        private void LoadImages()
        {
            credentialsPath = Path.Combine(Application.dataPath, "Resources/credentials.json");

            var service = new GoogleSheetsService(credentialsPath);
            var imageLinks = service.GetImageLinks(spreadsheetId, sheetName);

            foreach (var link in imageLinks)
            {
                Debug.Log($"Image URL: {link}");
                StartCoroutine(DownloadAndDisplayImage(link));
            }
        }

        private IEnumerator DownloadAndDisplayImage(string url)
        {
            using var www = new UnityEngine.Networking.UnityWebRequest(url);
            www.downloadHandler = new UnityEngine.Networking.DownloadHandlerTexture();

            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((UnityEngine.Networking.DownloadHandlerTexture)www.downloadHandler).texture;
                // Здесь вы можете применить текстуру к объекту
                Debug.Log($"Image downloaded: {url}");
            }
            else
            {
                Debug.LogError($"Failed to download image: {url}, Error: {www.error}");
            }
        }
    }

}