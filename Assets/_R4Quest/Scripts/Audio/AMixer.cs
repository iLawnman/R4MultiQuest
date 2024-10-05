using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AMixer : MonoBehaviour
{
    public bool musicMute;
    public AudioMixerSnapshot def;
    public AudioMixerSnapshot mMute;
    [SerializeField] Button muteButton;

    private void OnEnable()
    {
        muteButton.onClick.AddListener(MusicToggle);
    }

    private void OnDisable()
    {
        muteButton.onClick.RemoveListener(MusicToggle);
    }

    public void MusicToggle()
    {
        musicMute = !musicMute;

        if (musicMute)
        {
            mMute.TransitionTo(1);
            muteButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            def.TransitionTo(1);
            muteButton.GetComponent<Image>().color = Color.white;
        }
    }
}
