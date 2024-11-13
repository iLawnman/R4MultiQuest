using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private GoalsUI _goalsUI;

    private void Start()
    {
        _goalsUI = FindFirstObjectByType<GoalsUI>(FindObjectsInactive.Include);
    }

    public void InitAnswers()
    {
        GetComponent<QuestBasePrefabView>().FillAnswer(answers[0]);
        currentAnswer = 0;
        CheckQuestionAnswer();
    }

    public void ShowNextAnswer()
    {
        if (currentAnswer + 1 < answers.Count)
            currentAnswer++;
        else
            currentAnswer = 0;

        GetComponent<QuestBasePrefabView>().FillAnswer(answers[currentAnswer]);
        CheckQuestionAnswer();
    }

    public void ShowPreviewsAnswer()
    {
        if (currentAnswer == 0)
            currentAnswer = answers.Count - 1;
        else
            currentAnswer--;

        GetComponent<QuestBasePrefabView>().FillAnswer(answers[currentAnswer]);
        CheckQuestionAnswer();
    }

    private void CheckQuestionAnswer()
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

    private void NextStepWrong()
    {
        Debug.Log("next Step Wrong");
        _goalsUI.SetGoalState(gameObject.name, false);
        var currentArtefact = GetComponent<iQuestTask>();
        GameActions.OnShowStartQuestPanel.Invoke(currentArtefact.WrongReactionSign, currentArtefact.WrongReaction);
        GameActions.SendCurrentStep.Invoke("Wrong quest " + currentArtefact.name);

        Destroy(gameObject, 3);
    }

    void NextStepRight()
    {
        Debug.Log("next Step Right");
        _goalsUI.SetGoalState(gameObject.name, true);
        var currentArtefact = GetComponent<iQuestTask>();
        GameActions.OnShowStartQuestPanel.Invoke(currentArtefact.RightReactionSign, currentArtefact.RightReaction);

        GameActions.SendCurrentStep.Invoke("Right quest " + currentArtefact.name);
        GameActions.SetReadyForTracking.Invoke(true, currentArtefact.RightReactionSign);
        Destroy(gameObject, 3);
    }

    public void NextStep()
    {
        Debug.Log("next Step");
        _goalsUI.SetGoalState(gameObject.name, true);
        var currentArtefact = GetComponent<iQuestArtefact>();

        if (!currentArtefact.noArtefact)
        {
            GameActions.OnShowStartQuestPanel.Invoke(currentArtefact.ReactionSign, currentArtefact.Question);
            Destroy(gameObject, 3f);
        }
        else
        {
            Debug.Log("no artefact");
            GameActions.OnShowStartQuestPanel.Invoke(currentArtefact.ReactionSign, currentArtefact.Reaction);

            Destroy(gameObject, 3f);
        }
    }
}