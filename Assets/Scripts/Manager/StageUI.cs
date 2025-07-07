using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class StageUI : MonoBehaviour
{
    public GameObject stagePlayUI;
    public GameObject pauseUI;
    public GameObject stageClearUI;
    public GameObject stageFailUI;

    public GameObject scoreObj;
    public GameObject rankObj;
    public GameObject hpObj;
    public GameObject explain;

    // StageManager stageManager;

    public TextMeshProUGUI scoreText;
    // public TextMeshProUGUI rankText;
    // public TextMeshProUGUI hpText;

    public SpriteRenderer rankImage;
    public Light2D rankLight;
    public Sprite[] rankSprites;
    public Image[] heartImages;
    public Sprite[] heartSprites;
    public float duration = 0.25f;

    public ParticleSystem hpLossParticle;

    private Coroutine rankIntensityCor;
    private string curentRank = "D";
    public float initialIntensity = 1.5f;
    public float targetIntensity = 0.5f;
    public float increaseDuration = 0.1f;
    public float decreaseDuration = 0.3f;

    private bool isSceneOutable = true;

    // Start is called before the first frame update
    void Start()
    {
        // stageManager = GameObject.Find("StageManager");
        StagePlayManager.Instance.stageUI = this;
        StagePlayManager.Instance.stageUI.UpdateStageUI();
        scoreText.text = "0";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseUI.activeSelf)
            {
                CloseUI();
            }
            else
            {
                if (!StagePlayManager.Instance.isControlable) return;
                OpenUI();
            }
        }
    }

    public void OpenUI()
    {
        // ���� �ð��� ����
        Time.timeScale = 0;
        StagePlayManager.Instance.isControlable = false;
        AudioManager.Instance.PauseMusic();
        // UI �г��� Ȱ��ȭ�Ͽ� ����
        pauseUI.SetActive(true);
    }

    public void CloseUI()
    {
        // ���� �ð��� �ٽ� ����
        Time.timeScale = 1;
        StagePlayManager.Instance.isControlable = true;
        AudioManager.Instance.UnPauseMusic();
        // UI �г��� ��Ȱ��ȭ�Ͽ� �ݱ�
        pauseUI.SetActive(false);
    }

    public void ShowStageClearUI()
    {
        // ���� �ð��� ����
        // Time.timeScale = 0;
        // UI �г��� Ȱ��ȭ�Ͽ� ����
        stageClearUI.SetActive(true);
        stagePlayUI.SetActive(false);
        explain.SetActive(false);
        StagePlayManager.Instance.isControlable = false;
    }

    public void ShowStageFailUI()
    {
        // ���� �ð��� ����
        // Time.timeScale = 0;
        // UI �г��� Ȱ��ȭ�Ͽ� ����
        AudioManager.Instance.PauseMusic();
        Time.timeScale = 0;
        SpawnManager.Instance.StopAllCoroutines();
        stageFailUI.SetActive(true);
        explain.SetActive(false);
        StagePlayManager.Instance.isControlable = false;
    }

    public void UpdateStageUI()
    {
        // ���� UI ����
        if(StagePlayManager.Instance.score == 0)
            scoreText.text = "0";
        else
            scoreText.text = string.Format("{0:#,###}",StagePlayManager.Instance.score);

        if (!curentRank.Equals(StagePlayManager.Instance.rank))
        {
            if (rankIntensityCor != null)
                StopCoroutine(rankIntensityCor);

            curentRank = StagePlayManager.Instance.rank;
            switch (StagePlayManager.Instance.rank)
            {
                case "SSS":
                    rankImage.sprite = rankSprites[6];
                    rankLight.lightCookieSprite = rankSprites[6];
                    rankImage.transform.localScale = new Vector3(1f, 1f, rankImage.transform.localScale.z);
                    rankLight.transform.localScale = new Vector3(1f, 1f, rankImage.transform.localScale.z);
                    targetIntensity = 0.4f;
                    break;
                case "SS":
                    rankImage.sprite = rankSprites[5];
                    rankLight.lightCookieSprite = rankSprites[5];
                    rankImage.transform.localScale = new Vector3(0.9f, 0.9f, rankImage.transform.localScale.z);
                    rankLight.transform.localScale = new Vector3(0.9f, 0.9f, rankImage.transform.localScale.z);
                    targetIntensity = 0.4f;
                    break;
                case "S":
                    rankImage.sprite = rankSprites[4];
                    rankLight.lightCookieSprite = rankSprites[4];
                    rankImage.transform.localScale = new Vector3(0.8f, 0.8f, rankImage.transform.localScale.z);
                    rankLight.transform.localScale = new Vector3(0.8f, 0.8f, rankImage.transform.localScale.z);
                    targetIntensity = 0.4f;
                    break;
                case "A":
                    rankImage.sprite = rankSprites[3];
                    rankLight.lightCookieSprite = rankSprites[3];
                    rankImage.transform.localScale = new Vector3(0.9f, 0.9f, rankImage.transform.localScale.z);
                    rankLight.transform.localScale = new Vector3(0.9f, 0.9f, rankImage.transform.localScale.z);
                    targetIntensity = 0.4f;
                    break;
                case "B":
                    rankImage.sprite = rankSprites[2];
                    rankLight.lightCookieSprite = rankSprites[2];
                    rankImage.transform.localScale = new Vector3(0.8f, 0.8f, rankImage.transform.localScale.z);
                    rankLight.transform.localScale = new Vector3(0.8f, 0.8f, rankImage.transform.localScale.z);
                    targetIntensity = 0.15f;
                    break;
                case "C":
                    rankImage.sprite = rankSprites[1];
                    rankLight.lightCookieSprite = rankSprites[1];
                    rankImage.transform.localScale = new Vector3(0.9f, 0.9f, rankImage.transform.localScale.z);
                    rankLight.transform.localScale = new Vector3(0.9f, 0.9f, rankImage.transform.localScale.z);
                    targetIntensity = 0.25f;
                    break;
                case "D":
                    rankImage.sprite = rankSprites[0];
                    rankLight.lightCookieSprite = rankSprites[0];
                    rankImage.transform.localScale = new Vector3(0.75f, 0.75f, rankImage.transform.localScale.z);
                    rankLight.transform.localScale = new Vector3(0.75f, 0.75f, rankImage.transform.localScale.z);
                    targetIntensity = 0.5f;
                    break;
            }

            rankIntensityCor = StartCoroutine(ChangeRankIntensity());

        }
        // rankText.text = "Rank: " + StagePlayManager.Instance.rank;
    }

    public void UpdatePlayerHP()
    {
        ParticleSystem particleSystem = Instantiate(hpLossParticle, heartImages[StagePlayManager.Instance.hp].transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        particleSystem.Play();
        Destroy(particleSystem.gameObject, particleSystem.main.duration);
        StartCoroutine(ChangeHeartSprite(heartImages[StagePlayManager.Instance.hp]));
    }

    public void GoStageSelect()
    {
        if (!isSceneOutable) return;
        isSceneOutable = false;
        Time.timeScale = 1;
        PoolManager.Instance.AllDestroy();
        AudioManager.Instance.StopMusic();
        SpawnManager.Instance.StopAllCoroutines();
        GameManager.Instance.SceneOutCircle();
        StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, () =>
        {
            GameManager.Instance.LoadStageSelectScene();
        }));
        StagePlayManager.Instance.isControlable = true;
    }

    public void NextButton()
    {
        int playMode = GameManager.Instance.playMode;
        if (!isSceneOutable) return;
        isSceneOutable = false;
        if (playMode == 0)
        {
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
            StagePlayManager.Instance.isControlable = true;
        }
        else if (playMode == 1)
        {
            GoMainMenu();
        }
    }

    public void GoMainMenu()
    {
        if (!isSceneOutable) return;
        isSceneOutable = false;
        Time.timeScale = 1;
        PoolManager.Instance.AllDestroy();
        AudioManager.Instance.StopMusic();
        SpawnManager.Instance.StopAllCoroutines();
        GameManager.Instance.SceneOutCircle();
        StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, () =>
        {
            GameManager.Instance.LoadMainMenuScene();
        }));
        StagePlayManager.Instance.isControlable = true;
    }

    public void RestartStage()
    {
        if (!isSceneOutable) return;
        isSceneOutable = false;
        Time.timeScale = 1;
        PoolManager.Instance.AllDestroy();
        AudioManager.Instance.StopMusic();
        SpawnManager.Instance.StopAllCoroutines();
        GameManager.Instance.SceneOutCircle();
        StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, () =>
        {
            GameManager.Instance.LoadStagePlayScene(StagePlayManager.Instance.stageNumber);
        }));
        StagePlayManager.Instance.isControlable = true;
    }

    IEnumerator ChangeHeartSprite(Image heartImage)
    {
        heartImage.sprite = heartSprites[0];
        yield return new WaitForSeconds(duration);
        heartImage.sprite = heartSprites[1];
        yield return null;
    }

    IEnumerator ChangeRankIntensity()
    {
        // 첫 번째 타겟 Intensity로 랭크를 증가시키는 동안 대기합니다.
        float elapsedTime = 0f;
        while (elapsedTime < increaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / increaseDuration;
            rankLight.intensity = Mathf.Lerp(targetIntensity, initialIntensity, t);
            yield return null;
        }

        // 두 번째 타겟 Intensity로 랭크를 감소시키는 동안 대기합니다.
        elapsedTime = 0f;
        while (elapsedTime < decreaseDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / decreaseDuration;
            rankLight.intensity = Mathf.Lerp(initialIntensity, targetIntensity, t);
            yield return null;
        }

        rankLight.intensity = targetIntensity;
        rankIntensityCor = null;
    }

}
