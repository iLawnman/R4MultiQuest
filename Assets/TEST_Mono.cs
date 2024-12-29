using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TEST_Mono : MonoBehaviour
{
    //[SerializeField] private ARTrackedImageManager TrackedImageManager;
    //private MutableRuntimeReferenceImageLibrary mutableLibrary;

    [SerializeField] private KeyCode keyCode;
    [SerializeField] private UISceneSkin skin;
    [SerializeField] private Image image;

    [SerializeField] List<string> localDirectories = new List<string>()
        { "Assets/_Pirates/PiratesPictures", "Assets/_Pirates/RecognitionImages", "Assets/_Pirates/PiratesSounds" };

    [SerializeField] List<string> remoteDirectories = new List<string>()
        { "Pictures/", "RecognitionImages/", "Sounds/" };

    [SerializeField] private string siteUrl = "http://r4quest.ru/r4questdata/Pirates/";
    [SerializeField] private string cachePath = "Cache/";

    [ContextMenu("DO metadata")]
    private void DoMetadata()
    {
        foreach (var directory in localDirectories)
        {
            string[] filePaths = Directory.GetFiles(directory);
            List<string> _fileNames = filePaths.Select(Path.GetFileName).ToList();
            List<string> fileNames = new List<string>();
            _fileNames.ForEach(x =>
            {
                if (!x.Contains(".meta") && !x.Contains(".DS_Store") && !x.Contains(".json"))
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
            //SetSkin(skin).Forget();
        }
    }

    private void Start()
    {
        //CreateReferenceLibraryAsync().Forget();
    }

    // private async UniTask SetSkin(UISkin uiSceneSkin)
    // {
    //     CacheService cacheService = new CacheService();
    //     var _skin = uiSceneSkin as UISceneSkin;
    //     var a = await cacheService.LoadCachedImageAsync(_skin.background.name + ".png", 1024, 1);
    //     image.sprite = a;
    // }

    /*
    [ContextMenu("CreateReferenceLibrary")]
    public async UniTask CreateReferenceLibraryAsync()
    {
        var _library = TrackedImageManager.CreateRuntimeLibrary();

        if (TrackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary library)
        {
            mutableLibrary = library;
            TrackedImageManager.enabled = false;
            //MutableRuntimeReferenceImageLibrary mutableLibrary = TrackedImageManager.CreateRuntimeLibrary() as MutableRuntimeReferenceImageLibrary;

            for (int i = 0; i < 15; i++)
            {
                string _name = "tgid00" + i.ToString("D2");
                ;
                //Sprite img = await cacheService.LoadCachedImageAsync(_name, 1024, 1);
                //string path = Path.Combine("Cache/", _name);
                string path = Path.Combine("Assets/_Pirates/RecognitionImages/", _name + ".png");
                try
                {
                    //ResourceRequest request = Resources.LoadAsync<Texture2D>(path);
                    CacheService cacheService = new CacheService();
                    var texture = await cacheService.LoadCachedTextureAsync(path, 1024);

                    if (texture != null)
                    {
                        texture.name = _name;
                        if (_library is MutableRuntimeReferenceImageLibrary _mutableLibrary)
                        {
                            _mutableLibrary.ScheduleAddImageWithValidationJob(
                                texture,
                                _name,
                                0.2f);
                        //}
                        // AddReferenceImageJobState handle =
                        //     mutableLibrary.ScheduleAddImageWithValidationJob(texture, _name, 0.2f);
                        // await handle.jobHandle;

                        // Debug.Log("handle " + handle.status);
                        //
                        // if (handle.status == AddReferenceImageJobStatus.Success)
                        // {
                        //     var referenceImage = mutableLibrary[mutableLibrary.count - 1];
                        //     referenceImage.texture.name = _name;
                        // }
                    /*}
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            // foreach (var refer in mutableLibrary)
            // {
            //     Debug.Log("refer " + refer.texture.name);
            // }
            TrackedImageManager.referenceLibrary = mutableLibrary;
            TrackedImageManager.enabled = true;
            Debug.Log("end " + TrackedImageManager.referenceLibrary[0].name);
        }
        else
        {
            Debug.LogError("Изменяемая библиотека изображений не поддерживается.");
        }
    }

    struct DeallocateJob : IJob
    {
        [DeallocateOnJobCompletion] public NativeArray<byte> data;

        public void Execute()
        {
        }
    }

    void AddImage(NativeArray<byte> grayscaleImageBytes,
        int widthInPixels, int heightInPixels,
        float widthInMeters)
    {
        if (!(ARSession.state == ARSessionState.SessionInitializing ||
              ARSession.state == ARSessionState.SessionTracking))
            return; // Session state is invalid

        if (TrackedImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            var aspectRatio = (float)widthInPixels / (float)heightInPixels;
            var sizeInMeters = new Vector2(widthInMeters, widthInMeters * aspectRatio);
            var referenceImage = new XRReferenceImage(
                // Guid is assigned after image is added
                SerializableGuid.empty,
                // No texture associated with this reference image
                SerializableGuid.empty,
                sizeInMeters, "My Image", null);

            var jobState = mutableLibrary.ScheduleAddImageWithValidationJob(
                grayscaleImageBytes,
                new Vector2Int(widthInPixels, heightInPixels),
                TextureFormat.R8,
                referenceImage);

            // Schedule a job that deallocates the image bytes after the image
            // is added to the reference image library.
            new DeallocateJob { data = grayscaleImageBytes }.Schedule(jobState.jobHandle);
        }
        else
        {
            // Cannot add the image, so dispose its memory.
            grayscaleImageBytes.Dispose();
        }
    }
*/
    public async UniTask SetAllSkins()
    {
        // CacheService cacheService = new CacheService();
        //
        // var a = await cacheService.GetFilesByPrefix("tgid00");
        //
        // foreach (var clip in a)
        // {
        //     Debug.Log("clip " + clip.ToString());
        //     Sprite aSprite = clip as Sprite;
        //     image.sprite = aSprite;
        //     await UniTask.Delay(2000);
        // }
    }

    [ContextMenu("GetFiles")]
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
        }
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

            //CacheService.UpdateCache();
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