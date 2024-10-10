using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BootstrapUI : MonoBehaviour
{
    [SerializeField] private GameObject Indicator;
    [SerializeField] private float IndicatorSpeed = 5;
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
        else
        {
            Indicator.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnShowInfo(string s)
    {
        if (s == string.Empty)
            HideInfo();
        else
        {
            TextInfo.text = s;
            IndicatorSpeed *= -1;
        }
    }

    private void HideInfo()
    {
        foreach (var spriteRenderer in Indicator.transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            FXManager.PlayFx(spriteRenderer.gameObject, new HideRendererEffect(), 1);
        }
    }

    private void OnDestroy()
    {
        BootstrapActions.OnShowInfo -= OnShowInfo;
    }
}