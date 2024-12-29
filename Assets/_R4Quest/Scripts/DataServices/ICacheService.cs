using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

public interface ICacheService : IStartable
{
    void Start();
    UniTask<Sprite> LoadCachedImageAsync(string fileName, int textureSize, float vectorSize);
     void UpdateCache();
     Sprite GetCachedImage(string objAlertPanelBack);
     Dictionary<string, byte[]> LoadCachedObjects(string lua);
}