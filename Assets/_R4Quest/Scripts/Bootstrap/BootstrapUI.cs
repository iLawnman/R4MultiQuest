using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BootstrapUI : MonoBehaviour
{
    [SerializeField] private GameObject Indicator;
    [SerializeField] private float IndicatorSpeed = 5;
    [SerializeField] private float fadeTime = 1;
    [SerializeField] private TMP_Text TextInfo;
    void Start()
    {
        BootstrapActions.OnShowInfo += OnShowInfo;
    }


    private void Update()
    {
        if (TextInfo.text != String.Empty)
        {
            Indicator.transform.parent.gameObject.SetActive(true);
            Indicator.transform.RotateAround(Indicator.transform.position, Indicator.transform.forward,
                Time.deltaTime * IndicatorSpeed);
        }
    }

    private void OnShowInfo(string s)
    {
        if (s == string.Empty)
        {
            IndicatorSpeed = 0;
            HideInfo();
        }
        else
        {
            TextInfo.text = s;
            IndicatorSpeed *= -1;
        }
    }

    private async void HideInfo()
    {
        foreach (var spriteRenderer in Indicator.transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.DOFade(0, fadeTime);
        }
        foreach (var txt in Indicator.transform.parent.gameObject.GetComponentsInChildren<TMP_Text>())
        {
            txt.DOFade(0.1f, fadeTime);
        }
        // await UniTask.Delay(2); 
        //
        // foreach (var spriteRenderer in Indicator.transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>())
        // {
        //     spriteRenderer.DOFade(1, 0);
        // }
        // foreach (var txt in Indicator.transform.parent.gameObject.GetComponentsInChildren<TMP_Text>())
        // {
        //     txt.DOFade(1f, 0);
        // }
    }

    private void OnDestroy()
    {
        BootstrapActions.OnShowInfo -= OnShowInfo;
    }
}