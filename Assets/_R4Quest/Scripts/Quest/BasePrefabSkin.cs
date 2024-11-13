using UnityEngine;

[CreateAssetMenu(menuName = "R4Quest/Base Prefab Skin", fileName = "BasePrefabSkin")]
public class BasePrefabSkin : ScriptableObject
{
    public Sprite _mainBackImg1;
    public Sprite _mainBackImg2;
    public Color _mainBackColor = Color.white;
    public Sprite _mainBackImg;
    public Sprite _titleBackImg;
    public Sprite _titleBackImgEmpty;
    public Sprite _decorImg;
    public Sprite _additionalImg;
    public Sprite _answerImg;
    public Sprite _rightBackImg;
    public Sprite _rightBackImgEmpty;
    public Sprite _leftBackImg;
    public Sprite _leftBackImgEmpty;
    public Sprite _buttonsBackImg;
}

public class BasePrefabSkinData
{
    public string _mainBackImg1;
    public string _mainBackImg2;
    public string _mainBackColor;
    public string _mainBackImg;
    public string _titleBackImg;
    public string _titleBackImgEmpty;
    public string _decorImg;
    public string _additionalImg;
    public string _answerImg;
    public string _rightBackImg;
    public string _rightBackImgEmpty;
    public string _leftBackImg;
    public string _leftBackImgEmpty;
    public string _buttonsBackImg;
}