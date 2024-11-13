using System;
using UnityEngine;

    public abstract class iQuest : MonoBehaviour
    {
        [Multiline(10)]
        public string Question;
        
        public string recognitionImage;
        public string signImage;

        public abstract void Fill(iQuest actualQuest);

        public virtual void OnEnable()
        {
            GameActions.OnQuestStart?.Invoke(this.name);            
        }

    }
