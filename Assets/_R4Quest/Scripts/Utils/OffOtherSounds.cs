using System.Linq;
using UnityEngine;

public class OffOtherSounds : MonoBehaviour
{
    private void OnEnable()
    {
        OffOtherAudioSource();
    }

    private void OffOtherAudioSource()
    {
        var allAudio = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

        if (allAudio.Count > 0)
        {
            allAudio.ForEach(x => x.Stop());
        }
        GetComponent<AudioSource>().Play();
    }

    private void OnDisable()
    {
        //GameObject.Find("InfoPanel+").GetComponent<AudioSource>().Play();
    }
}
