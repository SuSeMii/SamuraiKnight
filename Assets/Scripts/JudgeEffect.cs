using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JudgeEffect : MonoBehaviour
{
    public Color PERFECT;
    public Color GREAT;
    public Color GOOD;
    public Color BAD;

    public float duration = 0.3f;
    public TextMeshPro text;
    public void ShowGrade(JudgeType type)
    {
        switch (type)
        {
            case JudgeType.PERFECT:
                text.text = "PERFECT";
                text.color = PERFECT;
                break;
            case JudgeType.GREAT:
                text.text = "GREAT";
                text.color = GREAT;
                break;
            case JudgeType.GOOD:
                text.text = "GOOD";
                text.color = GOOD;
                break;
            case JudgeType.BAD:
                text.text = "BAD";
                text.color = BAD;
                break;
        }

        Sequence seq = DOTween.Sequence()
        .Append(transform.DOMove(new Vector3(0, 2, 0), duration))
        //.Join(transform.DOShakePosition(duration, 0.5f, 3, 90))
        .Join(text.DOFade(0, duration))
        .AppendCallback(() => Destroy(gameObject));
    }
}

public enum JudgeType
{
    PERFECT,
    GREAT,
    GOOD,
    BAD
}