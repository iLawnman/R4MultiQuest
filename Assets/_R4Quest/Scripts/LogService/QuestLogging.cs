using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using VContainer.Unity;

public class QuestLogging : IStartable, ILogging
{
    //https://api.telegram.org/bot6371359479:AAHHgeEJbldkm-s_MSpcA2voTBYkqRiLA1g/sendmessage?chat_id=-1002214368660&text=%D0%A3%D0%B4%D0%B0%D1%87%D0%BD%D0%BE
    private string _postApi = "https://api.telegram.org/bot6371359479:AAHHgeEJbldkm-s_MSpcA2voTBYkqRiLA1g/sendmessage?chat_id=-1002214368660&text="; 
    //"%D0%A3%D0%B4%D0%B0%D1%87%D0%BD%D0%BE"; // "{ \"field1\": 1, \"field2\": 2 }", "application/json";
    private Queue debugMessageQue = new Queue();
    public string _name = "0";
    public string time;
    private readonly ApplicationSettings _applicationSettings;

    public QuestLogging(ApplicationSettings applicationSettings)
    {
        _applicationSettings = applicationSettings;
    }
    
    public void Start()
    {
        GameActions.SendCurrentStep += SendCurrentStep;
    }

    public void SendCurrentStep(string data)
    {
        // if(!_applicationSettings.QuestLogging)
        //     return;
        
        _name = PlayerPrefs.GetString("PlayerName") + " / " + PlayerPrefs.GetString("PlayerTeam");

        string message = _name + " " + data + " : " + DateTime.Now.ToString("hh:mm:ss");
        debugMessageQue.Enqueue(message);
        
        if(debugMessageQue.Count == 1)
            SendingQueue();
    }

    private async void SendingQueue()
    {
        Debug.Log("start send queue ");

        while (debugMessageQue.Count > 0)
        {
            var sendata = debugMessageQue.Dequeue().ToString();
            using (UnityWebRequest www = UnityWebRequest.PostWwwForm(
                       _postApi + sendata, sendata))
            {
                await www.SendWebRequest();
            
                if (www.result != UnityWebRequest.Result.Success)
                {
                    debugMessageQue.Enqueue(sendata);
                    Debug.LogError(www.error + "\nResend");
                }
                else
                {
                    Debug.Log("Step send complete!" 
                              + "\n" + www.result);
                }
            }
        }
    }
}