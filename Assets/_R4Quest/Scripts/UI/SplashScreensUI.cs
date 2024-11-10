using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
        SetSkin(skin);
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
