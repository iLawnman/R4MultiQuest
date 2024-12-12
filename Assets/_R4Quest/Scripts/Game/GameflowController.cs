using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using VContainer;

public class GameflowController : MonoBehaviour
{
    [Inject] private CacheService _cacheService;
    [Inject] private LuaScriptService _luaScriptService;
    [Inject] private GameObjectsFactory gameObjectsFactory;
    [Inject] private ConfigDataContainer container;

    public void Start()
    {
        Application.runInBackground = false;

        ARSceneActions.OnARTrackedImage += OnARTrackedImage;

        GameActions.CallQuestStart += CallQuestStart;
        GameActions.OnQuestStart += OnQuestStart;
        GameActions.OnQuestComplete += OnQuestComplete;
    }

    void OnARTrackedImage(string imgTrack, Transform position)
    {
        Debug.Log("imgTracked " + imgTrack);
        var q = container.ApplicationData.Quests
            .FirstOrDefault(x => x.RecognitionImage == imgTrack);

        UIActions.OnQuestStart?.Invoke(q, imgTrack);
        UIActions.OnShowScenFX?.Invoke(true, 2);
        gameObjectsFactory.CreateARTarget(q, imgTrack, position);
    }

    void CallQuestStart()
    {
        Debug.Log("gamecontroller CallQuestStart with lua modidifcator");
        _luaScriptService.ExecuteAction("CallQuestStart.lua", "start", "str");
        ARSceneActions.OnARSession?.Invoke();
        ARSceneActions.OnReadyForTracking?.Invoke(true);
    }

    void OnQuestStart(string quest)
    {
        Debug.Log("luaService onqueststart " + quest);
        container.ApplicationData.CurrentQuest = quest;
        _luaScriptService.ExecuteAction("OnQuestStart", "Start", "null");
    }

    void OnQuestComplete(string questId, bool state)
    {
        Debug.Log("luaService onquestcomplete " + questId + " " + state);
        var q = container.ApplicationData.Quests
            .FirstOrDefault(x => x.QuestID == questId);
        QuestData nextQuest = container.ApplicationData.Quests.FirstOrDefault(x => x.QuestID == q.RightWayQuest);
        UIActions.OnQuestPanel.Invoke(q.RightReaction, q.SignImage);
        container.ApplicationData.CurrentQuest = nextQuest.QuestID;

        _luaScriptService.ExecuteAction("OnQuestComplete", "Complete", "null");

        //bool asyncComplete = Convert.ToBoolean(container.ApplicationSettings.SetWaitNextQuest);
        //if (!asyncComplete)
        {
            ARSceneActions.OnWaitRecognitionImage?.Invoke(nextQuest);
            UIActions.OnQuestPanel.Invoke("ПРОДОЛЖАЙТЕ\nИЩИТЕ ЗНАК КАК НА КАРТИНКЕ", nextQuest.RecognitionImage);
        }
    }
}