using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

public class FileSyncService
{
    List<string> remoteDirectories = new List<string>()
        { "Pictures/", "RecognitionImages/", "Sounds/", "Skins/" };

    private string siteUrl = "https://r4quest.ru/r4questdata/";
    private string cachePath = Application.persistentDataPath + "/Cache/";

    private ApplicationSettings _currentSettings;

    public async UniTask Initilize(ApplicationSettings applicationSettings)
    {
        _currentSettings = applicationSettings;
        siteUrl += _currentSettings.AddressableKey + "/";
        await Start();

        BootstrapActions.OnShowInfo("All Downloaded");
    }

    private async UniTask Start()
    {
        await GetFileList();
    }

    public async UniTask GetFileList()
    {
        foreach (var directory in remoteDirectories)
        {
            await GetDrectoryFiles(directory);
        }

        Debug.Log("cache files updated");
    }

    private async Task GetDrectoryFiles(string directory)
    {
        try
        {
            var url = siteUrl + directory;

            Debug.Log("get remote directory " + url);
            BootstrapActions.OnShowInfo?.Invoke("Fetch remote metadata\n" + directory.Replace("/", string.Empty));
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

    private async UniTask<Dictionary<string, DateTime>> FetchMetadataAsync(string MetadataUrl)
    {
        Dictionary<string, DateTime> remoteFiles = new Dictionary<string, DateTime>();
        var request = UnityWebRequest.Get(MetadataUrl + "/metadata.json");
        Debug.Log("fetch " + MetadataUrl + "/metadata.json");
        await request.SendWebRequest();

        try
        {
            string json = request.downloadHandler.text;
            var files = JsonConvert.DeserializeObject<List<string>>(json);
            foreach (var x in files)
            {
                var filePath = MetadataUrl + x;
                using var requestHead = UnityWebRequest.Head(filePath);
                await requestHead.SendWebRequest();

                var fileDate = DateTime.Parse(requestHead.GetResponseHeader("Last-Modified"));
                remoteFiles.Add(x, fileDate);
            }
        }
        catch (Exception e)
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Не удалось получить {request} metadata.json: " + request.error);
                return null;
            }
            Console.WriteLine(e);
            throw;
        }

        return remoteFiles;
    }

    private async UniTask DownloadFileAsync(string name, string url, string cachePath)
    {
        BootstrapActions.OnShowInfo("downloading\n" + name);

        var fullFilePath = Path.Combine(cachePath, name);

        var directoryPath = Path.GetDirectoryName(fullFilePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        using var request = UnityWebRequest.Get(url + name);
        await request.SendWebRequest();

        try
        {
            File.WriteAllBytes(fullFilePath, request.downloadHandler.data);
            Debug.Log($"Файл {fullFilePath} обновлен.");

            CacheService.UpdateCache();
        }
        catch (Exception e)
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Не удалось загрузить файл {name}: {request.error}");
                return;
            }
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