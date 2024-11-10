using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "QuestUISkin", menuName = "iLawn/QuestUISceneSkin")]
public class QuestUISkin : UISkin
{
    public string skinName = "QuestUISkin";
    public Sprite questStartBack;
    public Sprite continueButton;
    public Sprite arrowsButton;
}