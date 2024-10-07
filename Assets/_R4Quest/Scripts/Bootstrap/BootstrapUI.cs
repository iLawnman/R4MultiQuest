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
        if(TextInfo.text != String.Empty)
            Indicator.transform.RotateAround(Indicator.transform.position, Indicator.transform.forward, Time.deltaTime * IndicatorSpeed);
    }

    private void OnShowInfo(string s)
    {
        TextInfo.text = s;
        IndicatorSpeed *= -1;
    }

    private void OnDestroy()
    {
        BootstrapActions.OnShowInfo -= OnShowInfo;
    }
}
