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
    public List<SplashScreenData> SplashScreens = new List<SplashScreenData>();
    public List<IntroScreenData> IntroScreens = new List<IntroScreenData>();
    public List<IntroScreenData> OutroScreens = new List<IntroScreenData>();
    public List<BasePrefabSkinData> BasePrefabSkin = new List<BasePrefabSkinData>();
    public List<string> Scripts = new List<string>();
    public string CurrentQuest;
}

[Serializable]
public class IntroScreenData
{
    public string Title;
    public string Text;
    public string Image;
    public string Background;
    public string NextScreenName;
    public string AdditionalUIprefab;
}

[Serializable]
public class SplashScreenData
{
    public string AlertPanelBack;
    public string Logo;
    public string Text;
    public string okButton;
    public string OfferButton;
    public string SplashPanelBack;
    public string CustomLogo;
    public string R4QLogo;
    public string Name1;
    public string Name2;
    public string AskPanelBack;
    public string AskText;
    public string YesButton;
    public string NoButton;
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
    public string Sound;
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