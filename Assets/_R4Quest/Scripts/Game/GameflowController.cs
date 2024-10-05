using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public static partial class GameActions
{
    public static Action OnGameStarting;
    public static Action OnGameStarted;
    public static Action OnGamePaused;
    public static Action OnGameResumed;
    public static Action OnGameEnded;
    public static Action<bool, string> SetReadyForTracking;
}

public class GameflowController : MonoBehaviour
    {
        private InfoPanelsController _infoPanel;
        private GoalsController _goalsConteroller;
        private MainCanvasController _mainCanvasController;
        private AskPanel askPanel;
        private iQuest currentQuest;
        private iQuest previewsQuest;
        private QuestsTimerController _questTimeController;

        private void Start()
        {
            Application.runInBackground = true;
            Application.targetFrameRate = 30;
            _infoPanel = FindFirstObjectByType<InfoPanelsController>();
            _goalsConteroller = FindFirstObjectByType<GoalsController>();
            _mainCanvasController = FindFirstObjectByType<MainCanvasController>();
            _questTimeController = FindFirstObjectByType<QuestsTimerController>();
            
            if (!PlayerPrefs.HasKey("SaveQuest"))
                ClearStart();
            else
                LoadSavedStart();
        }

        public void ApplicationQuit()
        {
            Application.Quit();
        }

        public void StartARSession()
        {
            FindFirstObjectByType<ARSession>(FindObjectsInactive.Include).gameObject.SetActive(true);
        }
        private void SetCurrentQuest(iQuest quest)
        {
            if(currentQuest != null)
                previewsQuest = currentQuest;
            currentQuest = quest;
        }

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
            GetComponent<InfoPanelsController>().ShowStartSequence();
            FindFirstObjectByType<TeamPresenter>()?.SetPanelActive(true);
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
