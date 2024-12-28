using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HideRendererEffect : IEffect
{
    public async Task<UniTask> PlayAsync(GameObject target, float duration)
    {
        foreach (SpriteRenderer spriteRenderer in target.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.DOFade(0, duration); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        } 
        foreach (Image image in target.GetComponentsInChildren<Image>())
        {
            image.DOFade(0, duration); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        }  
        foreach (TMP_Text tmpText in target.GetComponentsInChildren<TMP_Text>())
        {
            tmpText.DOFade(0, duration); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        } 
        return UniTask.CompletedTask;
    }

    public void StopFX()
    {
        //PlayAsync().Dispose();
    }
}