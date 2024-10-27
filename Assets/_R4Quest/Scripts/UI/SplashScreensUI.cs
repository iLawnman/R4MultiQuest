using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreensUI : MonoBehaviour
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

}
