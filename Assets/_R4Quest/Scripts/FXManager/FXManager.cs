using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IEffect
{
    Task<UniTask> PlayAsync(GameObject target, float duration);
    void StopFX();
}

public class FXManager
{
    public static async UniTask PlayFx(GameObject target, IEffect effect, float duration)
    {
        await effect.PlayAsync(target, duration);
    }
}