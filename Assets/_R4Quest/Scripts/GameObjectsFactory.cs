using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using VContainer;
using VContainer.Unity;

public class GameObjectsFactory
{
    private ConfigDataContainer container;
    private readonly IObjectResolver resolver;

    public GameObjectsFactory(IObjectResolver _resolver, ConfigDataContainer _container)
    {
        resolver = _resolver;
        container = _container;
    }

    public void CreateARTarget(QuestData quest, string trackeImg, Transform transform)
    {
        Debug.Log("create ar target for " + trackeImg + " quest " + quest.QuestID);
        GameObject prefab = container.ApplicationData.Prefabs.FirstOrDefault(x => x.name.Contains("BasePrefab"));

        //var arTarget = Object.Instantiate(prefab);
        var arTarget = resolver.Instantiate(prefab, transform);
        arTarget.transform.localPosition = Vector3.zero;
        arTarget.name = quest.QuestID;
        resolver.InjectGameObject(arTarget);
        
        var answControl = arTarget.GetComponent<AnswersController>();
        answControl.answers = BuildAnswers(quest);
        answControl.InitAnswers();
        ActivateInputPanel(quest, arTarget);

        if (quest.QuestID.Contains("Q"))
            BuildTask(quest, arTarget);
        else
            BuildArtefact(quest, arTarget);
    }
    private List<AnswerData> BuildAnswers(QuestData quest)
    {
        Debug.Log("buld answers " + quest.AnswerList);
        List<AnswerData> returnData = new List<AnswerData>();
        foreach (var a in container.ApplicationData.Answers)
        {
            if (quest.AnswerList.Contains(a.Answer_ID))
            {
                BasePrefabTexts txts = new BasePrefabTexts
                {
                    _answer_ID = a.Answer_ID,
                    _mainTxt = a.MainText,
                    _title = a.TitleText,
                    _helpUp = a.HelpUpText,
                    _helpDown = a.HelpDownText
                };

                BasePrefabImages imgs = new BasePrefabImages()
                {
                    _answerImg = container.ApplicationData.Sprites.FirstOrDefault(x => x.name == a.AnswerPicture)
                };
                BasePrefabPrefabs prfbs = new BasePrefabPrefabs()
                {
                    AdditionalPref = container.ApplicationData.Prefabs.FirstOrDefault(x => x.name == a.AdditionalPref),
                    TextDecor = container.ApplicationData.Prefabs.FirstOrDefault(x => x.name == a.TextDecor)
                };

                BasePrefabDecor decor = new BasePrefabDecor()
                {
                    TwoLines = a._2lineImg == "1",
                    FourCorner = a._4cornerImg == "1",
                    TitleBackEmptyImg = a.TitleBackEmptyImg == "1",
                    LeftEmptyImg = a.LeftEmptyImg == "1",
                    RightEmptyImg = a.RightEmptyImg == "1"
                };
                AnswerData answerData = new AnswerData();
                answerData.txts = txts;
                answerData.imgs = imgs;
                answerData.prefabs = prfbs;
                answerData.decor = decor;
                returnData.Add(answerData);
            }
        }

        return returnData;
    }
    private void BuildTask(QuestData quest, GameObject arTarget)
    {
        Debug.Log("build task " + quest.QuestID);

        var basePrefabView = arTarget.GetComponent<QuestBasePrefabView>();
        if (!arTarget.GetComponent<iQuestTask>())
            arTarget.AddComponent<iQuestTask>();
        var q = arTarget.GetComponent<iQuestTask>();
        q.signImage = quest.SignImage;
        q.recognitionImage = quest.RecognitionImage;
        q.name = quest.QuestID;
        q.Question = quest?.Question;
        q.rightAnswerIndex = quest?.RightWayIndx;
        q.rightWayQuest = quest?.RightWayQuest;
        q.RightReaction = quest?.RightReaction;
        q.wrongWayQuest = quest?.WrongWayQuest;
        q.WrongReaction = quest?.WrongReaction;
        q.WrongReactionSign = quest?.WrongReactionSign;
        q.RightReactionSign = quest?.RightReactionSign;
        q.timer = int.Parse(quest?.Timer);
    }

    private void BuildArtefact(QuestData quest, GameObject arTarget)
    {
        Debug.Log("build artefact");

        var q = arTarget.AddComponent<iQuestArtefact>();
        q.recognitionImage = quest.RecognitionImage;
        q.signImage = quest.SignImage;
        q.name = quest.QuestID;
        q.Question = quest.Question;
        q.noArtefact = quest.NoArtefact == "1";
        q.artefactPref = quest.BasePrefab;
        q.Reaction = quest.RightReaction;
        q.ReactionSign = quest.RightReactionSign;

        if (q.artefactPref != string.Empty)
            BuildAdditionalPrefab(q.artefactPref, arTarget.transform);
        q.Timer = int.Parse(quest.Timer);
    }
    
    public void BuildLocations(QuestData _quest, GameObject arTarget, string referenceImageName)
    {
        referenceImageName = referenceImageName.Substring(1);
        Debug.Log("build location for image T" + referenceImageName);

        var locations = container.ApplicationData.Location;
        if (locations.text == string.Empty)
            return;

        JArray locationArray = JArray.Parse(locations.text);
        var questId = referenceImageName; //.Substring(1);

        // var locationId = StepsQuestDict[int.Parse(referenceImageName)];
        // var locationIdstr = locationId.ToString();
        // if (locationId < 10)
        //     locationIdstr = "0" + locationId;
        //
        // Debug.Log("location ID " + locationIdstr);
        //
        // questId = locationIdstr;
        // var txtPanels = locationArray.Where(x => x.SelectToken("key").ToString().Substring(1, 2) == questId);
        // //txtPanels.ToList().ForEach(x => Debug.Log("txtPanel " + x.SelectToken("key")));
        //
        // for (int i = 1; i < 9; i++)
        // {
        //     var sliderName = "Slider" + i;
        //     //Card01 -Slider8 -Photo
        //     var sliderImg = "Card" + questId + " -" + sliderName + " -Photo";
        //     panel.name = "Card" + questId + " -" + sliderName;
        //
        //     var imgs = _resourcesService.GetSprite(sliderImg + "4");
        //
        //     if (imgs != null)
        //     {
        //         //Debug.Log(sliderName + " is images panel " + imgs.name);
        //         panel.typeEnum = DrawIfAttribute.PanelTypeEnum.FourImg;
        //         panel.img1 = _resourcesService.GetSprite(sliderImg + "1");
        //         panel.img2 = _resourcesService.GetSprite(sliderImg + "2");
        //         panel.img3 = _resourcesService.GetSprite(sliderImg + "3");
        //         panel.img4 = _resourcesService.GetSprite(sliderImg + "4");
        //     }
        //     else
        //     {
        //         var txt = txtPanels.FirstOrDefault(x =>
        //             x.SelectToken("key").ToString().Contains("S" + i));
        //
        //         if (txt != null)
        //         {
        //             //Debug.Log(sliderName + " is text panel " + i + " with " + txt);
        //             panel.typeEnum = DrawIfAttribute.PanelTypeEnum.ImgTtleTxt;
        //             panel.Title = txt.SelectToken("Title").ToString();
        //             panel.MainTxt = txt.SelectToken("MainTexts").ToString();
        //             panel.topImg = _resourcesService.GetSprite(sliderImg + "1");
        //             //panel.bottomTxt = y.SelectToken("BottomTxt").ToString();
        //         }
        //     }
        //
        //     if (panel.typeEnum != DrawIfAttribute.PanelTypeEnum.None)
        //         data.Add(panel);
        // }
        //
        // GameObject loGameObject =
        //     _resourcesService.GetGameobjectFromPrefab("LocationPrefab", arTarget.transform).Result;
        // loGameObject.transform.rotation = arTarget.transform.rotation;
        //
        //
        // LocationPanelData locationPanelData = new LocationPanelData(data);
        //
        // loGameObject.GetComponent<LocationPrefab>().FillLocationPanel(locationPanelData);
    }

    private void ActivateInputPanel(QuestData quest, GameObject arTarget)
    {
        var questView = arTarget.GetComponent<QuestBasePrefabView>();
        questView.ResetControls();
        var prefab = quest.BasePrefab;

        if (prefab.Contains("Slide"))
        {
            questView.OnSliderButtons();
        }

        if (prefab.Contains("ButtonAnsw"))
        {
            questView.OnFourButton(quest.AnswerList.Split(", ").ToList());
        }

        if (prefab.Contains("InputField"))
        {
            questView.OnInputFeild();
            FillAnswers(quest, arTarget);
        }

        if (prefab.Contains("ArtPrefab"))
        {
            questView.OnNextButton();
        }

        Debug.Log("activated input panel" + quest.BasePrefab);
    }

    private void FillAnswers(QuestData quest, GameObject go)
    {
        if (!go.GetComponent<iQuestTask>())
            go.AddComponent<iQuestTask>();

        go.GetComponent<iQuestTask>().rightAnswerIndex = string.Empty;

        foreach (var a in container.ApplicationData.Answers)
        {
            if (quest.AnswerList.Contains(a.Answer_ID))
            {
                go.GetComponent<iQuestTask>().rightAnswerIndex +=
                    "," + a.MainText;
            }
        }
    }
    
    private void BuildAdditionalPrefab(string additionalPrefab, Transform basePrefab)
    {
        var adduis = basePrefab.GetComponents<Transform>().Where(x => x.name == "AddUI").ToList();

        if (adduis.Count() > 0)
        {
            for (int i = 0; i < adduis.Count(); i++)
            {
                adduis[i].gameObject.SetActive(false);
            }
        }

        var addGo = Object.Instantiate(container.ApplicationData.Prefabs
            .FirstOrDefault(x => x.name == additionalPrefab));
        addGo.name = addGo.name.Replace("(Clone)", string.Empty);
    }
}