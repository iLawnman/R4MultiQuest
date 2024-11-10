using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreensUI : MonoBehaviour, IUISkin
{
    [SerializeField] private IntroUISkin skin;
    [SerializeField] private GameObject InfoPanel;
    public Image infoBack;
    public Image image;
    [SerializeField] private Text text;
    [SerializeField] private Text taptext;
    [SerializeField] private Button closeButtom;
    
    [SerializeField] private GameObject lastPanel;
    public Image lastBack;
    public Image logo;
    [SerializeField] private TMP_Text credits;
    [SerializeField] private Button linkButtom;
    [SerializeField] private Button closeLastButtom;

    [SerializeField] private GameObject finalTimerPanel;
    public Image finalBack;
    [SerializeField] private TMP_Text finalTitle;
    [SerializeField] private TMP_Text finalTxt;
    [SerializeField] private Button finalCloseButtom;

    private void Start()
    {
        SetSkin(skin);
    }

    public void SetSkin(UISkin uiSkin)
    {
        skin = uiSkin as IntroUISkin;
        text.font = skin.text;
        taptext.font = skin.taptext;
        credits.font = skin.credits;
        finalTitle.font = skin.finalTitle;
        finalTxt.font = skin.finalTxt;
        infoBack.sprite = skin.infoBack;
        lastBack.sprite = skin.lastBack;
        finalBack.sprite = skin.finalBack;
        logo.sprite = skin.logo;
        closeButtom.image.sprite = skin.closeButtom;
        closeLastButtom.image.sprite = skin.closeLastButtom;
        finalCloseButtom.image.sprite = skin.finalCloseButtom;
        linkButtom.image.sprite = skin.linkButtom;
        image.sprite = skin.image;
    }
}
