using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AskPanel : MonoBehaviour
{
    [SerializeField] private Text _text;
    public Button Yes;
    public Button No;

    void SetText(string text, string yesTxt, string noTxt)
    {
        _text.text = text;
        Yes.GetComponentInChildren<Text>().text = yesTxt;
        No.GetComponentInChildren<Text>().text = noTxt;
    }

    public int Show(string maintext, string yes, string no)
    {
        SetText(maintext, yes, no);
        //int result = StartCoroutine(WaitAnswer().CurrentCoroutine);
        return 0;
    }

    private IEnumerator WaitAnswer()
    {
        yield return new WaitUntil(() => Yes.interactable);
        yield return 0;
    }
}
