using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics.Contracts;

public class SpawnManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ��� ������ ���� ����
    private static SpawnManager _instance;

    // �ܺο��� ���� ������ �ν��Ͻ��� ��ȯ�ϴ� ������Ƽ
    public static SpawnManager Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ���� ����
            if (_instance == null)
            {
                // ������ �̱��� ������Ʈ�� ã��
                _instance = FindObjectOfType<SpawnManager>();

                // ���� �̱��� ������Ʈ�� ������ ���� �����Ͽ� �߰�
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(SpawnManager).Name);
                    _instance = singletonObject.AddComponent<SpawnManager>();
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

    public PoolManager poolManager;
    public GameObject player;
    public List<NoteInfo> noteInfos;

    public float noteArriveTime = 2.0f;

    public float spawnDelay = 2.0f;
    public float time = 0.0f;

    public Coroutine noteSpawnCor;
    public bool isPlaying = false;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        /*if (time >= spawnDelay && isPlaying)
        {
            time = 0;
            SpawnEnemy(Random.Range(0, 3), Random.Range(0, 3));
        }*/
    }

    public void StartSpawnOnMusic()
    {
        noteSpawnCor = StartCoroutine(SpawnEnemyCoroutine());
    }

    public void StopSpawnOnMusic()
    {
        if (noteSpawnCor != null)
        {
            StopCoroutine(noteSpawnCor);
            noteSpawnCor = null;
        }
    }

    public void SpawnEnemy(int prefabIndex, int lineIndex)
    {
        Vector2 spawnPoint = Vector2.zero;
        Vector2 actionPoint = Vector2.zero;
        switch (lineIndex)
        {
            case 0:
                spawnPoint = new Vector2(-10, 30);
                actionPoint = new Vector2(-5, 15);
                break;
            case 1:
                spawnPoint = new Vector2(0, 30);
                actionPoint = new Vector2(0, 15);
                break;
            case 2:
                spawnPoint = new Vector2(10, 30);
                actionPoint = new Vector2(5, 15);
                break;
        }

        GameObject obj = poolManager.getObjectPool(prefabIndex);
        obj.GetComponent<EnemyState>().type = prefabIndex;
        obj.GetComponent<EnemyState>().line = lineIndex;

        obj.transform.position = spawnPoint;
        if (prefabIndex % 3 == 0)
        {
            MeleeEnemy enemy = obj.GetComponent<MeleeEnemy>();
            enemy.player = player;
            enemy.target = player.transform;
            enemy.spawnPoint = spawnPoint;
            enemy.actionPoint = actionPoint;
            obj.SetActive(true);
            enemy.tween = obj.transform.DOMove(player.transform.position, noteArriveTime).SetEase(Ease.Linear).OnComplete(enemy.AttackPlayer);
        } 
        else if (prefabIndex % 3 == 1)
        {
            ArmoredEnemy enemy = obj.GetComponent<ArmoredEnemy>();
            enemy.player = player;
            enemy.target = player.transform;
            enemy.hp = 2;
            enemy.spawnPoint = spawnPoint;
            enemy.actionPoint = actionPoint;
            obj.SetActive(true);
            enemy.tween = obj.transform.DOMove(player.transform.position, noteArriveTime).SetEase(Ease.Linear).OnComplete(enemy.AttackPlayer);
        } 
        else if (prefabIndex % 3 == 2)
        {
            RangedEnemy enemy = obj.GetComponent<RangedEnemy>();
            enemy.player = player;
            enemy.target = player.transform;
            enemy.spawnPoint = spawnPoint;
            enemy.actionPoint = actionPoint;
            obj.SetActive(true);
            enemy.tween = obj.transform.DOMove(actionPoint, noteArriveTime/2)
                .SetEase(Ease.Linear)
                .OnComplete(enemy.Shot);
        }
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        AudioManager.Instance.PlayMusicWithLatencyTime(noteArriveTime);

        float i = 0;
        int currentIndex = 0;
        while (currentIndex < noteInfos.Count)
        {
            while (currentIndex < noteInfos.Count && noteInfos[currentIndex].hitTime - noteArriveTime <= i){
                SpawnEnemy(noteInfos[currentIndex].type, noteInfos[currentIndex].line);
                currentIndex++;
            }
            
            yield return new WaitForSeconds(0.1f);
            i += 0.1f + Time.deltaTime;
            time = i;
        }
        noteSpawnCor = null;
        StagePlayManager.Instance.Noteover();
        yield break;
    }
}
