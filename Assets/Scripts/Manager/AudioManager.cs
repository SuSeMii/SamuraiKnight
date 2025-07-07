using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

// using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    // �ܺο��� ���� ������ �ν��Ͻ��� ��ȯ�ϴ� ������Ƽ
    public static AudioManager Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ���� ����
            if (_instance == null)
            {
                // ������ �̱��� ������Ʈ�� ã��
                _instance = FindObjectOfType<AudioManager>();

                // ���� �̱��� ������Ʈ�� ������ ���� �����Ͽ� �߰�
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(AudioManager).Name);
                    _instance = singletonObject.AddComponent<AudioManager>();
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

    public AudioSource audioSource;
    public float latencyTime = 0f;
    public Coroutine cor;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StopCor()
    {
        if(cor == null)
            return;

        StopCoroutine(cor);
        PauseMusic();
    }
    public void PlayMusicWithLatencyTime(float noteArriveTime)
    {
        cor = StartCoroutine(GameManager.Instance.timeFunction(noteArriveTime, audioSource.Play));
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
    public void PlayMusic()
    {
        audioSource.Play();
    }
    
    public void PauseMusic()
    {
        audioSource.Pause();
    }
    public void UnPauseMusic()
    {
        audioSource.UnPause();
    }
}
