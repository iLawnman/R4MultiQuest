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
    private Queue panelsQueue;

    void Start()
    {
        panelsQueue = new Queue();
        UIActions.OnQuestStart += OnActive;
        UIActions.OnQuestPanel += OnQuestPanel;
        UIActions.OnQuestPanelCurrent += OnQuestPanelCurrent;

        SetSkin(skin);
    }

    private void OnActive(QuestData quest, string trackeImg)
    {
        Debug.Log("onActive " + quest.QuestID + " with img " + trackeImg);
        
        ShowQuestByRecognitionImageDelayed(quest);
    }

    void OnQuestPanel(string txt, string imgName)
    {
        UniTask.Run(() =>onQuestPanel(txt, imgName));
    }
    
    async UniTask onQuestPanel(string txt, string imgName)
    {
        await UniTask.WaitUntil(() => !questStartPanel.activeSelf);
        questStartPanel.transform.Find("Text").GetComponent<Text>().text = txt;
        
        if(CacheService.GetCachedImage(imgName + ".png"))
            questStartPanel.transform.Find("Image").GetComponent<Image>().sprite = 
                CacheService.GetCachedImage(imgName + ".png");
        questStartPanel.SetActive(true);
    }
    
    void OnQuestPanelCurrent()
    {
        string currentQuest = container.ApplicationData.CurrentQuest;
        string txt = "ПРОДОЛЖАЙТЕ ПРОГУЛКУ\nИЩИТЕ ЗНАК КАК НА КАРТИНКЕ";//container.ApplicationData.Quests.FirstOrDefault(x => x.QuestID == currentQuest).Question;
        string imgName = container.ApplicationData.Quests.FirstOrDefault(x => x.QuestID == currentQuest).RecognitionImage;
        
        OnQuestPanel(txt, imgName);
    }
    
    void OnDestroy()
    {
        UIActions.OnQuestStart += OnActive;
        UIActions.OnQuestPanel -= OnQuestPanel;
        UIActions.OnQuestPanelCurrent -= OnQuestPanelCurrent;
    }

    private async UniTask ShowQuestByRecognitionImageDelayed(QuestData quest)
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
        return;
        
        skin = uiSkin as QuestUISkin;
        if(skin.questStartBack)
            questStartPanel.GetComponent<Image>().sprite = CacheService.GetCachedImage(skin.questStartBack.name);
        continueButton.GetComponent<Image>().sprite = skin.continueButton;
    }
}