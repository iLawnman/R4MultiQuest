using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BootstrapUI : MonoBehaviour
{
    [SerializeField] private GameObject Indicator;
    [SerializeField] private float fadeTime = 1;
    [SerializeField] private TMP_Text TextInfo;
    void Start()
    {
        BootstrapActions.OnShowInfo += OnShowInfo;
    }

    private void OnEnable()
    {
        throw new NotImplementedException();
    }

    private void OnShowInfo(string s)
    {
        if (s == string.Empty)
            HideInfo();
        else
            TextInfo.text = s;
    }

    private void HideInfo()
    {
        GetComponent<UIFXBase>().enabled = false;
        
        foreach (var spriteRenderer in Indicator.transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.DOFade(0, fadeTime);
        }
        foreach (var txt in Indicator.transform.parent.gameObject.GetComponentsInChildren<TMP_Text>())
        {
            txt.DOFade(0.1f, fadeTime);
        }
    }

    private void OnDestroy()
    {
        BootstrapActions.OnShowInfo -= OnShowInfo;
    }
}