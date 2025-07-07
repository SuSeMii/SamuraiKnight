using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KoreanTyper;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DialogueManager2 : MonoBehaviour
{
    public GameObject tutorialObject;
    public TMP_Text TMPtext; // 텍스트를 표시할 텍스트 박스
    public string[] inputText; // 출력할 텍스트 배열
    public float typingDuration = 0.05f; // 출력 간격
    public float tutorialDuration = 1f;
    
    private int index = 0; // string 배열 인덱스
    private bool isTypingEnd = false; // 엔터 입력 방지

    public AudioClip typingClip;
    public float typingVolume;
    AudioSource typingPlayer;

    void Start()
    {
        Init();
        tutorialObject.transform.localScale = Vector3.zero;
        tutorialObject.SetActive(false);
        StartCoroutine(Typing(inputText[index]));
    }

    void Update()
    {
        //스킵 추가
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            GameManager.Instance.LoadStageSelectScene();


        }

        if(!isTypingEnd)
        {
            return;
        }

        if(tutorialObject.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)))
        {
            GameManager.Instance.LoadStageSelectScene();
        }

        // 엔터 키 입력으로 다음 대사 출력
        else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
        {
            index++;
            // 출력할 대사가 없으면 스테이지 선택 씬으로 이동
            if(index == inputText.Length)
            {   
                GameManager.Instance.LoadStageSelectScene();
                return;
            }

            // 대사 타이핑 코루틴 호출
            StartCoroutine(Typing(inputText[index]));
        }
    }

    // 대사 타이핑 코루틴
    IEnumerator Typing(string talk)
    {
        // 대사 출력이 종료되기 전까지 엔터 입력을 받지 않도록 설정
        isTypingEnd = false;

        // 텍스트 박스 초기화
        TMPtext.text = null;

        // 대사에 존재하는 띄어쓰기 두 번을 줄바꿈으로 바꿈
        if(talk.Contains("  "))
            talk = talk.Replace("  ", "\n");

        PlaySound();

        int typingLength = talk.GetTypingLength();
        for(int index = 0; index <= typingLength; index++)
        {
            TMPtext.text = talk.Typing(index);
            yield return new WaitForSeconds(typingDuration);
        }

        // 다음 대사 딜레이
        // yield return new WaitForSeconds(1);

        StopSound();
        isTypingEnd = true;
    }

    void Init()
    {
        GameObject typingObject = new GameObject("Typing");
        typingObject.transform.parent = transform;
        typingPlayer = typingObject.AddComponent<AudioSource>();
        typingPlayer.playOnAwake = false;
        typingPlayer.loop = true;
        typingPlayer.volume = typingVolume;
        typingPlayer.clip = typingClip;
    }

    public void PlaySound()
    {
        typingPlayer.Play();
    }

    public void StopSound()
    {
        typingPlayer.Stop();
    }

}
