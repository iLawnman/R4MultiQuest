using DG.Tweening;
using UnityEngine;

public class SpriteColorLerp : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Color Color = Color.gray;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeColor();
    }

    void ChangeColor()
    {
        if(!enabled)
            return;
        
        Color randomColor = new Color(Color.r, Color.g, Color.b, Random.value);

        spriteRenderer.DOColor(randomColor, 1.5f).OnComplete(() => {
            ChangeColor();
        });
    }
}
