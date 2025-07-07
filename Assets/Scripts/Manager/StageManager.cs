using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public GameObject[] stages; // 스테이지 오브젝트들
    public GameObject player; // 플레이어 오브젝트
    public GameObject stageObject;
    public float duration = 0.5f; // 스테이지 오브젝트 활성화 시간
    private int stageIndex = 0; // 현재 스테이지 인덱스
    private bool isMoving = true;
    
    void Awake()
    {
        Invoke("StageObjectActivated", duration);
    }

    void Update()
    {
        if(isMoving)
        {
            return;
        }
        
    
        // 스테이지 마우스 클릭시 실행
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if(hit.collider != null)
            {
                string[] str = hit.transform.name.Split();

                // 스테이지 씬 이동
                if(int.Parse(str[1])-1 == stageIndex)
                {
                    StageMove();
                }

                // 클릭한 스테이지로 이동
                else
                {
                    stageIndex = int.Parse(str[1])-1;

                    // StartCoroutine(StageSelected());
                    StageSelected();
                }
            }
        }

        // A키를 눌러 이전 스테이지 선택
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
        {
            if(stageIndex != 0)
            {
                stageIndex--;

                // StartCoroutine(StageSelected());
                StageSelected();

            }
        }

        // D키를 눌러 다음 스테이지 선택
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
        {
            if(stageIndex != stages.Length)
            {
                stageIndex++;

                // StartCoroutine(StageSelected());
                StageSelected();
            }
        }

        // 스테이지 씬 이동
        if(Input.GetKeyDown(KeyCode.J)||Input.GetKeyDown(KeyCode.K)||Input.GetKeyDown(KeyCode.Return))
        {
            StageMove();
        }
    }

    // 스테이지 선택

    // IEnumerator StageSelected()
    // {
    //     isMoving = true;
    //     SpriteDeactived();
    //     stages[stageIndex].GetComponent<Stage>().StageActived();
    //     Vector3 targetPosition = stages[stageIndex].transform.position;
    //     targetPosition.y += 0.7f;
    //     player.transform.DOMove(targetPosition, duration);
    //     yield return null;
    //     isMoving = false;
    // }
    void StageSelected()
    {
        if(stageIndex >= stages.Length)
            return;

        SpriteDeactived();
        Debug.Log("스테이지인덱스번호" + stageIndex);
        stages[stageIndex].GetComponent<Stage>().StageActived();
        Vector3 targetPosition = stages[stageIndex].transform.position;
        targetPosition.y += 0.4f;
        player.transform.position = targetPosition;
    }

    // 스테이지 씬 이동
    void StageMove()
    {
        GameManager.Instance.SceneOutSquare();
        StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, () =>
        {
            GameManager.Instance.LoadStagePlayScene(stageIndex);
        }));
    }

    // 스테이지 비활성 이미지로 변경
    void SpriteDeactived()
    {
        for(int i=0; i<stages.Length; i++)
        {
            stages[i].GetComponent<Stage>().StageDeactived();
        }
    }

    // 오브젝트 활성화 및 초기설정
    void StageObjectActivated()
    {
        // 오브젝트 활성화
        stageObject.SetActive(true);

        // 플레이어 초기 좌표 설정
        Vector3 targetPosition = stages[stageIndex].transform.position;
        targetPosition.y += 0.4f;
        player.transform.position = targetPosition;

        // 스테이지 인덱스 초기화
        stageIndex = 0;
        // 스테이지 활성화 이미지 변경
        // stages[stageIndex].GetComponent<Stage>().StageActived();

        isMoving = false;
    }
}
