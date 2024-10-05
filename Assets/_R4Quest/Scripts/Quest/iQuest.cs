using System;
using UnityEngine;

public static partial class GameActions
    {
        public static Action<iQuest> OnQuestStart;
    }

    public abstract class iQuest : MonoBehaviour
    {
        [Multiline(10)]
        public string Question;
        
        public string recognitionImage;
        public string signImage;

        public abstract void Fill(iQuest actualQuest);

        public virtual void OnEnable()
        {
            GameActions.OnQuestStart?.Invoke(this);            
        }

    }
