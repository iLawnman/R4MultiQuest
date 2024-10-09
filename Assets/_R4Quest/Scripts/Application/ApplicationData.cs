using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ApplicationData
{
    public TextAsset Location;
    public List<QuestData> Quests = new List<QuestData>();
    public List<AnswersData> Answers = new List<AnswersData>();
    public List<GameObject> Prefabs = new List<GameObject>();
    public List<Sprite> Sprites = new List<Sprite>();
    public List<Texture2D> Textures = new List<Texture2D>();
    public List<ResourcesData> Resources = new List<ResourcesData>();
}

[Serializable]
public class ResourcesData
{
    public string application;
    public string recognitionImage;
    public string pictures;
    public string sounds;
    public string maps;
}

[Serializable]
public class AnswersData
{
    public string Answer_ID;
    public string BasePrefab;
    public string MainText; 
    public string _2lineImg;
    public string _4cornerImg;
    public string AdditionalImg;
    public string TitleText;
    public string TitleBackEmptyImg;
    public string HelpUpText;
    public string HelpDownText;
    public string LeftEmptyImg;
    public string AnswerPicture;
    public string RightEmptyImg;
    public string AdditionalPref;
    public string TextDecor;
}
[Serializable]
public class QuestData
{
    public string QuestID;
    public string BasePrefab;
    public string GoalIndex;
    public string RecognitionImage;
    public string SignImage;
    public string Question;
    public string Task;
    public string AnswerList;
    public string Timer;
    public string RightWayIndx;
    public string RightWayQuest;
    public string RightReaction;
    public string RightReactionSign;
    public string WrongWayQuest;
    public string WrongReaction;
    public string WrongReactionSign;
    public string Artefact;
    public string NextWayQuest;
    public string NoArtefact;
    public string DistanceTimer;
    public string Sound;
}