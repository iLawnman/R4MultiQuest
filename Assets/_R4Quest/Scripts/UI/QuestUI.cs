using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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

    void Start()
    {
        UIActions.OnQuestStart += OnActive;
        SetSkin(skin);
    }

    private void OnActive(QuestData quest, ARTrackedImage trackeImg)
    {
        Debug.Log("onActive " + quest.QuestID + " with img " + trackeImg.referenceImage);
        
        ShowQuestByRecognitionImage(quest, trackeImg);
    }

    void OnDestroy()
    {
        UIActions.OnQuestStart += OnActive;
    }

    private async UniTask ShowQuestByRecognitionImage(QuestData quest, ARTrackedImage trackeImg)
    {
        await UniTask.Delay(2000);
        
        questStartPanel.SetActive(true);
        questStartPanel.transform.Find("Text").GetComponent<Text>().text = quest.Question;
        
        if(CacheService.GetCachedImage(quest.RecognitionImage + ".png"))
            questStartPanel.transform.Find("Image").GetComponent<Image>().sprite = 
                CacheService.GetCachedImage(quest.RecognitionImage + ".png");
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