using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class AudioService
{
    private string cacheDirectory = Application.persistentDataPath + "/Cache/";
    
    public async UniTask GetClip(string audioName)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(cacheDirectory, AudioType.MPEG))
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Ошибка загрузки аудио: {request.error}");
            }

            // Получение AudioClip из запроса
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
        }
    }

    public void PlayLoop(string loopName)
    {
        
    }

    public void PlayOneShot(string oneShotName)
    {
        
    }
}