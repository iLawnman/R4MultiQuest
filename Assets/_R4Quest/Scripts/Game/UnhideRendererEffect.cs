using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnhideRendererEffect : IEffect
{
    public async UniTask PlayAsync(GameObject target, float duration)
    {
        foreach (SpriteRenderer spriteRenderer in target.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.DOFade(0, 0); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        }

        foreach (Image image in target.GetComponentsInChildren<Image>())
        {
            image.DOFade(0, 0); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        }

        foreach (TMP_Text tmpText in target.GetComponentsInChildren<TMP_Text>())
        {
            tmpText.DOFade(0, 0); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        }

        UnHide(target, duration);
    }

    UniTask UnHide(GameObject target, float duration)
    {
        foreach (SpriteRenderer spriteRenderer in target.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.DOFade(1, duration); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        }

        foreach (Image image in target.GetComponentsInChildren<Image>())
        {
            image.DOFade(1, duration); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        }

        foreach (TMP_Text tmpText in target.GetComponentsInChildren<TMP_Text>())
        {
            tmpText.DOFade(1, duration); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        }

        return default;
    }

    public void StopFX()
    {
        throw new NotImplementedException();
    }
}