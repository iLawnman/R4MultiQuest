using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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

    private async void Start()
    {
        await LoadSetCachedResources();

        GameActions.OnQuestStarting += OnActive;
    }

    private void OnActive()
    {
        panel.SetActive(true);
        SetImgsBack();
    }

    private async UniTask LoadSetCachedResources()
    {
        // load goal skin async
    }

    private void SetImgsBack()
    {
        if (goalSuccessImgBack)
            goalsUIs.ForEach(x => x.GetComponent<Image>().sprite = goalSuccessImgBack);
    }

    private void GoalSuccess(int index, bool success)
    { 
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
        goalsCounter++;
        index = goalsCounter - 1;
        Debug.Log("activate " + goalsCounter + " / " + index);
        
        if(index <= goalsUIs.Count)
            goalsUIs[index].GetComponent<Image>().color = colorActive;
        GetComponent<AudioSource>().PlayOneShot(successFX);

        GetComponent<QuestsTimerController>()?.StartQuestTimer(goalsCounter);
    }

    public void SetCurrentState(int counter, List<int> succesIndx)
    {
        goalsCounter = counter;
        successIndx = succesIndx;
        
        Debug.Log("Set goals state " + goalsCounter);
        for (int i = 0; i < counter; i++)
        {
            if(_colorMarkedResult && !successIndx.Contains(i))
                goalsUIs[i].GetComponent<Image>().color = Color.red;
            else 
                goalsUIs[i].GetComponent<Image>().color = colorComplete;

            goalsUIs[i].GetComponent<Image>().sprite = goalSuccessImages;
        }
    }

    public void GoalSuccess(string gameObjectName, bool state)
    {
        Debug.Log("goal " + state + " / " + gameObjectName);
        GoalSuccess(0, state);
        GetComponent<QuestsTimerController>()?.StopQuestTimer(goalsCounter);
        if (state)
            successIndx.Add(goalsCounter);

        //FindFirstObjectByType<GameflowController>(FindObjectsInactive.Include).SaveCurrentStep(gameObjectName);

        //if (goalsCounter == 14)
        //TODO : get QuestListLastName and check
        if (gameObjectName.Contains("15"))
        {
            FindFirstObjectByType<InfoPanelsController>().ShowOutroScreen();
            GetComponent<QuestsTimerController>()?.CreateFinalList();
            PlayerPrefs.DeleteAll();
        }
    }
}