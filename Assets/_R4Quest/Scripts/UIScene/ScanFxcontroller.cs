using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static partial class GameActions
{
    public static Action<bool, int> OnShowScenFX;
}

public class ScanFxcontroller : MonoBehaviour, IUISkin
{
    [SerializeField] private ScanFXSkin skin;
    [SerializeField] private GameObject FxPanel;
    [SerializeField] private GameObject frame;
    private void Start()
    {
        GameActions.OnShowScenFX += ShowScenFX;
        SetSkin(skin);
    }

    private void OnDisable()
    {
        GameActions.OnShowScenFX -= ShowScenFX;
    }

    public void SetSkin(UISkin scanFXSkin)
    {
        skin = scanFXSkin as ScanFXSkin;
        frame.GetComponent<Image>().sprite = skin.FrameSprite;
        frame.GetComponent<Image>().color = skin.fxColor;
        FxPanel.GetComponentInChildren<Text>().font = skin.TxtFont;
        FxPanel.GetComponentInChildren<Text>().color = skin.fxColor;
    }
    
    private async void ShowScenFX(bool state, int duration)
    {
        Debug.Log(("ShowScenFX: " + state));
        FxPanel.SetActive(state);
        if (state)
        {
            await Task.Delay(duration);
            FxPanel.SetActive(false);
            frame.SetActive(false);
        }
    }
}