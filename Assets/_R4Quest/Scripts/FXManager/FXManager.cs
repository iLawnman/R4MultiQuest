using System.Threading.Tasks;
using UnityEngine;

public interface IEffect
{
    Task PlayAsync(GameObject target, float duration);
    void StopFX();
}

public class FXManager
{
    public static async Task PlayFx(GameObject target, IEffect effect, float duration)
    {
        await effect.PlayAsync(target, duration);
    }
}