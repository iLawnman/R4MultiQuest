using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class TEST_Mono : MonoBehaviour
{
    [SerializeField] private KeyCode keyCode;
    [SerializeField] List<string> localDirectories = new List<string>(){"Assets/_Pirates/PiratesPictures", "Assets/_Pirates/RecognitionImages", "Assets/_Pirates/PiratesSounds"};
    [SerializeField] List<string> remoteDirectories = new List<string>(){"Pictures/", "RecognitionImages/", "Sounds/"};
    [SerializeField] private string siteUrl = "http://r4quest.ru/r4questdata/Pirates/";
    [SerializeField] private string cachePath = "/Cache/"; 
    
    [ContextMenu("DO metadata")]
    private void DoMetadata()
    {
        foreach (var directory in localDirectories)
        {
            string[] filePaths = Directory.GetFiles(directory);
            List<string> _fileNames = filePaths.Select(Path.GetFileName).ToList();
            List<string> fileNames = new List<string>();
            _fileNames.ForEach(x=>
            {
                if(!x.Contains(".meta") && !x.Contains(".DS_Store")&& !x.Contains(".json"))
                    fileNames.Add(x);
            });
            string json = JsonConvert.SerializeObject(fileNames, Formatting.Indented);
            File.WriteAllText(Path.Combine(directory, "metadata.json"), json);

            Debug.Log("Список имен файлов сохранен в metadata.json");
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(keyCode))
        {
            GetFileList().Forget();
        }
    }

    [ContextMenu("CreateReferenceLibrary")]
    public async UniTask CreateReferenceLibrary()
    {
        //get all targets from cache
        //create library
    }

    [ContextMenu("GetFiles")]
    public async UniTask GetFileList()
    {
        foreach (var directory in remoteDirectories)
        {
            try
            {
                var url = siteUrl + directory;
                Dictionary<string, DateTime> remoteFiles = await FetchMetadataAsync(url);
                
                foreach (var file in remoteFiles)
                {
                    if (File.Exists(cachePath + file.Key))
                    {
                        DateTime localUpdated = File.GetLastWriteTimeUtc(cachePath);
                        if (file.Value > localUpdated)
                        {
                            Debug.Log("update " + file.Key + " with " + file.Value + " / " + localUpdated);
                            await DownloadFileAsync(file.Key, url, cachePath);
                        }
                    }
                    else
                    {
                        Debug.Log("download " + file.Key);
                        await DownloadFileAsync(file.Key, url, cachePath);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        Debug.Log("cache files updated");
    }

    private async UniTask<Dictionary<string, DateTime>> FetchMetadataAsync(string MetadataUrl)
    {
        Dictionary<string, DateTime> remoteFiles = new Dictionary<string, DateTime>();
        var request = UnityWebRequest.Get(MetadataUrl + "/metadata.json");
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Не удалось получить metadata.json: " + request.error);
            return null;
        }

        string json = request.downloadHandler.text;
        var files = JsonConvert.DeserializeObject<List<string>>(json);
        foreach (var x in files)
        {
            var filePath = MetadataUrl + x;
            using var requestHead = UnityWebRequest.Head(filePath);
            await requestHead.SendWebRequest();

            var fileDate = DateTime.Parse(requestHead.GetResponseHeader("Last-Modified"));
            remoteFiles.Add(x, fileDate);
        };

        return remoteFiles;
    }

    private async UniTask DownloadFileAsync(string name, string url, string cachePath)
    {
        var fullFilePath = Path.Combine(cachePath, name);

        var directoryPath = Path.GetDirectoryName(fullFilePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        using var request = UnityWebRequest.Get(url + name);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Не удалось загрузить файл {name}: {request.error}");
            return;
        }

        try
        {
            File.WriteAllBytes(fullFilePath, request.downloadHandler.data);
            Debug.Log($"Файл {fullFilePath} обновлен.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [Serializable]
    public class MetadataList
    {
        public List<FileMetadata> files;
    }

    [Serializable]
    public class FileMetadata
    {
        public string filename;
        public DateTime updated;
    }
}