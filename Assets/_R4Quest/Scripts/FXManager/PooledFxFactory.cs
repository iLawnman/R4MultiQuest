using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _R4Quest.Scripts.FXManager
{
    public class PooledFactory : IFactory
    {
        private readonly FxPool _Pool;

        public PooledFactory(FxPool fxPool)
        {
            _Pool = fxPool;
        }

        public UniTask<GameObject> CreateAsync(GameObject prefab, Transform parent)
        {
            var fxInstance = _Pool.Get();
            fxInstance.transform.SetParent(parent);
            fxInstance.SetActive(true);
            return UniTask.FromResult(fxInstance);
        }
    }
}