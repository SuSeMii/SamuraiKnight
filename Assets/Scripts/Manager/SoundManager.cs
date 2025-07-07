using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ��� ������ ���� ����
    private static SoundManager _instance;

    // �ܺο��� ���� ������ �ν��Ͻ��� ��ȯ�ϴ� ������Ƽ
    public static SoundManager Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ���� ����
            if (_instance == null)
            {
                // ������ �̱��� ������Ʈ�� ã��
                _instance = FindObjectOfType<SoundManager>();

                // ���� �̱��� ������Ʈ�� ������ ���� �����Ͽ� �߰�
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SoundManager).Name);
                    _instance = singletonObject.AddComponent<SoundManager>();
                }
            }
            return _instance;
        }
    }

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Bgm {}

    public enum Sfx {ButtonChange, ButtonConfirm, StageClear, GameStart, InGameButtonChange}

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

        Init();
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("Bgm");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("Sfx");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int index=0; index<sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int i=0; i<sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }
            
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    // public void PlaySound(string soundPath)
    // {
    //     // ���� ���� �ε�
    //     AudioClip clip = Resources.Load<AudioClip>(soundPath);
    //     if (clip != null)
    //     {
    //         // AudioSource ������Ʈ�� ã�Ƽ� ���� ���
    //         AudioSource audioSource = GetComponent<AudioSource>();
    //         if (audioSource != null)
    //         {
    //             audioSource.clip = clip;
    //             audioSource.Play();
    //         }
    //         else
    //         {
    //             Debug.LogError("AudioSource ������Ʈ�� ã�� �� �����ϴ�.");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("���� ������ ã�� �� �����ϴ�.");
    //     }
    // }
}
