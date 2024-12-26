using UnityEngine;

public class UIBackdropBlur : MonoBehaviour
{
    public Camera uiCamera;
    public RenderTexture blurTexture;
    public Material blurMaterial;
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = uiCamera;
    }

    void LateUpdate()
    {
        uiCamera.targetTexture = blurTexture;
        uiCamera.Render();
        blurMaterial.SetTexture("_MainTex", blurTexture);
    }
}