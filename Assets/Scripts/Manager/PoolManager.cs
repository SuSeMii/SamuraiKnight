using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;

    // �ܺο��� ���� ������ �ν��Ͻ��� ��ȯ�ϴ� ������Ƽ
    public static PoolManager Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ���� ����
            if (_instance == null)
            {
                // ������ �̱��� ������Ʈ�� ã��
                _instance = FindObjectOfType<PoolManager>();

                // ���� �̱��� ������Ʈ�� ������ ���� �����Ͽ� �߰�
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(PoolManager).Name);
                    _instance = singletonObject.AddComponent<PoolManager>();
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

    public GameObject[] prefabs;

    public int poolSize;

    private List<List<GameObject>> objectPools = new List<List<GameObject>>();


    void Start()
    {
        // Ǯ �����ŭ ������Ʈ�� �����Ͽ� Ǯ�� �߰��մϴ�.
        for (int i=0; i < prefabs.Length; i++)
        {
            GameObject prefab = prefabs[i];
            List<GameObject> objectPool = new List<GameObject>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject newObj = Instantiate(prefab, transform);
                newObj.SetActive(false);
                objectPool.Add(newObj);
            }

            objectPools.Add(objectPool);
        }
    }

    // Ǯ���� ��Ȱ��ȭ�� ������Ʈ�� ��ȯ�մϴ�.
    public GameObject getObjectPool(int prefabIndex)
    {
        List<GameObject> objectPool = objectPools[prefabIndex];
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }

        // ���� Ȱ��ȭ�� ������Ʈ�� ������, ���ο� ������Ʈ�� Ǯ�� �߰��մϴ�.
        GameObject newObj = Instantiate(prefabs[prefabIndex], transform);
        newObj.SetActive(false);
        objectPool.Add(newObj);
        return newObj;
    }

    public void AllDestroy()
    {
        foreach (Transform child in transform)
        {
            // �ڽ��� ��Ȱ��ȭ�մϴ�.
            child.gameObject.SetActive(false);
            DOTween.Kill(child);
        }

    }

}
