using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class QuestUI : MonoBehaviour, IUISkin
{
    [SerializeField] private QuestUISkin skin;
    [SerializeField] private GameObject questStartPanel;
    [SerializeField] private GameObject answerControlPanel;
    [SerializeField] private GameObject continueButton;
    void Start()
    {
        ARSceneActions.OnARTrackedImageAdded += OnARTrackedImageAdded;
        SetSkin(skin);
    }
    void OnDestroy()
    {
        ARSceneActions.OnARTrackedImageAdded -= OnARTrackedImageAdded;
    }

    private void OnARTrackedImageAdded(ARTrackedImage obj)
    {
        Debug.Log("tracked " + obj.referenceImage.name);
    }

    public void SetSkin(UISkin introSkin)
    {
        skin = introSkin as QuestUISkin;
        questStartPanel.GetComponent<Image>().sprite = skin.questStartBack;
        continueButton.GetComponent<Image>().sprite = skin.continueButton;
    }
}