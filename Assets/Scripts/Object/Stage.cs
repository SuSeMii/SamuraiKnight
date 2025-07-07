using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public Sprite[] beacons; // 비콘 이미지
    private SpriteRenderer currentImage; // 현재 이미지

    void Start()
    {
        currentImage = GetComponent<SpriteRenderer>();
    }

    // 스테이지 선택 활성화
    public void StageActived()
    {
        currentImage.sprite = beacons[1];
    }

    // 스테이지 선택 비활성화
    public void StageDeactived()
    {
        currentImage.sprite = beacons[0];
    }
}
