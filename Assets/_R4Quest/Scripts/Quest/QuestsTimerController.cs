using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestsTimerController : MonoBehaviour
{
    public int[] GoalTime = new int[14];
    private TimeSpan startTime;
    [SerializeField] private TMP_Text finalTextTitle;
    [SerializeField] private TMP_Text finalText;
    [SerializeField] private Text timerText;
    [SerializeField] private bool active;
    private TimeSpan startTimeQuest = TimeSpan.Zero;
    [SerializeField] private GameObject FinalTimerPanel;

    public void StartQuestTimer(int goalIndx)
    {
        startTime = DateTime.Now.TimeOfDay;
        if (goalIndx == 1)
            startTimeQuest = startTime;            
        Debug.Log("start " + goalIndx + " at " + startTime);

        StartCoroutine(startTimer());
    }

    private IEnumerator startTimer()
    {
        active = true;
        int sec = 0;
        while (active)
        {
            yield return new WaitForSecondsRealtime(1);
            sec++;
            timerText.text = (sec / 60).ToString() + ":" + (sec % 60).ToString("D2");
            if ((sec % 60) == 0)
            {
                timerText.color = Color.red;
                yield return new WaitForSecondsRealtime(1);
                timerText.color = Color.white;
            }
        }
    }

    public void StopQuestTimer(int index)
    {
        if (GoalTime.Length > index)
        {
            Debug.Log("stop timer index " + index + " goals indx " + GoalTime[index]);

            GoalTime[index] = (int)(DateTime.Now.TimeOfDay - startTime).TotalSeconds;
            Debug.Log("end index goal " + index + " - " + GoalTime[index]);
        }

        active = false;
        timerText.text = String.Empty;
        
        StopAllCoroutines();
    }

    public void CreateFinalList()
    {
        string final = String.Empty;
            
        for (int i = 0; i < GoalTime.Length; i++)
        {
            var time = (GoalTime[i] / 60).ToString() + ":" + (GoalTime[i] % 60).ToString("D2");
                
            if(GetComponent<GoalsController>().goalsUIs[i].GetComponent<Image>().color == Color.red)
                final += "<color=red>Задача " + (i + 1) + " - " + " / " +  time + " сек</color>\n";
            else
                final += "Задача " + (i + 1)+ " - " + " / " +  time + " сек\n";
        }

        var sum = (GoalTime.Sum() / 60).ToString() + ":" + (GoalTime.Sum() % 60).ToString("D2");
        var t = (int)(DateTime.Now.TimeOfDay - startTimeQuest).TotalSeconds;
        final += $"\n<color=yellow> <b>ВСЕГО - {sum} сек</color></b>";
        finalText.text = final;
        timerText.text = String.Empty;
        finalTextTitle.text += "\n" + PlayerPrefs.GetString("PlayerName") + " / " + PlayerPrefs.GetString("PlayerTeam");
        FinalTimerPanel.SetActive(true);
    }
}
