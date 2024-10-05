using UnityEngine;

[CreateAssetMenu(menuName = "R4Quest/Base Prefab Skin", fileName = "BasePrefabSkin")]
public class BasePrefabSkin : ScriptableObject
{
    public Sprite _mainBackImg1;
    public Color _mainBackColor = Color.white;
    public Sprite _mainBackImg2;
    public Sprite _titleBackImg;
    public Sprite _mainBackImg;
    public Sprite _rightBackImg;
    public Sprite _leftBackImg;
    public Sprite _buttonsBackImg;
    public Color ArtefactColor;
}