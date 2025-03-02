using UnityEngine;
using UnityEngine.UI;

public class OverlayUI : MonoBehaviour, IUISkin
{
    [SerializeField] private OverlayUISkin skin;
    [SerializeField] private GameObject questTimer;
    [SerializeField] private GameObject totalTimer;
    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private GameObject musicButton;
    [SerializeField] private GameObject lightButton;
    [SerializeField] private GameObject shareButton;
    [SerializeField] private GameObject mapButton;
    [SerializeField] private GameObject questInfoButton;

    private void Start()
    {
        SetSkin(skin);
        questInfoButton.GetComponent<Button>().onClick.AddListener(SendOnQuestPanelCurrent);
    }
    private void SendOnQuestPanelCurrent()
    {
        UIActions.OnQuestPanelCurrent.Invoke();
    }
    public void SetSkin(UISkin uiSkin)
    {
        skin = uiSkin as OverlayUISkin;
        musicButton.GetComponent<Image>().sprite = skin.musicButton;
        lightButton.GetComponent<Image>().sprite = skin.lightButton;
        shareButton.GetComponent<Image>().sprite = skin.shareButton;
        mapButton.GetComponent<Image>().sprite = skin.mapButton;
        questInfoButton.GetComponent<Image>().sprite = skin.questInfoButton;
    }
}