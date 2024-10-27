using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SplashUISkin", menuName = "iLawn/SplashUISceneSkin")]
public class SplashUISkin : UISkin
{
    [SerializeField] public Sprite logoback;
    [SerializeField] public Sprite logo;
    [SerializeField] public Font text;
    [SerializeField] public Sprite okButton;
    [SerializeField] public Sprite offerButton;
    
    [SerializeField] public Sprite splashBack;
    [SerializeField] public Sprite customlogo;
    [SerializeField] public Sprite r4logo;
    [SerializeField] public TMP_FontAsset name1text;
    [SerializeField] public TMP_FontAsset name2text;
    
    [SerializeField] public TMP_FontAsset askText;
    [SerializeField] public Sprite askBack;
    [SerializeField] public Sprite yesButton;
    [SerializeField] public Sprite noButton;
}