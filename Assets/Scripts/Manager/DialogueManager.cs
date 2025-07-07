using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KoreanTyper;
using DG.Tweening;
using System.Net;
using System.Diagnostics.Contracts;

public class DialogueManager : MonoBehaviour
{
    public int dialogueIndex;

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

    public GameObject knightSprite;
    public SpriteRenderer knightRenderer;
    public GameObject samuraiSprite;
    public SpriteRenderer samuraiRenderer;
    public Color dehighlightColor;
    public Color highlightColor;
    public GameObject leftEmotion;
    public DialogueEmotion leftEmotionScript;
    public GameObject rightEmotion;
    public DialogueEmotion rightEmotionScript;

    public AudioClip[] sfxAudioClips;
    public AudioSource sfxPlayer;

    private bool isSceneOutable = true;

    void Start()
    {
        Init();
        tutorialObject.transform.localScale = Vector3.zero;
        tutorialObject.SetActive(false);
        StartCoroutine(Typing(inputText[index]));
        DialogueAction(index);
        knightRenderer = knightSprite.GetComponent<SpriteRenderer>();
        samuraiRenderer = samuraiSprite.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //스킵 추가
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isSceneOutable)
            {
                isSceneOutable = false;
                GameManager.Instance.SceneOutSquare();
                StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, GameManager.Instance.LoadNextStory));
            }
        }

        if(!isTypingEnd)
        {
            return;
        }

        if(tutorialObject.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K)))
        {
            if (isSceneOutable)
            {
                isSceneOutable = false;
                GameManager.Instance.SceneOutSquare();
                StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, GameManager.Instance.LoadNextStory));
            }
        }

        // 엔터 키 입력으로 다음 대사 출력
        else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
        {
            index++;
            // 출력할 대사가 없으면 스테이지 선택 씬으로 이동
            if(index >= inputText.Length)
            {
                if (dialogueIndex == 0 && index == inputText.Length) {
                    tutorialObject.SetActive(true);
                    StartCoroutine(ShowTutorial());
                    return;
                } else {
                    if (isSceneOutable)
                    {
                        isSceneOutable = false;
                        GameManager.Instance.SceneOutSquare();
                        StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, GameManager.Instance.LoadNextStory));
                    }
                    return;
                }
            }
            DialogueAction(index);
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

    IEnumerator ShowTutorial()
    {
        HideEmotion(rightEmotionScript);
        isTypingEnd = false;
        tutorialObject.transform.DOScale(Vector3.one, tutorialDuration);
        yield return null;
        isTypingEnd = true;
    }

    public void PoppingSptite(GameObject _sprite)
    {
        Vector3 originalPosition = _sprite.transform.position;
        Vector3 poppingVector = new Vector2(0, 0.3f);
        _sprite.transform.DOMove(originalPosition + poppingVector, 0.1f).SetLoops(2, LoopType.Yoyo).OnKill(() => _sprite.transform.position = originalPosition);
    }

    public void HighlightSprite(SpriteRenderer _renderer)
    {
        _renderer.color = highlightColor;
    }

    public void DehighlightSprite(SpriteRenderer _renderer)
    {
        _renderer.color = dehighlightColor;
    }
    public void WaveEmotion(GameObject _emotion)
    {
        DOTween.Kill(_emotion.transform);

        Vector3 originalPosition = _emotion.transform.position;
        Vector3 start = _emotion.transform.position;
        Vector3 end = _emotion.transform.position + new Vector3(0.2f,0,0);

        // 두 점 사이의 중간 지점을 계산합니다.
        Vector3 middlePoint = _emotion.transform.position + new Vector3(0.1f, 0.2f, 0);

        // 부채꼴 곡선을 정의합니다.
        Vector3[] path = new Vector3[] { start, middlePoint, end };

        // 부채꼴 곡선을 따라 오브젝트를 이동시킵니다.
        _emotion.transform.DOPath(path, 2f, PathType.CatmullRom)
            .SetEase(Ease.InOutQuad) // 이동 속도를 조절할 수 있습니다.
            .SetOptions(true) // 루프 옵션을 설정합니다.
            .SetLoops(-1, LoopType.Yoyo) // 앞뒤로 반복하도록 설정합니다.
            .OnKill(() => _emotion.transform.position = originalPosition);
    }

    public void ShowEmotion(DialogueEmotion _emotionScript, string _emotionName)
    {
        _emotionScript.ShowEmotion(_emotionName);
    }

    public void ShowEmotion(DialogueEmotion _emotionScript, int _emotionIndex)
    {
        _emotionScript.ShowEmotion(_emotionIndex);
    }

    public void HideEmotion(DialogueEmotion _emotionScript)
    {
        _emotionScript.HideEmotion();
    }

    public void PlayAudioClipsIndex(int _index)
    {
        if (sfxPlayer == null) return;
        if (sfxPlayer.isPlaying) sfxPlayer.Stop();
        sfxPlayer.clip = sfxAudioClips[_index];
        sfxPlayer.Play();
    }

    public void DialogueAction(int _Index)
    {
        if (dialogueIndex == 0) // 다이얼로그 0 액션
        {
            switch (_Index)
            {
                case 0:
                    // PlayAudioClipsIndex(0); // SFX나 클립을 넣을 수 있습니다.
                    ShowEmotion(leftEmotionScript, "angry"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;
                case 1:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, 12); // 인덱스로도 가능합니다. talking
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
                case 2:
                    HideEmotion(rightEmotionScript);

                    ShowEmotion(leftEmotionScript, "curious");
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;
                case 3:
                    HideEmotion(leftEmotionScript);

                    ShowEmotion(rightEmotionScript, "brightened");
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
                case 4:
                    HideEmotion(rightEmotionScript);

                    ShowEmotion(leftEmotionScript, 9); //sad
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;
                case 5:
                    HideEmotion(leftEmotionScript);

                    ShowEmotion(rightEmotionScript, "talking");
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
            }
        }
        else if (dialogueIndex == 1) // 다이얼로그 1 액션
        {
            switch (_Index)
            {
                case 0:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 인덱스로도 가능합니다. talking
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                case 1:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(leftEmotionScript, "happy"); // 이름으로도 가능합니다.
                    ShowEmotion(leftEmotionScript, "happy2"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;

                case 2:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                case 3:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(leftEmotionScript, "curious"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;


                case 4:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                //좌 감정표현+좌 스프라이트 강조 (우 감표 삭제, 어둡게)
                case 5:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(leftEmotionScript, "brightened"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;

                //좌 감정표현+좌 스프라이트 강조 (우 감표 삭제, 어둡게)
                case 6:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(leftEmotionScript, "ashamed"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;

                //우 감정표현+우 스프라이트 강조 (좌 감표 삭제, 어둡게)
                case 7:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
                //좌 감정표현+좌 스프라이트 강조 (우 감표 삭제, 어둡게)
                case 8:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(leftEmotionScript, "hmmm"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;
                //좌 감정표현+좌 스프라이트 강조 (좌 감표 삭제)
                case 9:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(leftEmotionScript, "brightened"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;
            }
        }
        else if (dialogueIndex == 2) // 다이얼로그 2 액션
        {
            switch (_Index)
            {
                case 0:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    DehighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                case 1:
                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                case 2:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
                //좌 감정표현+좌 스프라이트 강조 (우 감표 삭제, 어둡게)
                case 3:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.
                    
                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
                //좌 감정표현+좌 스프라이트 강조 (우 감표 삭제, 어둡게)
                case 4:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                case 5:
                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    DehighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
                //좌 감정표현+좌 스프라이트 강조 (우 감표 삭제, 어둡
                case 6:
                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;

                //우 감정표현+우 스프라이트 강조 (좌 감표 삭제, 어둡게)
                case 7:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "brightened"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                //우 감정표현+우 스프라이트 강조 (좌 감표 삭제, 어둡게)
                case 8:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
            }
        }
        else if (dialogueIndex == 3) // 다이얼로그 3 액션
        {
            switch (_Index)
            {
                case 0:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.
                   
                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    DehighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                case 1:
                    ShowEmotion(leftEmotionScript, "sleepy"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;
                case 2:
                    HideEmotion(leftEmotionScript);

                    ShowEmotion(leftEmotionScript, "shock"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;

                //우 감정표현+우 스프라이트 강조 (좌 감표 삭제, 어둡게)
                case 3:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                //우 감정표현+우 스프라이트 강조 (좌 감표 삭제, 어둡게)
                case 4:
                    HideEmotion(leftEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;

                //우 감정표현+우 스프라이트 강조 (좌 감표 삭제, 어둡게)
                case 5:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
                //우 감정표현+우 스프라이트 강조 (좌 감표 삭제, 어둡게)
                case 6:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(rightEmotionScript, "talking"); // 이름으로도 가능합니다.
                    WaveEmotion(rightEmotion);

                    PoppingSptite(samuraiSprite);
                    HighlightSprite(samuraiRenderer);
                    DehighlightSprite(knightRenderer);
                    break;
                //우 감정표현+우 스프라이트 강조 (좌 감표 삭제, 어둡게)
                case 7:
                    HideEmotion(rightEmotionScript); // 감정을 숨기지 않으면 계속보입니다.

                    ShowEmotion(leftEmotionScript, "brightened"); // 이름으로도 가능합니다.
                    WaveEmotion(leftEmotion);

                    PoppingSptite(knightSprite);
                    HighlightSprite(knightRenderer);
                    DehighlightSprite(samuraiRenderer);
                    break;
            }
        }
        else if (dialogueIndex == 4) // 다이얼로그 4 액션
        {
            switch (_Index)
            {
            }
        }
        else if (dialogueIndex == 5) // 다이얼로그 5 액션
        {
            switch (_Index)
            {
            }
        }
    }
}
