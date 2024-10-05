using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class BasePrefabTexts
{
    public string _title;
    public string _mainTxt;
    public string _helpUp;
    public string _helpDown;
    public string _answer_ID;
}
[Serializable]
public class BasePrefabImages
{
    public Sprite _mainBackImg1;
    public Color _mainBackColor;
    public Sprite _mainBackImg2;
    public Sprite _titleBackImg;
    public Sprite _mainBackImg;
    public Sprite _answerImg;
    public Sprite _rightBackImg;
    public Sprite _leftBackImg;
    public Sprite _buttonsBackImg;
}
[Serializable]
public class BasePrefabPrefabs
{
    public GameObject AdditionalPref;
    public GameObject TextDecor;
}
[Serializable]
public class BasePrefabDecor
{
    public bool TwoLines;
    public bool FourCorner;
    public bool TitleBackEmptyImg;
    public bool LeftEmptyImg;
    public bool RightEmptyImg;
}