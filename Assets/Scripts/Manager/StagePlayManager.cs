using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DG.Tweening;
using TMPro;
// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Handles;



public class StagePlayManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ��� ������ ���� ����
    private static StagePlayManager _instance;
    

    // �ܺο��� ���� ������ �ν��Ͻ��� ��ȯ�ϴ� ������Ƽ
    public static StagePlayManager Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ���� ����
            if (_instance == null)
            {
                // ������ �̱��� ������Ʈ�� ã��
                _instance = FindObjectOfType<StagePlayManager>();

                // ���� �̱��� ������Ʈ�� ������ ���� �����Ͽ� �߰�
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(StagePlayManager).Name);
                    _instance = singletonObject.AddComponent<StagePlayManager>();
                }
            }
            return _instance;
        }
    }

    // �̱��� �ν��Ͻ��� �����ϵ��� ����
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public PlayerAnimation playerAnimation;
    public int stageNumber = 0;

    public int score;
    public int maxScore;
    public int combo;
    public int maxCombo;
    public int totalCount;
    public int maxNoteCount;

    public int perfectCount;
    public int greatCount;
    public int goodCount;
    public int badCount;

    public TextMeshPro text;

    public int perfectScore;
    public int greatScore;
    public int goodScore;
    public int badScore;

    //��ũ S A B C D E F
    public float rankSSRatio;
    public float rankSRatio;
    public float rankARatio;
    public float rankBRatio;
    public float rankCRatio;
    public float rankDRatio;

    public float judgeDistance;
    public float toleranceRange;

    // ���� ���� (�����̸� �ش� ����)
    public float perfectRatio;
    public float greatRatio;
    public float goodRatio;

    public int hp = 3;
    public string rank = "D";

    public StageUI stageUI;
    public GameObject player;

    public bool isPlaying = false;
    private float scoreTime = 0.0f;

    public GameObject judgeEffect;

    public AudioClip parryingSound;
    public AudioClip attackSound;
    public AudioClip playerHitSound;

    public float stage4EndTime;
    private float stage4PlayTime;

    public bool isControlable = true;

    private void Update()
    {
        if (isPlaying)
        {
            scoreTime += Time.deltaTime;
            if (scoreTime > 0.1f)
            {
                scoreTime -= 0.1f;
                switch (rank)
                {
                    case "SSS":
                        score += 40;
                        break;
                    case "SS":
                        score += 40;
                        break;
                    case "S":
                        score += 40;
                        break;
                    case "A":
                        score += 30;
                        break;
                    case "B":
                        score += 20;
                        break;
                    case "C":
                        score += 10;
                        break;
                    case "D":
                        score += 5;
                        break;
                }
                stageUI.UpdateStageUI();
            }
        }

        if (stageNumber == 4)
        {
            stage4PlayTime += Time.deltaTime;
            if (stage4PlayTime > stage4EndTime)
            {
                stage4PlayTime = 0;
                stageNumber = 0;
                isPlaying = false;
                Time.timeScale = 1;
                PoolManager.Instance.AllDestroy();
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.StopCor();
                SpawnManager.Instance.StopAllCoroutines();
                GameManager.Instance.SceneOutCircle();
                StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, () =>
                {
                    GameManager.Instance.LoadNextStory();
                }));
                isControlable = true;
            }
        }
    }

    public void SetStart(){
        score = 0;
        combo = 0;
        maxCombo = 0;
        totalCount = 0;

        perfectCount = 0;
        greatCount = 0;
        goodCount = 0;
        badCount = 0;
        hp = 3;
        rank = "D";
        isPlaying = true;
        stage4PlayTime = 0;

        maxNoteCount = SpawnManager.Instance.noteInfos.Count;
        maxScore = maxNoteCount * perfectScore;
    }

    public void updateStageInfo(int _flag)
    {
        switch (_flag)
        {
            case 0:
                score += perfectScore;
                combo++;
                totalCount++;
                break;
            case 1:
                score += greatScore;
                combo++;
                totalCount++;
                break;
            case 2:
                score += goodScore;
                combo++;
                totalCount++;
                break;
            case 3:
                score += badScore;
                combo = 0;
                break;
        }

        // ���� UI ����
        if(combo > maxCombo)
        {
            maxCombo = combo;
        }

        UpdateRank();

        stageUI.UpdateStageUI();

        // HP ����
        if (_flag == 3)
        {
            PlayerLoseLife();
        }
    }

    public void JudgeText(JudgeType type){
        GameObject je = Instantiate(judgeEffect, Vector3.zero, Quaternion.identity);
        je.GetComponent<JudgeEffect>().ShowGrade(type);
    }
    public void JudgeNote(GameObject note)
    {
        float hitDistance = Math.Abs(judgeDistance - note.transform.position.y - player.transform.position.y);
        Debug.Log("hitDistance Y: " + hitDistance);
        if (hitDistance < toleranceRange * perfectRatio)
        {
            //판정 인식
            // ����
            Debug.Log("Perfect");
            JudgeText(JudgeType.PERFECT);
            updateStageInfo(0);
        }
        else if (hitDistance < toleranceRange * greatRatio)
        {
            Debug.Log("Great");
            JudgeText(JudgeType.GREAT);
            updateStageInfo(1);
        }
        else if (hitDistance < toleranceRange * goodRatio)
        {
            Debug.Log("Good");
            JudgeText(JudgeType.GOOD);
            updateStageInfo(2);
        }
        else
        {
            Debug.Log("Bad");
            JudgeText(JudgeType.BAD);
            updateStageInfo(3);
        }
    }

    public void PlayerHit()
    {
        // Debug.Log("Player Hit!!!");
        updateStageInfo(3);
    }

    public void PlayerLoseLife()
    {
        hp--;
        AudioSource.PlayClipAtPoint(playerHitSound, Camera.main.transform.position);
        stageUI.UpdatePlayerHP();
        if (hp == 0)
        {
            isPlaying = false;
            playerAnimation.deathAnim();
            StartCoroutine(GameManager.Instance.timeFunction(1, stageUI.ShowStageFailUI));
        }
        else
        {
            playerAnimation.hurtsAnim();
        }
    }

    public void UpdateRank()
    {
        if(totalCount == maxNoteCount)
        {
            rank = "SSS";
            score += 10000;
        }
        /*else if (totalCount > maxNoteCount * rankSSRatio)
        {
            rank = "SS";
        }*/ // 보류
        else if (totalCount > maxNoteCount * rankSRatio)
        {
            rank = "S";
        }
        else if (totalCount > maxNoteCount * rankARatio)
        {
            rank = "A";
        }
        else if(totalCount > maxNoteCount * rankBRatio)
        {
            rank = "B";
        }
        else if(totalCount > maxNoteCount * rankCRatio)
        {
            rank = "C";
        }
        else
        {
            rank = "D";
        }
    }

    public void Noteover()
    {
        StartCoroutine(GameManager.Instance.timeFunction(3, GameClearCheck));
    }

    public void GameClearCheck()
    {
        if (hp > 0)
        {
            isPlaying = false;
            StartCoroutine(GameManager.Instance.timeFunction(1, stageUI.ShowStageClearUI));
        }
    }

    public void PlaySound(string _soundType)
    {
        switch (_soundType)
        {
            case "Parrying":
                AudioSource.PlayClipAtPoint(parryingSound, Camera.main.transform.position);
                break;
            case "Attack":
                AudioSource.PlayClipAtPoint(attackSound, Camera.main.transform.position);
                break;
        }
    }
}
