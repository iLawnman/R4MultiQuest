using UnityEngine;

public class iQuestTask : iQuest
    {
        public int timer;
        [Header("Right Answer Way")]
        public string rightAnswerIndex;
        public string rightWayQuest;
        [Multiline]
        public string RightReaction;
        [Header("Wrong Answer Way")]
        public string wrongWayQuest;
        [Multiline]
        public string WrongReaction;
        public string WrongReactionSign;
        public string RightReactionSign;

        public override void Fill(iQuest actualQuest)
        {
            FiilQuestTask(actualQuest as iQuestTask);
        }

        private void FiilQuestTask(iQuestTask actualQuest)
        {
            recognitionImage = actualQuest.recognitionImage;
            signImage = actualQuest.signImage;
            
            Question = actualQuest.Question;
            timer = actualQuest.timer;
            rightAnswerIndex = actualQuest.rightAnswerIndex;
            rightWayQuest = actualQuest.rightWayQuest;
            RightReaction = actualQuest.RightReaction;
            WrongReaction = actualQuest.WrongReaction;
            WrongReactionSign = actualQuest.WrongReactionSign;
            RightReactionSign = actualQuest.RightReactionSign;
        }
    }
