using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
    public class IntroScreen
    {
        public string Title;
        [Multiline]
        public string text;
        public Sprite image;
        public Sprite background;
        public string nextScreenName;
        public GameObject additionalUIpref;

    }

    public class InfoPanelsController : MonoBehaviour
    {
        [Header("Game UI Elements")]
        public GameObject startPanel;
        public GameObject questStartPanel;
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

        void ShowScreen(string name)
        {
                slideInprogress = true;

                startPanel.SetActive(true);

                var newPanel = Instantiate(startPanel, startPanel.transform.parent);
                newPanel.name = "panel_" + name;

                newPanel.transform.SetAsFirstSibling();
                newPanel.transform.DOMoveX(toPositionNew, 0.7f).From(fromPositionNew)
                    .OnComplete(() => slideInprogress = false);

                var newScreen = introScreens.Where(screen => screen.Title == name).ToList();
                
                    currentScreenInx = introScreens.FindIndex(screen => screen.Title == name);

                    if(newScreen.Count == 0)
                        return;
                    else
                    {
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
                                 Destroy(newPanel.transform.Find("addUI").gameObject);
                            
                            var addUI = Instantiate(newScreen[0].additionalUIpref, newPanel.transform);
                            addUI.name = "addUI";
                        }
                    }

            destroyPanel(startPanel);
            startPanel = newPanel;
        }

        void destroyPanel (GameObject panel)
        {
            panel.transform.DOMoveX(toPositionOld, 0.7f).From(fromPositionOld).OnComplete(() =>
            {
                slideInprogress = false;
                panel.SetActive(false);
                //Destroy(panel, 1);
            });
        }

        public void ShowOutroScreen()
        {
            if (outroScreens.Count > 0)
            {
                introScreens = outroScreens;
                startPanel.transform.parent.gameObject.SetActive(true);
                nextScreen = "End Quest";
                ShowScreen(nextScreen);
            }
            else
            {
                startPanel.transform.parent.gameObject.SetActive(false);
                LastPanel.SetActive(true);
            }
        }

        public void PrevScreen ()
        {
            if (slideInprogress == false)
            {
                if (currentScreenInx > 0)
                {
                    fromPositionNew = -Screen.width / 2;
                    toPositionNew = Screen.width / 2;
                    fromPositionOld = Screen.width / 2;
                    toPositionOld = Screen.width * 1.5f;

                    var prevScreen = introScreens[currentScreenInx - 1].Title;
                    ShowScreen(prevScreen);
                }
            }
        }

        public void NextScreen()
        {
                fromPositionNew = Screen.width * 1.5f;
                toPositionNew = Screen.width / 2;
                fromPositionOld = Screen.width / 2;
                toPositionOld = -Screen.width / 2;
                
                if (nextScreen == "Quest Start")
                {
                    startPanel.SetActive(false);
                    StartTimer();
                    startPanel.transform.parent.gameObject.SetActive(false);
                    return;
                }
                if (nextScreen == "Quit")
                {
                    startPanel.SetActive(false);
                    LastPanel.SetActive(true);
                }

            else 
            {
                if (slideInprogress == false)
                {
                    ShowScreen(nextScreen);
                }
            }
        }

        private static void StartTimer()
        {
            var tTimer =FindObjectOfType<TotalTimer>();
            if (tTimer != null)
            {
                tTimer.curTime = 0;
                tTimer.enabled = true;
            }
        }

        public void CloseApp()
        {
            Application.Quit();
        }

        public void ShowStartSequence ()
        {
            toPositionNew = Screen.width / 2;

            ShowScreen("Start Quest Screen");
        }
    }
