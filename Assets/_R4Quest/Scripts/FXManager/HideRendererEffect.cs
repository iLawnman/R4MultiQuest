using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class HideRendererEffect : IEffect
{
    public async Task PlayAsync(GameObject target, float duration)
    {
        foreach (SpriteRenderer spriteRenderer in target.GetComponentsInChildren<SpriteRenderer>())
        {
            // if (spriteRenderer.gameObject.GetComponentsInChildren<IEffect>().Length > 0)
            //     spriteRenderer.gameObject.GetComponentsInChildren<IEffect>().ForEach(x => x.StopFX());

            spriteRenderer.DOFade(0, duration); //.onComplete = () => spriteRenderer.gameObject.SetActive(false);
        }    
    }

    public void StopFX()
    {
        //PlayAsync().Dispose();
    }
}