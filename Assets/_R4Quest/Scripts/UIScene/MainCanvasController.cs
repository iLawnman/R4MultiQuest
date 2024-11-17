using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject splashPanel;
    [SerializeField] private GameObject introScreenPanel;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject overlayPanel;
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
    
    private void ShowStartQuestPanel(string signImag, string findTxt)
    {
       
    }

    public void ShowLoadAskPanel()
    {
        
    }

    public void SplashPanelSetActive(bool state)
    {
    }
}

public class StartScreen
{
     public string findTxt;
     public Sprite signImg;

     public StartScreen(Sprite signImage, string s)
    {
        signImg = signImage; 
        findTxt = s;
    }
}