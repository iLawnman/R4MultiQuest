using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _R4Quest.Scripts.FXManager
{
    public class FxLifetimeManager : IFxLifetimeManager
    {
        private readonly FxPool _fxPool;

        public FxLifetimeManager(FxPool fxPool)
        {
            _fxPool = fxPool;
        }

        public async UniTask ManageFxLifetime(GameObject fxInstance, float duration)
        {
            await UniTask.Delay((int)(duration * 1000));
            _fxPool.Release(fxInstance);
        }
    }
}