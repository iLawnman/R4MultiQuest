using UnityEngine;
using UnityEngine.Events;

public class SlideController : MonoBehaviour
{

    public UnityEvent SwipeLeft;
    public UnityEvent SwipeRight;
    public UnityEvent TouchUp;
    private Vector2 startPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;

                    if (swipeDistHorizontal > 10)
                    {
                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
                        if (swipeValue > 0)
                        {//right swipe
                            Debug.Log("swipe right");
                            SwipeRight.Invoke();
                        }
                        else if (swipeValue < 0)
                        {//left swipe
                            Debug.Log("swipe left");
                            SwipeLeft.Invoke();
                        }
                    }
                    else
                    {
                        TouchUp.Invoke();
                    }
                    break;
            }
        }
    }
}
