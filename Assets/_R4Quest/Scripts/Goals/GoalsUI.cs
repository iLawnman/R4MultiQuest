using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GoalsUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GoalsUISkin goalsUISkin;
    [SerializeField] public List<GameObject> goalsUIs;
    [SerializeField] Sprite goalSuccessImgBack;
    [SerializeField] Sprite goalSuccessImages;
    [SerializeField] Color colorComplete;
    [SerializeField] Color colorActive;
    
    [SerializeField] internal int goalsCounter;
    [SerializeField] internal List<int> successIndx = new List<int>();
    [SerializeField] private bool _colorMarkedResult;
    [SerializeField] private AudioClip successFX;
    [Inject] private ConfigDataContainer container;
    private List<QuestData> activeGoals;

    private void Start()
    {
        activeGoals = container.ApplicationData.Quests
            .Where(x => !string.IsNullOrWhiteSpace(x.GoalIndex))
            .ToList();
        
        goalsUIs.ForEach(x => x.SetActive(false));
        for (int i = 0; i < activeGoals.Count; i++)
        {
            goalsUIs[i].SetActive(true);
            goalsUIs[i].GetComponent<Image>().sprite = goalSuccessImgBack;
        }

        UIActions.CallQuestStart += OnActive;
        UIActions.OnQuestComplete += CheckQuestComplete;
    }

    private void OnDestroy()
    {
        UIActions.CallQuestStart -= OnActive;
        UIActions.OnQuestComplete -= CheckQuestComplete;
    }

    [ContextMenu("check")]
    public void check()
    {
        CheckQuestComplete("Art1", true);
    }
    
    private void CheckQuestComplete(string questName, bool state)
    {
        var goalQuest = activeGoals.FirstOrDefault(x => x.QuestID == questName);
        if (goalQuest != null)
        {
            Debug.Log("set goal " + questName);  
        }
    }

    private void OnActive()
    {
        panel?.SetActive(true);
        //SetGoalsActive(0);
    }

    private void SetGoalState(int index, bool success)
    { 
        Debug.Log("set goal " + index + " - " + success);
        return;
        index = goalsCounter - 1;
        
        if (goalsUIs.Count > index)
        {
            goalsUIs[index].GetComponent<Image>().color = colorComplete;
            goalsUIs[index].GetComponent<Image>().sprite = goalSuccessImages;

            if (_colorMarkedResult)
            {
                if (success)
                    goalsUIs[index].GetComponent<Image>().color = colorComplete;
                else
                    goalsUIs[index].GetComponent<Image>().color = Color.red;
            }
        }
        GetComponent<AudioSource>()?.PlayOneShot(successFX);
    }

    public void SetGoalsActive(int index)
    {
        //index = goalsCounter - 1;
        Debug.Log("activate " + goalsCounter + " / " + index);
        if(index <= goalsUIs.Count)
            goalsUIs[index].GetComponent<Image>().color = colorActive;
        GetComponent<AudioSource>().PlayOneShot(successFX);
        
        //GetComponent<QuestsTimerController>()?.StartQuestTimer(goalsCounter);
    }

    public void SetGoalState(string gameObjectName, bool state)
    {
        Debug.Log("goal " + state + " / " + gameObjectName);
        SetGoalState(0, state);
        GetComponent<QuestsTimerController>()?.StopQuestTimer(goalsCounter);
        if (state)
            successIndx.Add(goalsCounter);

        //TODO : get QuestListLastName and check
        if (gameObjectName.Contains("15"))
        {
            FindFirstObjectByType<InfoPanelsController>().ShowOutroScreen();
            GetComponent<QuestsTimerController>()?.CreateFinalList();
            PlayerPrefs.DeleteAll();
        }
    }
}