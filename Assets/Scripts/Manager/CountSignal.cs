using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CountSignal : MonoBehaviour
{
    RectTransform countRect;
    TextMeshProUGUI count;
    private void Start() { 

        countRect = GetComponent<RectTransform>();
        count = GetComponent<TextMeshProUGUI>();

        Sequence seq1 = DOTween.Sequence()
        .AppendCallback(() => 
        {
            count.enabled = true;
            count.text = "3";
        })
        .Append(countRect.DOScale(Vector3.zero, 1f))
        .AppendCallback(() => 
        {
            count.text = "2";
            countRect.transform.localScale = Vector3.one;
        })
        .Append(countRect.DOScale(Vector3.zero, 1F))
        .AppendCallback(() => 
        {
            count.text = "1";
            countRect.transform.localScale = Vector3.one;
        })
        .Append(countRect.DOScale(Vector3.zero, 1f))
        .AppendCallback(() => { countRect.gameObject.SetActive(false); });
    }
}
