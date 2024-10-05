using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

    public class LeftTimer : MonoBehaviour
    {
        [Header("Ingame timers")]
        public int QuestTimeAvailible;
        public Text tTimer;
        public int curTime;
        private int leftMin;
        public int leftSec;

        private int pauseTime;
        private int unpauseTime;

        private Color defColor;
        private bool punching;

        private void Start()
        {
            tTimer = GameObject.Find("QuestTimer").GetComponent<Text>();
            defColor = GameObject.Find("QuestTimer").GetComponent<Text>().color;
            tTimer.color = defColor;

            StartCoroutine("StartCounter");
            Application.runInBackground = true;
        }

        public void StopTimer()
        {
            StopAllCoroutines();
            tTimer.text = "";
            Debug.Log("Timer stopped");
        }
        
        private void Update()
        {
            if (leftSec % 60 == 0)
            {
                tTimer.color = Color.yellow;
                if (!punching)
                    Punching();
            }
            else
                tTimer.color = defColor;
        }

        IEnumerator StartCounter()
        {
            yield return new WaitForSeconds(1f);

            curTime++;
            leftSec = QuestTimeAvailible * 60 - curTime;
            tTimer.text = (leftSec / 60).ToString() + ":" + (leftSec % 60).ToString("D2");

            if (leftSec < 1)
            {
                Debug.Log("Time end " + transform.parent.name);
                StartCoroutine("timeout");
                tTimer.text = "";
            }
            else
                StartCoroutine("StartCounter");
        }
        IEnumerator timeout () {

            //var introPanel = FindFirstObjectByType<MainCanvasController>().IntroPanel;
            //introPanel.SetActive(true);
            
            //TO-DO
            //introPanel.transform.Find("present").GetComponent<Text>().text = "Квест не пройден... \r\n\nПопробуйте еще раз!";
            //yield return new WaitUntil(() => !introPanel.activeInHierarchy);
            Application.Quit();
            yield return null;
        }
        
        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                unpauseTime = (int)Time.realtimeSinceStartup;
                curTime += (unpauseTime - pauseTime);
            }
            else
            {
                pauseTime = (int)Time.realtimeSinceStartup;
            }
        }

        void Punching ()
        {
            punching = true;

            //FindFirstObjectByType<SoundManager>().PlayOneShot(SoundTags.MinuteTimer);
            tTimer.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 1, 0, 1).OnComplete(() => punching = false);
            
            tTimer.transform.localScale = Vector3.one;
        }
    }

