using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using VContainer;

public class QuestUI : MonoBehaviour, IUISkin
{
    [SerializeField] private QuestUISkin skin;
    [SerializeField] private GameObject questStartPanel;
    [SerializeField] private GameObject answerControlPanel;
    [SerializeField] private GameObject continueButton;
    [Inject] private ConfigDataContainer container;
    [Inject] private GameObjectsFactory gameObjectsFactory;

    void Start()
    {
        GameActions.CallQuestStart += OnActive;
        ARSceneActions.OnARTrackedImageAdded += OnARTrackedImageAdded;
        SetSkin(skin);
    }

    private void OnActive()
    {
        questStartPanel?.SetActive(true);
    }

    void OnDestroy()
    {
        ARSceneActions.OnARTrackedImageAdded -= OnARTrackedImageAdded;
        GameActions.CallQuestStart -= OnActive;

    }

    private void OnARTrackedImageAdded(ARTrackedImage trackeImg)
    {
        Debug.Log("tracked " + trackeImg.referenceImage.name);

        var quest = container.ApplicationData.Quests
            .FirstOrDefault(x => x.RecognitionImage == trackeImg.referenceImage.name);
        
        ShowQuestByRecognitionImage(quest, trackeImg);
    }

    private void ShowQuestByRecognitionImage(QuestData quest, ARTrackedImage trackeImg)
    {
        questStartPanel.SetActive(true);
        questStartPanel.transform.Find("Text").GetComponent<Text>().text = quest.Question;
        
        if(CacheService.GetCachedImage(quest.RecognitionImage + ".png"))
            questStartPanel.transform.Find("Image").GetComponent<Image>().sprite = 
                CacheService.GetCachedImage(quest.RecognitionImage + ".png");

        gameObjectsFactory.CreateARTarget(quest, trackeImg);
    }

    public void SetSkin(UISkin uiSkin)
    {
        //skin = uiSkin as QuestUISkin;
        //questStartPanel.GetComponent<Image>().sprite = skin.questStartBack.LoadAssetAsync<Sprite>().WaitForCompletion();
        //if(skin.questStartBack.)
        //    questStartPanel.GetComponent<Image>().sprite = CacheService.GetCachedImage(skin.questStartBack.name);
        //continueButton.GetComponent<Image>().sprite = skin.continueButton;
    }
}