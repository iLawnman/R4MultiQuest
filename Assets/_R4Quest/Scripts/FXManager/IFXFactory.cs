using Cysharp.Threading.Tasks;
using UnityEngine;


    public interface IFactory
    {
        UniTask<GameObject> CreateAsync(GameObject prefab, Transform parent);
    }
