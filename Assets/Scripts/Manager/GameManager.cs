using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ��� ������ ���� ����
    private static GameManager _instance;

    // �ܺο��� ���� ������ �ν��Ͻ��� ��ȯ�ϴ� ������Ƽ
    public static GameManager Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ���� ����
            if (_instance == null)
            {
                // ������ �̱��� ������Ʈ�� ã��
                _instance = FindObjectOfType<GameManager>();

                // ���� �̱��� ������Ʈ�� ������ ���� �����Ͽ� �߰�
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                    _instance = singletonObject.AddComponent<GameManager>();
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

    public StageDataSO stageDataSO;
    public string nowScenePath = "";
    public int dialogueIndex;
    public int playMode; // 0: story play, 1: challenge stage play
    public int storyIndex;

    public GameObject maskEffect;
    public GameObject blackScreen;
    public GameObject maskCircle;
    public GameObject maskSquare;
    public float maskEffectDuration = 0.3f;

    Coroutine stageCor;

    private void Start()
    {
        LoadMainMenuScene();
    }

    public void StopStageCor()
    {
        if (stageCor != null)
        {
            StopCoroutine(stageCor);
            stageCor = null;
        }
        SpawnManager.Instance.StopSpawnOnMusic();
    }

    public void LoadMainMenuScene()
    {
        StopStageCor();
        if (nowScenePath.Length > 0)
        {
            SceneManager.UnloadSceneAsync(nowScenePath);
        }
        SceneManager.LoadSceneAsync("Scenes/MainMenu", LoadSceneMode.Additive);
        nowScenePath = "Scenes/MainMenu";
        SceneIn();
    }


    public void LoadIntroScene()
    {
        SceneManager.UnloadSceneAsync(nowScenePath);
        SceneManager.LoadSceneAsync("Scenes/AnimationScene", LoadSceneMode.Additive);
        nowScenePath = "Scenes/AnimationScene";
        //SceneIn();
    }

    public void LoadDialogueScene(int _dialogueIndex)
    {
        StopStageCor();
        SceneManager.UnloadSceneAsync(nowScenePath);
        SceneManager.LoadSceneAsync("Scenes/DialogueScene "+ _dialogueIndex, LoadSceneMode.Additive);
        nowScenePath = "Scenes/DialogueScene " + _dialogueIndex;
        dialogueIndex = _dialogueIndex;
        SceneInSquare();
    }

    public void LoadSpriteScene(int _spriteSceneIndex)
    {
        StopStageCor();
        SceneManager.UnloadSceneAsync(nowScenePath);
        SceneManager.LoadSceneAsync("Scenes/SpriteScene " + _spriteSceneIndex, LoadSceneMode.Additive);
        nowScenePath = "Scenes/SpriteScene " + _spriteSceneIndex;
        dialogueIndex = _spriteSceneIndex;
        SceneInSquare();
    }

    public void LoadStageSelectScene()
    {
        StopStageCor();
        AudioManager.Instance.StopCor();

        SceneManager.UnloadSceneAsync(nowScenePath);
        SceneManager.LoadSceneAsync("Scenes/StageSelectScene", LoadSceneMode.Additive);
        nowScenePath = "Scenes/StageSelectScene";
        SceneInSquare();
    }

    public void LoadStagePlayScene(int stageNumber)
    {
        StopStageCor();
        AudioManager.Instance.StopCor();

        SceneManager.UnloadSceneAsync(nowScenePath);
        SceneManager.LoadSceneAsync("Scenes/StagePlayScene", LoadSceneMode.Additive);
        nowScenePath = "Scenes/StagePlayScene";

        PoolManager.Instance.AllDestroy();

        //음악 셋팅
        AudioManager.Instance.audioSource.clip = stageDataSO.stages[stageNumber].music;
        AudioManager.Instance.latencyTime = stageDataSO.stages[stageNumber].musicPlayLatencyTime;

        //스폰 실행 (도중 음악 재생)
        SpawnManager.Instance.noteInfos = stageDataSO.stages[stageNumber].noteInfos;
        SpawnManager.Instance.noteArriveTime = stageDataSO.stages[stageNumber].noteArriveTime;

        //스테이지 상태값 셋팅
        StagePlayManager.Instance.SetStart();
        StagePlayManager.Instance.stageNumber = stageNumber;

        //3초 대기
        stageCor = StartCoroutine(timeFunction(3f, () => {
            // 레이턴시 적용 후 스폰 시작
            StartCoroutine(timeFunction(stageDataSO.stages[stageNumber].musicPlayLatencyTime, () => { 
                SpawnManager.Instance.StartSpawnOnMusic();
            }));
        }));
        SceneInCircle();
    }

    public IEnumerator timeFunction(float _time, Action _func)
    {
        yield return new WaitForSeconds(_time);
        _func();
    }

    public void SceneOutSquare()
    {
        maskSquare.transform.position = new Vector3(0, 0, 0);
        DOTween.Kill(maskSquare.transform);
        maskSquare.SetActive(true);
        maskCircle.SetActive(false);
        maskEffect.SetActive(true);
        maskSquare.transform.DOMove(new Vector3(20, 0, 0), maskEffectDuration);
    }

    public void SceneOutCircle()
    {
        maskCircle.transform.localScale = new Vector3(50, 50, 1);
        DOTween.Kill(maskCircle.transform);
        maskCircle.SetActive(true);
        maskSquare.SetActive(false);
        maskEffect.SetActive(true);
        maskCircle.transform.DOScale(new Vector3(0, 0, 1), maskEffectDuration);
    }

    public void SceneInSquare()
    {
        maskSquare.transform.position = new Vector3(-20, 0, 0);
        DOTween.Kill(maskSquare.transform);
        maskCircle.SetActive(false);
        maskSquare.SetActive(true);
        maskSquare.transform.DOMove(new Vector3(0, 0, 0), maskEffectDuration).OnComplete(() => { maskEffect.SetActive(false); }) ;
    }

    public void SceneInSquare(float _maskEffectDuration)
    {
        maskSquare.transform.position = new Vector3(-20, 0, 0);
        DOTween.Kill(maskSquare.transform);
        maskCircle.SetActive(false);
        maskSquare.SetActive(true);
        maskSquare.transform.DOMove(new Vector3(0, 0, 0), maskEffectDuration).OnComplete(() => { maskEffect.SetActive(false); });
    }

    public void SceneInCircle()
    {
        maskCircle.transform.localScale = new Vector3(0, 0, 1);
        DOTween.Kill(maskCircle.transform);
        maskSquare.SetActive(false);
        maskCircle.SetActive(true);
        maskCircle.transform.DOScale(new Vector3(50, 50, 1), maskEffectDuration).OnComplete(() => { maskEffect.SetActive(false); });
    }

    public void SceneInCircle(float _maskEffectDuration)
    {
        maskCircle.transform.localScale = new Vector3(0, 0, 1);
        DOTween.Kill(maskCircle.transform);
        maskSquare.SetActive(false);
        maskCircle.SetActive(true);
        maskCircle.transform.DOScale(new Vector3(50, 50, 1), _maskEffectDuration).OnComplete(() => { maskEffect.SetActive(false); });
    }

    public void SceneIn()
    {
        maskEffect.SetActive(false);
    }

    public void LoadNextStory()
    {
        switch (storyIndex){
            case 0:
                LoadDialogueScene(0);
                break;
            case 1:
                LoadStagePlayScene(0);
                break;
            case 2:
                LoadDialogueScene(1);
                break;
            case 3:
                LoadDialogueScene(2);
                break;
            case 4:
                LoadDialogueScene(3);
                break;
            case 5:
                LoadSpriteScene(0);
                break;
            case 6:
                LoadSpriteScene(5);
                break;
            case 7:
                LoadDialogueScene(4);
                break;
            case 8:
                LoadStagePlayScene(1);
                break;
            case 9:
                LoadDialogueScene(5);
                break;
            case 10:
                LoadSpriteScene(1);
                break;
            case 11:
                LoadSpriteScene(6);
                break;
            case 12:
                LoadDialogueScene(6);
                break;
            case 13:
                LoadStagePlayScene(2);
                break;
            case 14:
                LoadDialogueScene(7);
                break;
            case 15:
                LoadSpriteScene(2);
                break;
            case 16:
                LoadSpriteScene(7);
                break;
            case 17:
                LoadDialogueScene(8);
                break;
            case 18:
                LoadStagePlayScene(3);
                break;
            case 19:
                LoadDialogueScene(9);
                break;
            case 20:
                LoadSpriteScene(3);
                break;
            case 21:
                LoadSpriteScene(8);
                break;
            case 22:
                LoadDialogueScene(10);
                break;
            case 23:
                LoadStagePlayScene(4);
                break;
            /*case 24:
                LoadDialogueScene(11);
                break;*/
            case 24:
                LoadSpriteScene(4);
                break;
            case 25:
                LoadSpriteScene(9);
                break;
            case 26:
                LoadDialogueScene(11);
                break;
            case 27:
                LoadMainMenuScene();
                break;
        }
        storyIndex++;
    }
}