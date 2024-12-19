using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class AnswerData
{
    public BasePrefabTexts txts;
    public BasePrefabImages imgs;
    public BasePrefabPrefabs prefabs;
    public BasePrefabDecor decor;
}

public class AnswersController : MonoBehaviour
{
    public List<AnswerData> answers = new List<AnswerData>();
    [SerializeField] private int currentAnswer;

    public void InitAnswers()
    {
        GetComponent<QuestBasePrefabView>().FillAnswer(answers[0]);
        currentAnswer = 0;
        CheckAnswerOKbutton();
    }

    public void ShowNextAnswer()
    {
        if (currentAnswer + 1 < answers.Count)
            currentAnswer++;
        else
            currentAnswer = 0;

        GetComponent<QuestBasePrefabView>().FillAnswer(answers[currentAnswer]);
        CheckAnswerOKbutton();
    }

    public void ShowPreviewsAnswer()
    {
        if (currentAnswer == 0)
            currentAnswer = answers.Count - 1;
        else
            currentAnswer--;

        GetComponent<QuestBasePrefabView>().FillAnswer(answers[currentAnswer]);
        CheckAnswerOKbutton();
    }

    private void CheckAnswerOKbutton()
    {
        if (currentAnswer == 0)
            GetComponent<QuestBasePrefabView>().SetOkButton(false);
        else
            GetComponent<QuestBasePrefabView>().SetOkButton(true);
    }

    public void CheckAnswer()
    {
        Debug.Log("check answer " + currentAnswer + " / " + GetComponent<iQuestTask>().rightAnswerIndex);
        if (GetComponent<iQuestTask>().rightAnswerIndex.Contains((currentAnswer).ToString()))
            NextStepRight();
        else
            NextStepWrong();
    }

    public void CheckAnswer(int index)
    {
        Debug.Log("check answer button index" + index + " / " + GetComponent<iQuestTask>().rightAnswerIndex);

        if (GetComponent<iQuestTask>().rightAnswerIndex.Contains(index.ToString()))
            NextStepRight();
        else
            NextStepWrong();
    }

    public void CheckAnswer(string index)
    {
        for (int ans = 1; ans < answers.Count; ans++)
        {
            GetComponent<iQuestTask>().rightAnswerIndex += ", " + answers[ans].txts._mainTxt;
        }

        Debug.Log("check answer " + index + " / " + GetComponent<iQuestTask>().rightAnswerIndex);

        if (GetComponent<iQuestTask>().rightAnswerIndex.Contains(index))
            NextStepRight();
        else
            NextStepWrong();
    }

    async UniTask NextStepWrong()
    {
        Debug.Log("next Step Wrong");
        var currentArtefact = GetComponent<iQuestTask>();

        FXManager.PlayFx(gameObject, new HideRendererEffect(), 3).Forget();
        await UniTask.Delay(3000);

        GameActions.OnQuestComplete.Invoke(currentArtefact.name, false);
        Destroy(gameObject, 3);
    }

    async UniTask NextStepRight()
    {
        Debug.Log("next Step Right");
        var currentArtefact = GetComponent<iQuestTask>();

        FXManager.PlayFx(gameObject, new HideRendererEffect(), 3).Forget();
        await UniTask.Delay(3000);
        GameActions.OnQuestComplete.Invoke(currentArtefact.name, true);
        Destroy(gameObject, 3);
    }

    public void NextStep()
    {
        Debug.Log("next Step");
        var currentArtefact = GetComponent<iQuestArtefact>();
        OnCompleteAsync(currentArtefact);
    }

    async UniTask OnCompleteAsync(iQuestArtefact currentArtefact)
    {
        FXManager.PlayFx(gameObject, new HideRendererEffect(), 3).Forget();
        await UniTask.Delay(3000);
        GameActions.OnQuestComplete.Invoke(currentArtefact.name, true);
        Destroy(gameObject, 3f);
    }
}