using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SlidePanelUIFX : MonoBehaviour
{
    [SerializeField] private RectTransform uiElement;
    [SerializeField] private Button closeButton;
    private Vector2 screenSize;

    private void Start()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        closeButton.onClick.AddListener(MoveOutScreen);
    }

    private void OnEnable()
    {
        MoveInScreen();
    }

    private void MoveOutScreen()
    {
        uiElement.DOAnchorPos(new Vector2(0, screenSize.y), 2, true).OnComplete(()=> uiElement.gameObject.SetActive(false));
        
    }
    private void MoveInScreen()
    {
        uiElement.DOAnchorPos(new Vector2(0, 0), 2, true);
    }
}
