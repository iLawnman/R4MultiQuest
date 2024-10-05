/// Credit .entity
/// Sourced from - http://forum.unity3d.com/threads/rescale-panel.309226/

using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Extensions/RescalePanels/RescaleDragPanel")]
    public class RescaleDragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] private Vector2 pointerOffset;
        [SerializeField] private RectTransform canvasRectTransform;
        [SerializeField] private RectTransform panelRectTransform;

        [SerializeField] private Transform goTransform;

        void Awake()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                canvasRectTransform = canvas.transform as RectTransform;
                panelRectTransform = transform.parent as RectTransform;
                goTransform = transform.parent;
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            //Debug.Log("down " + data.position);
            panelRectTransform.SetAsLastSibling();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out pointerOffset);
        }

        public void OnDrag(PointerEventData data)
        {
            Debug.Log("drug " + data.delta);

            if (panelRectTransform == null)
                return;

            Vector2 pointerPosition = ClampToWindow(data);

            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, pointerPosition, data.pressEventCamera, out localPointerPosition
                ))
            {
                panelRectTransform.localPosition = localPointerPosition - new Vector2(pointerOffset.x * goTransform.localScale.x, pointerOffset.y * goTransform.localScale.y);
            }
        }

        Vector2 ClampWindow(Vector2 futurePosition)
        {
            Vector3[] canvasCorners = new Vector3[4];
            canvasRectTransform.GetWorldCorners(canvasCorners);

            float clampedX = Mathf.Clamp(futurePosition.x, 0, Screen.width / 2);
            float clampedY = Mathf.Clamp(futurePosition.y, 0, Screen.height / 2);
            Vector2 newPosition = new Vector2(clampedX, clampedY);

            return newPosition;
        }

        Vector2 ClampToWindow(PointerEventData data)
        {
            Vector2 rawPointerPosition = data.position;

            Vector3[] canvasCorners = new Vector3[4];
            canvasRectTransform.GetWorldCorners(canvasCorners);

            Debug.Log(canvasCorners[0].x + " / " + canvasCorners[2].y);

            float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
            float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

            Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
            return newPointerPosition;
        }
    }
}