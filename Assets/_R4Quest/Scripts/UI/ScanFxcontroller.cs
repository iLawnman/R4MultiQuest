using System;
using System.Threading.Tasks;
using UnityEngine;

public static partial class GameActions
{
    public static Action<bool, int> OnShowScenFX;
}

public class ScanFxcontroller : MonoBehaviour
{
    [SerializeField] private GameObject FxPanel;
    [SerializeField] private GameObject frame;
    private void Awake()
    {
        GameActions.OnShowScenFX += ShowScenFX;
    }

    private void OnDisable()
    {
        GameActions.OnShowScenFX -= ShowScenFX;
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
