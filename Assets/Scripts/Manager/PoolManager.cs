using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;

    // 외부에서 접근 가능한 인스턴스를 반환하는 프로퍼티
    public static PoolManager Instance
    {
        get
        {
            // 인스턴스가 없으면 새로 생성
            if (_instance == null)
            {
                // 씬에서 싱글턴 오브젝트를 찾음
                _instance = FindObjectOfType<PoolManager>();

                // 씬에 싱글턴 오브젝트가 없으면 새로 생성하여 추가
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(PoolManager).Name);
                    _instance = singletonObject.AddComponent<PoolManager>();
                }
            }
            return _instance;
        }
    }

    // 싱글턴 인스턴스가 유일하도록 보장
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
        // 풀 사이즈만큼 오브젝트를 생성하여 풀에 추가합니다.
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

    // 풀에서 비활성화된 오브젝트를 반환합니다.
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

        // 만약 활성화된 오브젝트가 없으면, 새로운 오브젝트를 풀에 추가합니다.
        GameObject newObj = Instantiate(prefabs[prefabIndex], transform);
        newObj.SetActive(false);
        objectPool.Add(newObj);
        return newObj;
    }

    public void AllDestroy()
    {
        foreach (Transform child in transform)
        {
            // 자식을 비활성화합니다.
            child.gameObject.SetActive(false);
            DOTween.Kill(child);
        }

    }

}
