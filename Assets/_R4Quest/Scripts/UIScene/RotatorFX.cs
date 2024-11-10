using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RotatorFX : UIFXBase
{
    [SerializeField] private Transform boxParent;
    private SpriteRenderer[] boxes;
    [SerializeField] private Color targetColor;
    [SerializeField] private GameObject Indicator;
    [SerializeField] private float IndicatorSpeed = 5;
    [SerializeField] private float fadeTime = 1;
    [SerializeField] private TMP_Text TextInfo;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private int boxSwithPause = 1;
    void Start()
    {
        BootstrapActions.OnShowInfo += ChangeTextFX; 
        boxes = boxParent.GetComponentsInChildren<SpriteRenderer>()
            .Where(x => x.name == "box")
            .Where(y => y.GetComponent<Rigidbody2D>().gravityScale == -1)
            .ToArray();
    }

    private void ChangeTextFX(string obj)
    {
        IndicatorSpeed *= -1;
    }

    private void OnEnable()
    {
        StartCoroutine(DoBoxes());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator DoBoxes()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            int rIndx = Random.Range(0, boxes.Length);

            boxes[rIndx].DOColor(targetColor, duration).SetEase(Ease.Linear);
            boxes[rIndx].transform.DOScale(new Vector3(0.15f, 0.15f, 0.15f), duration).SetEase(Ease.Linear);
            boxes[rIndx].GetComponent<Rigidbody2D>().gravityScale *= -1;
        }
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
}
