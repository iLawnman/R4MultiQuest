using System.Collections.Generic;
using UnityEngine;

public class AudioSourcesPool
{
    private static readonly List<AudioSource> pool = new List<AudioSource>();

    public static AudioSource Get()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] != null && !pool[i].isPlaying)
            {
                return pool[i];
            }
        }

        var newAudioSource = new GameObject("PooledAudioSource").AddComponent<AudioSource>();
        Object.DontDestroyOnLoad(newAudioSource.gameObject); 
        pool.Add(newAudioSource);
        return newAudioSource;
    }

    public static void Return(AudioSource audioSource)
    {
        if (audioSource == null) return;

        audioSource.Stop();
        audioSource.clip = null;

        if (audioSource.gameObject != null)
        {
            audioSource.gameObject.SetActive(false);
        }

        if (!pool.Contains(audioSource))
        {
            pool.Add(audioSource);
        }
    }

    public static void Clear()
    {
        foreach (var audioSource in pool)
        {
            if (audioSource != null && audioSource.gameObject != null)
            {
                Object.Destroy(audioSource.gameObject);
            }
        }
        pool.Clear();
    }
}