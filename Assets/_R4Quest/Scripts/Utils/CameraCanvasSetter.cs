using UnityEngine;

public class CameraCanvasSetter : MonoBehaviour
{
    void Start()
    {
        var canvas = GetComponent<Canvas>();
        
        canvas.worldCamera = Camera.main;
        canvas.overrideSorting = true;
    }
}
