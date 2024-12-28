using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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

        [ContextMenu("DO")]
        public void Do()
        {
            FetchImagesFromGoogleSheet();
        }

        async UniTask<List<string>> FetchImagesFromGoogleSheet()
        {
            var scriptUrl =
                "https://script.google.com/macros/s/AKfycbxBpEA7jspwLve_y6RaV6wU54SOUlhGHMS_SjQDkVaiE-JBlb7vExYrlRkSumw3YHED2w/exec";
                //"https://script.google.com/macros/s/AKfycbzNBMSd4FzfaSPr389vnHsKTivDyv1Ow64ilmrE_zSDS9WjLy3nd3t4xBFOeKZcoRM-0A/exec";
        
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(scriptUrl);
            request.Method = "GET";
            request.ContentType = "application/json";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = await reader.ReadToEndAsync();
                    List<string> links = JsonConvert.DeserializeObject<List<string>>(result);
                    Debug.Log(links.Count);
                    return links;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка: {e.Message}");
            }
            return null;
        }
    }
}