using System.Linq;
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
        
        GameActions.CallQuestStart += CallQuestStart;
        GameActions.OnQuestStart += OnQuestStart;
        GameActions.OnQuestComplete += OnQuestComplete;
        
        ARSceneActions.OnARTrackedImage += OnARTrackedImage;
    }

    void OnARTrackedImage(ARTrackedImage imgTrack)
    {
        Debug.Log("imgTracked " + imgTrack.referenceImage.texture.name);
        UIActions.OnShowScenFX?.Invoke(true, 2);
        
        var q = container.ApplicationData.Quests
            .FirstOrDefault(x => x.RecognitionImage == imgTrack.referenceImage.texture.name);
        gameObjectsFactory.CreateARTarget(q, imgTrack);
        Debug.Log("q " + q.QuestID);
        UIActions.OnQuestStart?.Invoke(q, imgTrack);    
    }
    
    void CallQuestStart()
    {
        Debug.Log("gamecontroller CallQuestStart with lua modidcator");
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
        var nextQuest = container.ApplicationData.Quests.FirstOrDefault(x => x.QuestID == q.RightWayQuest);
        string imgNextQuest = nextQuest.RecognitionImage;
        UIActions.OnQuestPanel.Invoke(q.RightReaction, imgNextQuest);
        container.ApplicationData.CurrentQuest = nextQuest.QuestID;
    }
}