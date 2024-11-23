using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _R4Quest.Scripts.FXManager
{
    public interface IFxLifetimeManager
    {
        UniTask ManageFxLifetime(GameObject fxInstance, float duration);
    }
}