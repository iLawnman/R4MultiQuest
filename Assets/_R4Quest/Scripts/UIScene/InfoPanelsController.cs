using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

[Serializable]
public class IntroScreen
{
    public string Title;
    [Multiline] public string text;
    public Sprite image;
    public Sprite background;
    public string nextScreenName;
    public GameObject additionalUIpref;
}

public class InfoPanelsController : MonoBehaviour
{
    [Inject] private ConfigDataContainer dataContainer;

    [Header("Game UI Elements")] public GameObject panelPrefab;

    public GameObject panel;
    public List<IntroScreen> introScreens;
    public List<IntroScreen> outroScreens;
    public GameObject LastPanel;
    public int currentScreenInx;
    public string nextScreen;
    private float fromPositionOld;
    private float toPositionOld;
    private float fromPositionNew;
    private float toPositionNew;
    private bool slideInprogress;
    [SerializeField] SlideController slideController;

    private void Start()
    {
        //Debug.Log("start infopanels controller with container " + dataContainer.ApplicationSettings.AddressableKey);
        if(dataContainer.ApplicationSettings)
            LoadFillIntroOutro();
        CreateScreens();
        //fill flow settings
    }

    private void CreateScreens()
    {
        introScreens.ForEach(x => { CreatePanel(x.Title, out var panel); });
    }

    public void PageScrollerControl(Int32 page)
    {
        if (page == introScreens.Count - 1)
        {
            var lastPage = panel.transform.GetChild(introScreens.Count - 1);
            lastPage.transform.Find("TapText").GetComponent<Text>().text = ">                      ЗАКРЫТЬ                       <";
            lastPage.GetComponentInChildren<Button>().onClick.AddListener(CloseInfoPanel);
        }
        else
        {
            var lastPage = panel.transform.GetChild(page);
            lastPage.transform.Find("TapText").GetComponent<Text>().text = "<                      ЛИСТАЙТЕ                       >";
            lastPage.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        }
    }

    private void CloseInfoPanel()
    {
        foreach (Transform chi in panel.transform)
        {
            Destroy(chi.gameObject);
        }
        GameActions.CallQuestStart?.Invoke();
    }

    private void LoadFillIntroOutro()
    {
        dataContainer.ApplicationData.IntroScreens.ForEach(x =>
        {
            IntroScreen scr = new IntroScreen()
            {
                Title = x.Title,
                text = x.Text,
                nextScreenName = x.NextScreenName,
                background = CacheService.GetCachedImage(x.Background + ".png"),
                image = CacheService.GetCachedImage(x.Image + ".png"),
            };
            introScreens.Add(scr);
        });

        dataContainer.ApplicationData.OutroScreens.ForEach(x =>
        {
            IntroScreen scr = new IntroScreen()
            {
                Title = x.Title,
                text = x.Text,
                nextScreenName = x.NextScreenName,
                background = CacheService.GetCachedImage(x.Background + ".png"),
                image = CacheService.GetCachedImage(x.Image + ".png"),
            };
            outroScreens.Add(scr);
        });
    }

    private void CreatePanel(string name, out GameObject newPanel)
    {
        panelPrefab.SetActive(true);
        newPanel = Instantiate(panelPrefab, panel.transform);
        panelPrefab.SetActive(false);

        newPanel.name = "panel_" + name;
        var newScreen = introScreens.Where(screen => screen.Title == name).ToList();
        currentScreenInx = introScreens.FindIndex(screen => screen.Title == name);

        if (newScreen[0].background)
            newPanel.GetComponentInChildren<Image>().sprite = newScreen[0].background;

        if (newScreen[0].image)
            newPanel.transform.Find("Image").GetComponent<Image>().sprite =
                newScreen[0].image;

        newPanel.transform.Find("Text").GetComponent<Text>().text = newScreen[0].text;
        nextScreen = newScreen[0].nextScreenName;

        if (newScreen[0].additionalUIpref)
        {
            if (newPanel.transform.Find("addUI") != null)
                Destroy(newPanel.transform.Find("addUI")?.gameObject);

            var addUI = Instantiate(newScreen[0].additionalUIpref, newPanel.transform);
            addUI.name = "addUI";
        }
    }

    public void ShowOutroScreen()
    {
        if (outroScreens.Count > 0)
        {
            introScreens = outroScreens;
            panelPrefab.transform.parent.gameObject.SetActive(true);
            nextScreen = "End Quest";
        }
        else
        {
            panelPrefab.transform.parent.gameObject.SetActive(false);
            LastPanel.SetActive(true);
        }
    }
}