using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;

public class AudioService
{
    private string cacheDirectory = Application.persistentDataPath + "/Cache/";
    private AudioSource playingLoop;

    private async UniTask<AudioClip> GetClip(string audioName)
    {
        var audioClip = await LoadMP3FromCache(cacheDirectory + audioName);
        return audioClip;
    }

    async UniTask<AudioClip> LoadMP3FromCache(string filePath)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG);
        try
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerAudioClip.GetContent(request);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return null;
    }

    public async UniTask PlayLoop2D(string loopName)
    {
        Debug.Log("call play 2dloop " + loopName);
        var clip = await GetClip(loopName);
        if (clip == null)
        {
            Debug.Log("no clip in cache " + loopName);
            return;
        }

        PlayLoop2D(clip);
    }

    public async UniTask PlayOneShot2D(string oneShotName)
    {
        Debug.Log("call play 2dshot " + oneShotName);
        await UniTask.CompletedTask;
    }

    public async UniTask PlayLoop3D(string loopName, Transform transform)
    {
        Debug.Log("call play 3dloop " + loopName);
        await UniTask.CompletedTask;
    }

    public async UniTask PlayOneShot3D(string oneShotName, Transform transform)
    {
        Debug.Log("call play 3dshot " + oneShotName);
        await UniTask.CompletedTask;
    }

    public void PlayLoop2D(AudioClip clip)
    {
        var aSource = AudioSourcesPool.Get();
        aSource.clip = clip;
        aSource.loop = true;
        aSource.volume = 0;
        
        if(playingLoop != null)
            playingLoop.DOFade(0, 3).OnComplete(() => AudioSourcesPool.Return(playingLoop));
        aSource.Play();
        aSource.DOFade(1, 3).OnComplete(() => playingLoop = aSource);
    }
}