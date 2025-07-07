using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimationDirector : MonoBehaviour
{
    public PlayableDirector director;

    private bool isSceneOutable = true;

    private void Start()
    {
        StartCoroutine(BeginAfterDelay());
        director = GetComponent<PlayableDirector>();
        director.stopped += OnTimeLineEnd;

        GameManager.Instance.SceneIn();
        director.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            director.Stop();
            StopAllCoroutines();
            Time.timeScale = 1f;
        }
    }

    public void OnTimeLineEnd(PlayableDirector director)
    {
        if (isSceneOutable)
        {
            isSceneOutable = false;
            GameManager.Instance.LoadNextStory();
        }
    }

    IEnumerator BeginAfterDelay()
    {
        Time.timeScale = 0f;
        Debug.Log("게임이 일시 정지되었습니다.");

        // 실제 시간 기준으로 3초를 기다립니다.
        // 여기서는 Time.timeScale에 영향을 받지 않는 WaitForSecondsRealtime을 사용합니다.
        yield return new WaitForSecondsRealtime(3.5f);

        // 게임을 다시 시작합니다.
        Time.timeScale = 1f;
        Debug.Log("게임이 다시 시작되었습니다.");
    }
}
