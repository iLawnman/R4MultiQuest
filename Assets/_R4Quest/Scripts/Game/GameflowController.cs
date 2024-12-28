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
    [Inject] private AudioService _audioService;

    public void Start()
    {
        _audioService.PlayLoop2D("seashore.mp3").Forget();
        //await CheckPlayerPrefsSaved();
        
        ARSceneActions.OnARTrackedImage += OnARTrackedImage;

        GameActions.CallQuestStart += StartARQuest;
        GameActions.OnQuestStart += OnQuestStart;
        GameActions.OnQuestComplete += OnQuestComplete;

        if (container.ApplicationSettings.WaitConcreteNextQuest != string.Empty)
        {
            Debug.Log("set wait recognition image " + container.ApplicationSettings.WaitConcreteNextQuest);
            var q = container.ApplicationData.Quests
                .FirstOrDefault(x => x.RecognitionImage == container.ApplicationSettings.WaitConcreteNextQuest);
            ARSceneActions.OnWaitRecognitionImage?.Invoke(q);
        }
    }

    private async UniTask CheckPlayerPrefsSaved()
    {
        if (PlayerPrefs.HasKey("GoalsCounter"))
        {
            Debug.Log("saved " + PlayerPrefs.GetInt("GoalsCounter"));
        }
        await UniTask.CompletedTask;
    }

    public void OnARTrackedImage(string imgTrack, Transform position)
    {
        Debug.Log("imgTracked " + imgTrack);
        var q = container.ApplicationData.Quests
            .FirstOrDefault(x => x.RecognitionImage == imgTrack);

        UIActions.OnShowScenFX?.Invoke(true, 2);
        CreateAR(q, imgTrack, position).Forget();
    }

    async UniTask CreateAR(QuestData q, string imgTrack, Transform position)
    {
        await UniTask.Delay(2000);
        GameObject arTarget = gameObjectsFactory.CreateARTarget(q, imgTrack, position);
        
        FXManager.PlayFx(arTarget, new UnhideRendererEffect(), 3).Forget();
        await UniTask.Delay(2000);
        UIActions.OnQuestStart?.Invoke(q, imgTrack);

        Debug.Log("quest start sound " + q.Sound);
        //_audioService.PlayOneShot2D("queststart.mp3").Forget();
    }

    void StartARQuest()
    {
        Debug.Log("gameflowcontroller CallQuestStart with lua modificator");
        _luaScriptService.ExecuteAction("CallQuestStart.lua", "start", "str");
        ARSceneActions.OnARSession?.Invoke();
        ARSceneActions.OnReadyForTracking?.Invoke(true);
    }

    void OnQuestStart(string quest)
    {
        Debug.Log("luaService onqueststart " + quest);
        container.ApplicationData.CurrentQuest = quest;
        _luaScriptService.ExecuteAction("OnQuestStart", "Start", "null");
        
        PlayerPrefs.SetString("CurrentQuest", quest);

        AddQuestCounter();
    }

    private void AddQuestCounter()
    {
        if(PlayerPrefs.HasKey("GoalsCounter") && PlayerPrefs.GetInt("GoalsCounter") >= 0)
            PlayerPrefs.SetInt("GoalsCounter", PlayerPrefs.GetInt("GoalsCounter") + 1);
        else
            PlayerPrefs.SetInt("GoalsCounter", 0);
    }

    void OnQuestComplete(string questId, bool state)
    {
        Debug.Log("luaService onquestcomplete " + questId + " " + state);

        if (CheckLastQuest())
            StartEndSequence();

        var q = container.ApplicationData.Quests
            .FirstOrDefault(x => x.QuestID == questId);
        QuestData nextQuest = null;

        if (state)
        {
            nextQuest = container.ApplicationData.Quests.FirstOrDefault(x => x.QuestID == q.RightWayQuest);
            UIActions.OnQuestPanel.Invoke(q.RightReaction, nextQuest.RecognitionImage);
        }
        else
        {
            nextQuest = container.ApplicationData.Quests.FirstOrDefault(x => x.QuestID == q.WrongWayQuest);
            UIActions.OnQuestPanel.Invoke(q.WrongReaction, nextQuest.RecognitionImage);
        }

        container.ApplicationData.CurrentQuest = nextQuest.QuestID;
        container.ApplicationSettings.WaitConcreteNextQuest = nextQuest.RecognitionImage;
        _luaScriptService.ExecuteAction("OnQuestComplete", "Complete", "null");

        if (container.ApplicationSettings.WaitConcreteNextQuest != string.Empty)
        {
            ARSceneActions.OnWaitRecognitionImage?.Invoke(nextQuest);
            //UIActions.OnQuestPanel.Invoke("ПРОДОЛЖАЙТЕ\n\nИЩИТЕ ЗНАК КАК НА КАРТИНКЕ", nextQuest.RecognitionImage);
        }
    }

    private void StartEndSequence()
    {
        Debug.Log("Start EndSequence");
    }

    private bool CheckLastQuest()
    {
        Debug.Log("check last quest " + PlayerPrefs.GetInt("GoalsCounter") + " / " +
                  container.ApplicationData.Quests.Count);
        if (PlayerPrefs.GetInt("GoalsCounter") == container.ApplicationData.Quests.Count)
            return true;

        return false;
    }
}