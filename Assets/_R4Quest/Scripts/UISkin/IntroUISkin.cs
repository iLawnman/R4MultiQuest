using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "IntroUISkin", menuName = "iLawn/IntroUISceneSkin")]
public class IntroUISkin : UISkin
{
    public Sprite infoBack;
    public Sprite image;
    public Font text;
    public Font taptext;
    public Sprite closeButtom;
    
    public Sprite lastBack;
    public Sprite logo;
    public TMP_FontAsset credits;
    public Sprite linkButtom;
    public Sprite closeLastButtom;

    public Sprite finalBack;
    public TMP_FontAsset finalTitle;
    public TMP_FontAsset  finalTxt;
    public Sprite finalCloseButtom;
}