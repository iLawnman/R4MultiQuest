using System.Collections;
using UnityEngine;
using UnityEngine.UI;

    public class TotalTimer : MonoBehaviour
    {
        [Header("Ingame timers")]
        public int QuestTimeAvailible = 0;
        public Text tTimer;
        public int curTime;
        private int leftMin;
        private int leftSec;

        private int pauseTime;
        private int unpauseTime;

        private void OnEnable()
        {
            if(QuestTimeAvailible == 0)
                return;
            
            StartCoroutine("StartCounter");
            Application.runInBackground = true;

            tTimer = GetComponent<Text>();
        }

        IEnumerator StartCounter()
        {
            yield return new WaitForSeconds(1f);
            curTime++;
            leftSec = QuestTimeAvailible * 60 - curTime;
            tTimer.text = (leftSec / 60).ToString() + ":" + (leftSec % 60).ToString("D2");

            if (leftSec < 1)
            {
                Debug.Log("Time end");
                //FindObjectOfType<SceneflowController>().TimerOut();
                tTimer.text = "--:--";
            }
            else
            StartCoroutine("StartCounter");
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
    }