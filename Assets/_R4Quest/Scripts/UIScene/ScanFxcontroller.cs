using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScanFxcontroller : MonoBehaviour, IUISkin
{
    [SerializeField] private ScanFXSkin skin;
    [SerializeField] private GameObject FxPanel;
    [SerializeField] private GameObject frame;

    private void Start()
    {
        UIActions.OnShowScenFX += ShowScenFX;
        SetSkin(skin);
    }

    private void OnDisable()
    {
        UIActions.OnShowScenFX -= ShowScenFX;
    }

    public void SetSkin(UISkin uiSkin)
    {
        skin = uiSkin as ScanFXSkin;
        frame.GetComponent<Image>().sprite = skin.FrameSprite;
        frame.GetComponent<Image>().color = skin.fxColor;
        FxPanel.GetComponentInChildren<Text>().font = skin.TxtFont;
        FxPanel.GetComponentInChildren<Text>().color = skin.fxColor;
    }

    private void ShowScenFX(bool state, int duration)
    {
        StartCoroutine(showFX(state, duration));
    }

    IEnumerator showFX(bool state, float duration)
    {
        FxPanel.SetActive(state);
        Debug.Log(("ShowScenFX: " + state));
        if (state)
        {
            yield return new WaitForSeconds(duration);
            FxPanel.SetActive(false); // Disable the effect after the duration
        }
    }
}