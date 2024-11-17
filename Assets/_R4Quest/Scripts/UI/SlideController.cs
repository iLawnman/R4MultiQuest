using UnityEngine;
using UnityEngine.Events;

public class SlideController : MonoBehaviour
{

    public UnityEvent SwipeLeft;
    public UnityEvent SwipeRight;

    private Vector2 _startPosition;   
    private Vector2 _endPosition;     
    private float _swipeThreshold = 50f;

    void Update()
    {
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _endPosition = Input.mousePosition;
            ProcessSwipe();
        }
    }

    private void ProcessSwipe()
    {
        float swipeDistance = _endPosition.x - _startPosition.x;

        if (Mathf.Abs(swipeDistance) > _swipeThreshold)
        {
            if (swipeDistance > 0)
                SwipeRight.Invoke();
            else
                SwipeLeft.Invoke();
        }
    }
}
