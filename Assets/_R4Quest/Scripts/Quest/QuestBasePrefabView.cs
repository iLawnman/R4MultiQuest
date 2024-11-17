using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class QuestBasePrefabView : MonoBehaviour
{
    [Header("-- Main Back")] [SerializeField]
    private SpriteRenderer _mainBackImg1;

    [SerializeField] private SpriteRenderer _mainBackImg2;

    [Header("-- Title")] [SerializeField] private SpriteRenderer _titleBackImg;
    [SerializeField] private GameObject _titleBackEmptyImg;
    [SerializeField] private TMP_Text _title;

    [Header("-- Main Text")] [SerializeField]
    private GameObject _2lineImg;

    [SerializeField] private GameObject _4cornerImg;
    [SerializeField] private SpriteRenderer _mainBackImg;
    [SerializeField] private TMP_Text _mainTxt;

    [Header("-- Additional")] [SerializeField]
    private Image _additionalImg;

    [Header("Right Panel")] [SerializeField]
    private GameObject _rightEmptyImg;

    [SerializeField] private GameObject _rightDecorImg;
    [SerializeField] private SpriteRenderer _rightBackImg;

    [Header("-- Left Panel")] [SerializeField]
    private GameObject _leftEmptyImg;

    [SerializeField] private GameObject _leftDecorImg;
    [SerializeField] private SpriteRenderer _leftBackImg;
    [SerializeField] private TMP_Text _helpUp;
    [SerializeField] private TMP_Text _helpDown;

    [Header("-- Buttons Panel")] [SerializeField]
    private SpriteRenderer _buttonsBackImg;

    [Header("-- Slider Panel")] [SerializeField]
    private GameObject _sliderButtons;

    [SerializeField] private Button _sliderLeftButton;
    [SerializeField] private Button _sliderRightButton;
    [SerializeField] private Button _sliderROKButton;

    [Header("-- Next Panel")] [SerializeField]
    private GameObject _nextPanel;

    [SerializeField] private Button _nextButton;

    [Header("-- 4 Buttons Panel")] [SerializeField]
    private GameObject _4buttons;

    [SerializeField] private Button _1Button;
    [SerializeField] private Button _2Button;
    [SerializeField] private Button _3Button;
    [SerializeField] private Button _4Button;

    [Header("-- Input Panel")] [SerializeField]
    private GameObject _inputDataPanel;

    [SerializeField] private TMP_InputField _inputDataField;
    [SerializeField] private Button _inputFieldOkButton;

    [Header("-- Variant Button")] public Button _variantButton;

    private ConfigDataContainer container;

    [Inject]
    public void Construct(ConfigDataContainer configDataContainer)
    {
        container = configDataContainer;
    }

    private void Start()
    {
        ApplySkin();
    }

    private void ApplySkin()
    {
        Debug.Log("apply baseprefab skin");
        var skin = container.ApplicationData.BasePrefabSkin[0];
        _mainBackImg.sprite = CacheService.GetCachedImage(skin?._mainBackImg + ".png");
        _mainBackImg1.sprite = CacheService.GetCachedImage(skin?._mainBackImg1 + ".png");
        _mainBackImg2.sprite = CacheService.GetCachedImage(skin?._mainBackImg2 + ".png");
        _titleBackImg.sprite = CacheService.GetCachedImage(skin?._titleBackImg + ".png");
        _titleBackEmptyImg.GetComponentsInChildren<Image>().ToList().ToList().ForEach(x =>
            x.sprite = CacheService.GetCachedImage(skin?._titleBackImgEmpty + ".png"));
        var decorImg = CacheService.GetCachedImage(skin?._decorImg + ".png");
        _additionalImg.sprite = CacheService.GetCachedImage(skin._additionalImg + ".png");
        _rightBackImg.sprite = CacheService.GetCachedImage(skin._rightBackImg + ".png");
        _rightEmptyImg.GetComponentInChildren<SpriteRenderer>().sprite =
            CacheService.GetCachedImage(skin._rightBackImgEmpty + ".png");
        _leftBackImg.sprite = CacheService.GetCachedImage(skin._leftBackImg + ".png");
        _leftEmptyImg.GetComponentInChildren<SpriteRenderer>().sprite =
            CacheService.GetCachedImage(skin._leftBackImgEmpty + ".png");
        _buttonsBackImg.sprite = CacheService.GetCachedImage(skin._buttonsBackImg + ".png");
    }

    public void FillAnswer(AnswerData answer)
    {
        FillImages(answer.imgs);
        FillTexts(answer.txts);
        FillPrefabs(answer.prefabs);
        //FillDecor(answer.decor);
    }

    private void FillDecor(BasePrefabDecor answerDecor)
    {
        _2lineImg.SetActive(answerDecor.TwoLines);
        _4cornerImg.SetActive(answerDecor.FourCorner);
        _titleBackEmptyImg.SetActive(answerDecor.TitleBackEmptyImg);
        //_leftEmptyImg.SetActive(answerDecor.LeftEmptyImg);
       // _rightEmptyImg.SetActive(answerDecor.RightEmptyImg);
    }

    private void FillPrefabs(BasePrefabPrefabs answerPrefabs)
    {
        var objList = transform.GetComponentsInChildren<Transform>();
        var addObjs = objList.Where(x => x.name.Contains("AddPrefab")).ToList();
        addObjs.AddRange(objList.Where(x => x.name.Contains("TxtDecor")).ToList());
        addObjs.ForEach(x => Destroy(x.gameObject));

        if (answerPrefabs.AdditionalPref != null)
        {
            var addpref = Instantiate(answerPrefabs.AdditionalPref, transform);
            addpref.name = addpref.name.Replace("(Clone)", "AddPrefab");
        }

        if (answerPrefabs.TextDecor != null)
        {
            var txtDecor = Instantiate(answerPrefabs.TextDecor, transform);
            txtDecor.name = txtDecor.name.Replace("(Clone)", "TxtDecor");
        }
    }

    private void FillTexts(BasePrefabTexts txts)
    {
        OnIfEmptyLeft(true);
        OnTitleDecor(true);

        _title.text = txts._title;
        _mainTxt.text = txts._mainTxt;
        _helpUp.text = txts._helpUp;
        _helpDown.text = txts._helpDown;

        if (!string.IsNullOrWhiteSpace(txts._helpUp) || !string.IsNullOrWhiteSpace(txts._helpDown))
            OnIfEmptyLeft(false);

        if (!string.IsNullOrWhiteSpace(txts._title))
            OnTitleDecor(false);
    }

    private void FillImages(BasePrefabImages imgs)
    {
        _rightEmptyImg.SetActive(true);

        if (imgs._mainBackImg != null)
        {
            _mainBackImg.sprite = imgs._mainBackImg;
            _mainBackImg.color = imgs._mainBackColor;
        }

        if (imgs._mainBackImg1 != null)
            _mainBackImg1.sprite = imgs._mainBackImg1;
        if (imgs._mainBackImg2 != null)
            _mainBackImg2.sprite = imgs._mainBackImg2;

        if (imgs._answerImg != null)
            SetAnswerPicture(imgs._answerImg);
        else
            _additionalImg.enabled = false;
    }

    public void OnSliderButtons()
    {
        var answerC = FindFirstObjectByType<AnswersController>();
        _sliderButtons.SetActive(true);
        _sliderLeftButton.onClick.AddListener(answerC.ShowPreviewsAnswer);
        _sliderRightButton.onClick.AddListener(answerC.ShowNextAnswer);
        _sliderROKButton.onClick.AddListener(answerC.CheckAnswer);
    }

    public void OnNextButton()
    {
        _nextPanel.SetActive(true);
        _nextButton.onClick.AddListener(GetComponent<AnswersController>().NextStep);
    }

    public async void OnFourButton(List<string> answers)
    {
        _4buttons.SetActive(true);

        var answerC = GetComponent<AnswersController>();

        await Task.Delay(100);

        _1Button.GetComponentInChildren<Text>().text =
            answerC.answers.FirstOrDefault(x => x.txts._answer_ID == answers[1])?.txts._mainTxt;
        _2Button.GetComponentInChildren<Text>().text =
            answerC.answers.FirstOrDefault(x => x.txts._answer_ID == answers[2])?.txts._mainTxt;
        _3Button.GetComponentInChildren<Text>().text =
            answerC.answers.FirstOrDefault(x => x.txts._answer_ID == answers[3])?.txts._mainTxt;
        _4Button.GetComponentInChildren<Text>().text =
            answerC.answers.FirstOrDefault(x => x.txts._answer_ID == answers[4])?.txts._mainTxt;

        _1Button.onClick.AddListener(() => answerC.CheckAnswer(1));
        _2Button.onClick.AddListener(() => answerC.CheckAnswer(2));
        _3Button.onClick.AddListener(() => answerC.CheckAnswer(3));
        _4Button.onClick.AddListener(() => answerC.CheckAnswer(4));
    }

    public void OnInputFeild()
    {
        _inputDataPanel.SetActive(true);
        _inputFieldOkButton.onClick.AddListener(() =>
        {
            GetComponent<AnswersController>().CheckAnswer(_inputDataField.text);
            _inputFieldOkButton.onClick.RemoveAllListeners();
        });
    }

    public void SetBaseColor(Color skinArtefactColor) => _mainBackImg1.color = skinArtefactColor;

    public void SetAnswerPicture(Sprite answerPicture)
    {
        _additionalImg.enabled = true;
        _additionalImg.sprite = answerPicture;
        _rightEmptyImg.SetActive(false);
    }

    void OnIfEmptyLeft(bool state) => _leftEmptyImg.SetActive(state);

    void OnTitleDecor(bool state) => _titleBackEmptyImg.SetActive(state);

    public void ResetControls()
    {
        _4buttons.SetActive(false);
        _nextPanel.SetActive(false);
        _sliderButtons.SetActive(false);
        _inputDataPanel.SetActive(false);
    }

    public void SetOkButton(bool b) => _sliderROKButton.gameObject.SetActive(b);
}