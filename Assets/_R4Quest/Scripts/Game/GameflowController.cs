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
        GameActions.OnShowStartQuestPanel += OnShowStartQuestPanel;
        GameActions.OnQuestComplete += OnQuestComplete;
        
        ARSceneActions.OnARTrackedImageAdded += OnARTrackedImageAdded;
    }

    void OnARTrackedImageAdded(ARTrackedImage imgTrack)
    {
        Debug.Log("imgTracked " + imgTrack.referenceImage);
        
        //TO-DO lua script-modificator
        
        var q = container.ApplicationData.Quests
            .FirstOrDefault(x => x.RecognitionImage == imgTrack.referenceImage.name);
        gameObjectsFactory.CreateARTarget(q, imgTrack);
        
        UIActions.OnQuestStart?.Invoke(q, imgTrack);    
    }

    void OnShowStartQuestPanel(string quest, string state)
    {
        Debug.Log("gamecontroller OnLoadFinish");
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
        _luaScriptService.ExecuteAction("OnQuestStart", "Start", "null");
    }

    void OnQuestComplete(string questId, bool state)
    {
        Debug.Log("luaService onquestcomplete " + questId + " " + state);
    }
}
/*
    _infoPanel = FindFirstObjectByType<InfoPanelsController>();
    _goalsConteroller = FindFirstObjectByType<GoalsController>();
    _mainCanvasController = FindFirstObjectByType<MainCanvasController>();
    _questTimeController = FindFirstObjectByType<QuestsTimerController>();
    if (!PlayerPrefs.HasKey("SaveQuest"))
        ClearStart();
    else
        LoadSavedStart();
public void ApplicationQuit()
{
    Application.Quit();
}

// private void SetCurrentQuest(iQuest quest)
// {
//     if(currentQuest != null)
//         previewsQuest = currentQuest;
//     currentQuest = quest;
// }

private void OnDestroy()
{
    askPanel?.Yes.onClick.RemoveAllListeners();
    askPanel?.No.onClick.RemoveAllListeners();
}

private void LoadSavedStart()
{
    _infoPanel.startPanel.SetActive(false);
    // askPanel = _mainCanvasController.ShowLoadAskPanel();
    // askPanel.Yes.onClick.AddListener(LoadData);
    // askPanel.No.onClick.AddListener(ClearStart);
}

[ContextMenu("Clear Start")]
public void ClearStart()
{
    PlayerPrefs.DeleteAll();
    // GetComponent<InfoPanelsController>()?.ShowStartSequence();
    // FindFirstObjectByType<TeamPresenter>()?.SetPanelActive(true);
}

public void SaveCurrentStep (string savedQuestName)
{
    PlayerPrefs.SetString("SaveQuest", savedQuestName);
    PlayerPrefs.SetInt("QuestLetTimer", 0);
    PlayerPrefs.SetString("GoalsCounter", _goalsConteroller.goalsCounter.ToString());
    var lst = string.Join(",", _goalsConteroller.successIndx);
    PlayerPrefs.SetString("SuccessList", lst);
    // var recognized = string.Join(",", FindFirstObjectByType<FlowManager>().RecognitionImgList);
    // PlayerPrefs.SetString("Recogntized", recognized);

    if (_questTimeController)
    {
        var lstTime = string.Join(", ", _questTimeController.GoalTime);
        PlayerPrefs.SetString("TimeList", lstTime);
    }

    Debug.Log("saved " + PlayerPrefs.GetString("SaveQuest")
                       + " / left time " + PlayerPrefs.GetInt("QuestLetTimer")
              + " counter " + PlayerPrefs.GetString("GoalsCounter")
              + " SuccessList " + PlayerPrefs.GetString("SuccessList")
                       + " recogn List " + PlayerPrefs.GetString("Recogntized"));
}

public void StartFromSave()
{
    DeleteTargets();
    GetComponent<InfoPanelsController>().startPanel.SetActive(false);

    StartCoroutine("LoadData");
}

private void DeleteTargets()
{
    var targets = FindObjectsOfType<iQuest>().ToList();
    if(targets.Count > 0)
        targets.ForEach(x => Destroy(x.gameObject));
}

void LoadData()
{
    Debug.Log("loaded " + PlayerPrefs.GetString("SaveQuest") + " / left time " +
              PlayerPrefs.GetInt("QuestLetTimer")
              + " counter " + PlayerPrefs.GetString("GoalsCounter")
              + " player " + PlayerPrefs.GetString("PlayerName")
              + " - " + PlayerPrefs.GetString("PlayerTeam"));

    FindFirstObjectByType<TeamPresenter>().LoadData();

    // PlayerPrefs.GetString("Recogntized")?.Split(",").ToList().ForEach(x =>
    //     FindFirstObjectByType<neSkazkaFlowManager>().RecognitionImgList.Add(x));
    _mainCanvasController.SplashPanelSetActive(false);

    if (PlayerPrefs.HasKey("GoalsCounter"))
    {
        var x = PlayerPrefs.GetString("SuccessList");
        var successIndx = x.Split(",");
        List<int> sucIndx = new List<int>();
        successIndx?.ToList().ForEach(x => sucIndx.Add(int.Parse(x)));
        _goalsConteroller.SetCurrentState(int.Parse(PlayerPrefs.GetString("GoalsCounter")), sucIndx);
    }

    if(!PlayerPrefs.HasKey("TimeList"))
        return;

    var lstTime = PlayerPrefs.GetString("TimeList").Split(",");
    List<int> goalTime = new List<int>();
    lstTime.ToList().ForEach(x => goalTime.Add(int.Parse(x)));
    FindFirstObjectByType<QuestsTimerController>(FindObjectsInactive.Include).GoalTime = goalTime.ToArray();
}
}
*/