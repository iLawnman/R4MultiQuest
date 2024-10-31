using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

public class FileSyncService
{
    private Dictionary<string, string> remoteFolders = new Dictionary<string, string>
    {
        { "pictures", "Pictures/" },
        { "recongitionImages", "RecognitionImages/" },
        { "sounds", "Sounds/" }
    };

    private string localCachePath = Application.persistentDataPath + "/Cache";

    private Dictionary<string, Dictionary<string, DateTime>> folderFileDates = new Dictionary<string, Dictionary<string, DateTime>>();
    private string siteUrl = "https://r4quest.ru/r4questdata/";
    private ApplicationSettings _currentSettings;
    
    public async UniTask Initilize(ApplicationSettings applicationSettings)
    {
        _currentSettings = applicationSettings;
        await Start();
    }

    private async UniTask Start()
    {
        LoadLocalMetadata();
        await SyncFilesWithRemote();
        SaveLocalMetadata();
    }
    
    private void LoadLocalMetadata()
    {
        foreach (var folderType in remoteFolders.Keys)
        {
            string metadataPath = Path.Combine(localCachePath, folderType, "metadata.json");
            if (File.Exists(metadataPath))
            {
                string json = File.ReadAllText(metadataPath);
                var fileDates = JsonUtility.FromJson<Dictionary<string, DateTime>>(json);
                folderFileDates[folderType] = fileDates;
            }
            else
            {
                folderFileDates[folderType] = new Dictionary<string, DateTime>();
            }
        }
    }

    private void SaveLocalMetadata()
    {
        foreach (var folderEntry in folderFileDates)
        {
            string folderType = folderEntry.Key;
            string metadataPath = Path.Combine(localCachePath, folderType, "metadata.json");

            string json = JsonUtility.ToJson(folderEntry.Value);
            Directory.CreateDirectory(Path.GetDirectoryName(metadataPath));
            File.WriteAllText(metadataPath, json);
        }
    }

    private async UniTask SyncFilesWithRemote()
    {
        foreach (var folderEntry in remoteFolders)
        {
            string folderType = folderEntry.Key;
            string folderUrl = folderEntry.Value;
            var fileDates = folderFileDates[folderType];

            List<string> remoteFiles = await GetRemoteFileList(folderUrl);

            // Удаляем локальные записи, которых нет на сервере
            foreach (var fileName in new List<string>(fileDates.Keys))
            {
                if (!remoteFiles.Contains(fileName))
                {
                    fileDates.Remove(fileName);
                    string localFilePath = Path.Combine(localCachePath, folderType, fileName);
                    if (File.Exists(localFilePath))
                    {
                        File.Delete(localFilePath);
                        Debug.Log($"Локальный файл {fileName} удален, так как его нет на сервере.");
                    }
                }
            }

            // Скачиваем или обновляем недостающие файлы
            foreach (var fileName in remoteFiles)
            {
                DateTime localDate = fileDates.ContainsKey(fileName) ? fileDates[fileName] : DateTime.MinValue;
                DateTime? remoteDate = await GetRemoteFileLastModified(folderUrl, fileName);

                if (remoteDate.HasValue && (remoteDate > localDate || !fileDates.ContainsKey(fileName)))
                {
                    await DownloadFile(folderType, folderUrl, fileName);
                    fileDates[fileName] = remoteDate.Value;
                }
            }
        }
    }

    private async UniTask<List<string>> GetRemoteFileList(string folderUrl)
    {
        List<string> fileList = new List<string>();
        var _folderUrl = siteUrl + folderUrl;
        
        var info = new DirectoryInfo(_folderUrl);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            Debug.Log("file " + file);
            fileList.Add(file.Name);
        }
        
        // using (UnityWebRequest request = UnityWebRequest.Get(_folderUrl))
        // {
        //     await request.SendWebRequest();
        //
        //     if (request.result != UnityWebRequest.Result.Success)
        //     {
        //         Debug.LogError($"Ошибка при получении списка файлов из {_folderUrl}");
        //         return fileList;
        //     }
        //
        //     string json = request.downloadHandler.text;
        //     fileList = JsonUtility.FromJson<List<string>>(json);
        // }

        return fileList;
    }

    private async UniTask<DateTime?> GetRemoteFileLastModified(string folderUrl, string fileName)
    {
        using (UnityWebRequest request = UnityWebRequest.Head(folderUrl + fileName))
        {
            await request.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"Ошибка при проверке даты обновления файла: {fileName}");
                return null;
            }

            string lastModifiedString = request.GetResponseHeader("Last-Modified");
            if (DateTime.TryParse(lastModifiedString, out DateTime remoteDate))
            {
                return remoteDate;
            }
            else
            {
                Debug.LogWarning($"Невозможно разобрать Last-Modified для файла: {fileName}");
                return null;
            }
        }
    }

    private async UniTask DownloadFile(string folderType, string folderUrl, string fileName)
    {
        string filePath = Path.Combine(localCachePath, folderType, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        if (fileName.EndsWith(".png"))
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(folderUrl + fileName))
            {
                await request.SendWebRequest().ToUniTask();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Ошибка при загрузке изображения: {fileName}");
                    return;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                byte[] textureData = texture.EncodeToPNG();
                await UniTask.SwitchToThreadPool();
                File.WriteAllBytes(filePath, textureData);
                await UniTask.SwitchToMainThread();
                Debug.Log($"Изображение {fileName} успешно скачано и сохранено.");
            }
        }
        else if (fileName.EndsWith(".mp3"))
        {
            using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(folderUrl + fileName, AudioType.MPEG))
            {
                await request.SendWebRequest().ToUniTask();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Ошибка при загрузке аудио: {fileName}");
                    return;
                }

                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                // byte[] audioData = WavUtility.FromAudioClip(audioClip);
                // await UniTask.SwitchToThreadPool();
                // File.WriteAllBytes(filePath, audioData);
                // await UniTask.SwitchToMainThread();
                Debug.Log($"Аудиофайл {fileName} успешно скачан и сохранен.");
            }
        }
        else
        {
            using (UnityWebRequest request = UnityWebRequest.Get(folderUrl + fileName))
            {
                await request.SendWebRequest().ToUniTask();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Ошибка при загрузке файла: {fileName}");
                    return;
                }

                byte[] fileData = request.downloadHandler.data;
                await UniTask.SwitchToThreadPool();
                File.WriteAllBytes(filePath, fileData);
                await UniTask.SwitchToMainThread();
                Debug.Log($"Файл {fileName} успешно скачан и сохранен.");
            }
        }
    }

    public byte[] GetFileFromCache(string folderType, string fileName)
    {
        string filePath = Path.Combine(localCachePath, folderType, fileName);

        if (File.Exists(filePath))
            return File.ReadAllBytes(filePath);

        Debug.LogWarning($"Файл {fileName} не найден в локальном репозитории.");
        return null;
    }
}
