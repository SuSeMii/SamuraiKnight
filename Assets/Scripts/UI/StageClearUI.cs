using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class StageClearUI : MonoBehaviour
{
    //public Image rankImage;
    public SpriteRenderer rankImage;
    public Light2D rankLight;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI commentText;
    StageUI stageUI;
    RectTransform clearPanel;

    void Start()
    {
        clearPanel = GetComponent<RectTransform>();
        stageUI = GameObject.Find("StageUICanvas").GetComponent<StageUI>();
        rankImage.sprite = stageUI.rankImage.sprite;
        rankLight.lightCookieSprite = stageUI.rankLight.lightCookieSprite;
        //rankLight.intensity = stageUI.rankLight.intensity;
        scoreText.text = stageUI.scoreText.text;
        SetComment();
        StartCoroutine(DropUI());
    }

    void SetComment()
    {
        switch(StagePlayManager.Instance.rank)
        {
            case "SSS":
                commentText.text = "훌륭한 솜씨다.";
                break;
            case "SS":
                commentText.text = "SS Rank Comment";
                break;
            case "S":
                commentText.text = "마음가짐은 좋았군";
                break;
            case "A":
                commentText.text = "더 잘할 수 있다고 믿는다.";
                break;
            case "B":
                commentText.text = "검을 소중히 하지 않는군.";
                break;
            case "C":
                commentText.text = "마음이 다른 곳에 가있군.";
                break;
            case "D":
                commentText.text = "다시 검을 쥐는 자세부터 고쳐줘야겠군.";
                break;
        }
    }

    IEnumerator DropUI()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.StageClear);
        yield return new WaitForSeconds(2.7f);
        StagePlayManager.Instance.isControlable = false;
        clearPanel.DOAnchorPosY(0, 0.5f);
        yield return null;
    }
}
