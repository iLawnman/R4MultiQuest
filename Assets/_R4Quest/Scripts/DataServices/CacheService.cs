using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

public class CacheService : IStartable
{
    private static string cacheDirectory = Application.persistentDataPath + "/Cache/";
    private static Dictionary<string, byte[]> cachedObjects = new Dictionary<string, byte[]>();

    public void Start()
    {
        cachedObjects.Clear();
        UpdateCache();
    }

    public async UniTask<Sprite> LoadCachedImageAsync(string fileName, int textureSize, float vectorSize)
    {
        UpdateCache();
        try
        {
            var filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"Файл {filePath} не найден в кеше.");
                return null;
            }

            byte[] imageData = await File.ReadAllBytesAsync(filePath);

            Texture2D texture = new Texture2D(textureSize, textureSize);
            if (!texture.LoadImage(imageData))
            {
                Debug.LogError("Не удалось загрузить изображение из данных.");
                return null;
            }

            Sprite a = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(vectorSize, vectorSize));
            a.name = fileName;

            UpdateCache();
            return a;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(cacheDirectory, fileName);
    }

    public static void UpdateCache()
    {
        var cachedObjectNames = Directory.GetFiles(cacheDirectory).ToList();
        cachedObjectNames.ForEach(x =>
        {
            var fileName = Path.GetFileName(x);
            if (!cachedObjects.ContainsKey(fileName))
            {
                var bytes = File.ReadAllBytes(x);
                cachedObjects.Add(fileName, bytes);
            }
        });
        Debug.Log("updated cache count " + cachedObjects.Count);
    }

    public static Sprite GetCachedImage(string assetName)
    {
        if (string.IsNullOrWhiteSpace(assetName) || assetName == ".png")
            return null;

        if (cachedObjects.ContainsKey(assetName))
        {
            byte[] bytes = cachedObjects.FirstOrDefault(x => x.Key == assetName).Value;
            Texture2D texture = new Texture2D(1024, 1024);
            if (!texture.LoadImage(bytes))
            {
                Debug.LogError("Не удалось загрузить изображение из данных.");
                return null;
            }

            Sprite a = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            a.name = assetName;
            return a;
        }
        return null;
    }

    public byte[] LoadCachedObject(string name)
    {
        return cachedObjects[name];
    }

    public Dictionary<string, byte[]> LoadCachedObjects(string nameMask)
    {
        var _objs = cachedObjects.Where(x => x.Key.Contains(nameMask)).ToList();
        if (_objs.Count > 0)
        {
            Dictionary<string, byte[]> objs = new Dictionary<string, byte[]>();
            _objs.ForEach(x => objs.Add(x.Key, x.Value));
            return objs;
        }
        return null;
    }
}