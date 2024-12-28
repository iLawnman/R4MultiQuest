using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class GoogleSheetsService
{
    async UniTask<List<string>> FetchLinksFromGoogleSheet()
    {
        var scriptUrl =
            "https://script.google.com/macros/s/AKfycbxBpEA7jspwLve_y6RaV6wU54SOUlhGHMS_SjQDkVaiE-JBlb7vExYrlRkSumw3YHED2w/exec";
        
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