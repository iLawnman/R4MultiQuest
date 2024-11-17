using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SceneflowController : MonoBehaviour
    {
        private InfoPanelsController _infoPanel;
        private GoalsUI _goalsConteroller;
        private MainCanvasController _mainCanvasController;
        private AskPanel askPanel;

        private void Start()
        {
            _infoPanel = FindFirstObjectByType<InfoPanelsController>();
            _goalsConteroller = FindFirstObjectByType<GoalsUI>();
            _mainCanvasController = FindFirstObjectByType<MainCanvasController>();
            
            if (!PlayerPrefs.HasKey("SaveQuest"))
                ClearStart();
            else
                LoadSavedStart();
        }

        ///TMP
        [ContextMenu("SetEnd")]
        public void SetEnd()
        {
            PlayerPrefs.SetString("SaveQuest", "Test");
            PlayerPrefs.SetString("GoalsCounter", "13");
        }
        
        [ContextMenu("Clear PlayerPrefs")]
        public void Clear()
        {
            PlayerPrefs.DeleteAll();
        }

        public void ClearStart()
        {
            PlayerPrefs.DeleteAll();
            //_infoPanel.ShowStartSequence();
        }

        private void OnDestroy()
        {
            // askPanel = _mainCanvasController.ShowLoadAskPanel();
            // askPanel.Yes.onClick.RemoveAllListeners();
            // askPanel.No.onClick.RemoveAllListeners();
        }

        private void LoadSavedStart()
        {
            _infoPanel.panelPrefab.SetActive(false);
            // askPanel = _mainCanvasController.ShowLoadAskPanel();
            // askPanel.Yes.onClick.AddListener(LoadData);
            // askPanel.No.onClick.AddListener(ClearStart);
        }

        public void StartARSession() => FindFirstObjectByType<ARSession>(FindObjectsInactive.Include).gameObject.SetActive(true);
        
        public void FinishSequence () => _infoPanel.ShowOutroScreen();
        
        public void SaveCurrentStep (string savedQuestName)
        {
            PlayerPrefs.SetString("SaveQuest", savedQuestName);
            PlayerPrefs.SetInt("QuestLetTimer", 0);
            PlayerPrefs.SetString("GoalsCounter", (_goalsConteroller.goalsCounter + 1).ToString());
            
            Debug.Log("saved " + PlayerPrefs.GetString("SaveQuest") + " / left time " + PlayerPrefs.GetInt("QuestLetTimer")
                      + " counter " + PlayerPrefs.GetString("GoalsCounter"));
        }

        void LoadData() {
            
            Debug.Log("loaded " + PlayerPrefs.GetString("SaveQuest") + " / left time " + PlayerPrefs.GetInt("QuestLetTimer")
                      + " counter " + PlayerPrefs.GetString("GoalsCounter"));

            _mainCanvasController.SplashPanelSetActive(false);

            var _savedQuest = PlayerPrefs.GetString("SaveQuest");
            var _savedQuestLeftTimer = PlayerPrefs.GetInt("QuestLetTimer");

            //TODO : if Quests with current next way quest mechanics make current target
            
            if (PlayerPrefs.HasKey("GoalsCounter"))
            {
                //_goalsConteroller.SetCurrentState(int.Parse(PlayerPrefs.GetString("GoalsCounter")), new List<int>());
            }
        }
    }
