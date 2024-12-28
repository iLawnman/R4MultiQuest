using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class ScaleUnhideFX : IEffect
{
    [Header("Play FX by hierarchy order")]
    [SerializeField] private float _duration;
    [SerializeField] private float endScale;

    public async Task<UniTask> PlayAsync(GameObject target, float duration)
    {
        var objs = target.GetComponentsInChildren<Transform>();
        foreach (Transform obj in objs)
        {
            if (obj != target.transform)
            {
                await obj.DOScale(Vector3.zero, 0).AsyncWaitForCompletion();
                await obj.DOScale(endScale, duration).AsyncWaitForCompletion();
            }
        }
        return UniTask.CompletedTask;
    }

    public void StopFX() { }
}