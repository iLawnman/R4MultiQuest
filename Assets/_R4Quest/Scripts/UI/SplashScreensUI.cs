using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SplashScreensUI : MonoBehaviour, IUISkin
{
    [SerializeField] private SplashUISkin skin;

    [SerializeField] private GameObject alertPanel;
    [SerializeField] private Image alertPanelBack;
    [SerializeField] private Image logo;
    [SerializeField] private Text text;
    [SerializeField] private Button okButton;
    [SerializeField] private Button offerButton;

    [SerializeField] private GameObject splashPanel;
    [SerializeField] private Image splashPanelBack;
    [SerializeField] private Image customlogo;
    [SerializeField] private Image r4logo;
    [SerializeField] private TMP_Text name1text;
    [SerializeField] private TMP_Text name2text;

    [SerializeField] private GameObject askPanel;
    [SerializeField] private Image askPanelBack;
    [SerializeField] private Text askText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [Inject] private ConfigDataContainer dataContainer;

    private void Awake()
    {
        FillPanelFromGoogleSheetCache();
        //SetSkin(skin);
    }

    private void FillPanelFromGoogleSheetCache()
    {
        dataContainer.ApplicationData.SplashScreens.ForEach(x =>
        {
            if (CacheService.GetCachedImage(x.AlertPanelBack))
                alertPanelBack.sprite = CacheService.GetCachedImage(x.AlertPanelBack);
            
            if (CacheService.GetCachedImage(x.Logo))
                logo.sprite = CacheService.GetCachedImage(x.Logo);
            
            if(!string.IsNullOrWhiteSpace(x.Text))
                text.text = x.Text;

            if (CacheService.GetCachedImage(x.okButton))
                okButton.GetComponent<Image>().sprite = CacheService.GetCachedImage(x.okButton);

            if (CacheService.GetCachedImage(x.OfferButton))
                offerButton.GetComponent<Image>().sprite = CacheService.GetCachedImage(x.OfferButton);

            if (CacheService.GetCachedImage(x.SplashPanelBack))
                splashPanelBack.sprite = CacheService.GetCachedImage(x.SplashPanelBack);

            if (CacheService.GetCachedImage(x.CustomLogo))
                customlogo.sprite = CacheService.GetCachedImage(x.CustomLogo);
            
            if (CacheService.GetCachedImage(x.R4QLogo))
                r4logo.sprite = CacheService.GetCachedImage(x.R4QLogo);
            
            if(!string.IsNullOrWhiteSpace(x.Name1))
                name1text.text = x.Name1;
            if(!string.IsNullOrWhiteSpace(x.Name2))
                name2text.text = x.Name2;
            
            if (CacheService.GetCachedImage(x.AskPanelBack))
                askPanelBack.sprite = CacheService.GetCachedImage(x.AskPanelBack);
            if(!string.IsNullOrWhiteSpace(x.AskText))
                askText.text = x.AskText;
            
            if (CacheService.GetCachedImage(x.YesButton))
                yesButton.GetComponent<Image>().sprite = CacheService.GetCachedImage(x.YesButton);
            
            if (CacheService.GetCachedImage(x.NoButton))
                noButton.GetComponent<Image>().sprite = CacheService.GetCachedImage(x.NoButton);
        });
    }

    public void SetSkin(UISkin uiSkin)
    {
        alertPanelBack.sprite = skin.logoback;
        logo.sprite = skin.logo;
        //text.text = skin.text;
        okButton.GetComponent<Image>().sprite = skin.okButton;
        offerButton.GetComponent<Image>().sprite = skin.offerButton;

        splashPanelBack.sprite = skin.splashBack;
        customlogo.sprite = skin.customlogo;
        r4logo.sprite = skin.r4logo;
        //name1text.text = skin.name1text;
        //name2text.text = skin.name2text;

        askPanelBack.sprite = skin.askBack;
        //askText.text = skin.askText;
        yesButton.GetComponent<Image>().sprite = skin.yesButton;
        noButton.GetComponent<Image>().sprite = skin.noButton;
    }
}