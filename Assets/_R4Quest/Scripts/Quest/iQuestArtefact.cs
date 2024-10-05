using UnityEngine;

public class iQuestArtefact : iQuest
    {
        public string artefactPref;
        public int Timer;
        public string WayQuest;
        public bool noArtefact;
        public string Reaction;
        public string ReactionSign;


        public override void Fill(iQuest actualQuest)
        {
            FiilArtefact(actualQuest as iQuestArtefact);
        }

        private void FiilArtefact(iQuestArtefact actualQuest)
        {
            Question = actualQuest.Question;
            recognitionImage = actualQuest.recognitionImage;
            signImage = actualQuest.signImage;
            artefactPref = actualQuest.artefactPref;
            Timer = actualQuest.Timer;
            WayQuest = actualQuest.WayQuest;
            noArtefact = actualQuest.noArtefact;
            ReactionSign = actualQuest.ReactionSign;
        }
    }

