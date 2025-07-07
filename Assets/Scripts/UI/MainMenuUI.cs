using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public TextMeshProUGUI[] texts;
    public int currentButton = 0;

    private bool isSceneOutable;

    void Start()
    {
        isSceneOutable = true;
        for (int i = 0; i < texts.Length; i++)
        {
            if(i == currentButton)
                ChangeWhite(i);
            else
                ChangeGray(i);
        }
    }
    
    void Update()
    {
        if (!isSceneOutable) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
        {
            if(currentButton != 0)
            {
                currentButton--;
                ChangeColor();
            }
        }

        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
        {
            if(currentButton != 2)
            {
                currentButton++;
                ChangeColor();
            }
        }

        if(Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Return))
        {
            switch(currentButton)
            {
                case 0:
                    OnStartButtonClicked();
                    break;
                case 1:
                    OnChallengeStageButtonClicked();
                    break;
                case 2:
                    OnExitButtonClicked();
                    break;
            }
        }
    }
    public void OnStartButtonClicked()
    {
        if (!isSceneOutable) return;
        isSceneOutable = false;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.GameStart);
        GameManager.Instance.playMode = 0;
        GameManager.Instance.storyIndex = 0;
        GameManager.Instance.SceneOutSquare();
        StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, () =>
        {
            GameManager.Instance.LoadIntroScene();
        }));
    }

    

    public void OnChallengeStageButtonClicked()
    {
        if (!isSceneOutable) return;
        isSceneOutable = false;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.GameStart);
        GameManager.Instance.playMode = 1;
        GameManager.Instance.SceneOutSquare();
        StartCoroutine(GameManager.Instance.timeFunction(GameManager.Instance.maskEffectDuration, () =>
        {
            GameManager.Instance.LoadStagePlayScene(5);
        }));
    }

    public void OnExitButtonClicked()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.ButtonConfirm);
        Application.Quit();
    }

    void ChangeWhite(int index)
    {
        texts[index].color = Color.white;
    }

    void ChangeGray(int index)
    {
        texts[index].color = new Color32(161,161,161,255);
    }

    public void ChangeColor()
    {
        for(int i=0; i<texts.Length; i++)
        {
            if(i == currentButton)
            {
                ChangeWhite(i);
            }
            else
            {
                ChangeGray(i);
            }
        }
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.ButtonChange);
    }

    public void MouseEnter(int index)
    {
        currentButton = index;
        ChangeColor();
    }
}
