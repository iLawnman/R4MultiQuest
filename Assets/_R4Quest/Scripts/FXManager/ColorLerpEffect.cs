using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ColorLerpEffect : IEffect
{
    private SpriteRenderer spriteRenderer;
    private Color startColor;
    private Color targetColor;
    private float duration;

    public ColorLerpEffect(SpriteRenderer spriteRenderer, Color targetColor, float duration)
    {
        this.spriteRenderer = spriteRenderer;
        this.startColor = spriteRenderer.color;
        this.targetColor = targetColor;
        this.duration = duration;
    }

    public async Task<UniTask> PlayAsync(GameObject target, float duration1)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            spriteRenderer.color = Color.Lerp(startColor, targetColor, t);
            await Task.Yield();  // Возвращаем управление, чтобы другие задачи могли выполняться
        }

        spriteRenderer.color = targetColor; // Устанавливаем окончательный цвет
        return UniTask.CompletedTask;
    }

    public void StopFX()
    {
        //PlayAsync().Dispose();
    }
}