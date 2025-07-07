using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class PlayerControl : MonoBehaviour
{
    //����    Spawner_Line_L
    //public Transform spawner_Line_L;
    //�߾�    Spawner_Line_M
    //public Transform spawner_Line_M;
    //������  Spawner_Line_R
    //public Transform spawner_Line_R;

    //���� ����, 0:����, 1:�߾�, 2:������
    [SerializeField] public int line = 0;
    public GameObject[] lineLights;

    private List<GameObject> nearbyObjects = new List<GameObject>(); // �ֺ��� ��� ������Ʈ�� Transform
    private GameObject nearestObject; // ���� ����� ������Ʈ

    private bool needLineCheck = true;

    [SerializeField] private bool isFever = false;
    [SerializeField] private bool isFeverReady = true;
    public float fevertime = 6;
    private float fevertimer = 0;

    public GameObject judgeLine;

    private void Start()
    {
        SpawnManager.Instance.player = gameObject;
        StagePlayManager.Instance.player = gameObject;
        line = 1;
        LineLightOn();

        //Spawner �Ҵ�
        /*if (spawner_Line_L == null)
        {
            spawner_Line_L = GameObject.FindWithTag("Spawner_Line_L").GetComponent<Transform>();
            Debug.Log("Spawner_Line_L ������Ʈ�� ��ũ���� �ʾҽ��ϴ�.");
        }
        if (spawner_Line_M == null)
        {
            spawner_Line_M = GameObject.FindWithTag("Spawner_Line_M").GetComponent<Transform>();
            Debug.Log("Spawner_Line_M ������Ʈ�� ��ũ���� �ʾҽ��ϴ�.");
        }
        if (spawner_Line_R == null)
        {
            spawner_Line_R = GameObject.FindWithTag("Spawner_Line_R").GetComponent<Transform>();
            Debug.Log("Spawner_Line_R ������Ʈ�� ��ũ���� �ʾҽ��ϴ�.");
        }*/
    }

    private void Update()
    {
        if (Time.timeScale == 0 || !StagePlayManager.Instance.isControlable)
        {
            return;
        }
        if (Input.GetAxis("Middle") != 0)
        {
            line = 1;
            LineLightOn();
            FindNearestObject();
        }
        else if (Input.GetAxis("LR") != 0)
        {
            if (Input.GetAxis("LR") < 0)
            {
                line = 0;
                LineLightOn();
                FindNearestObject();
            }
            else
            {
                line = 2;
                LineLightOn();
                FindNearestObject();
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            if(nearestObject != null)
            {
                HitTarget(nearestObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (nearestObject != null)
            {
                ParryTarget(nearestObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            if(isFeverReady == true)
            {
                Debug.Log("Fever");
                isFeverReady = false;
                needLineCheck = false;
                isFever = true;
            }
        }

        if (isFever)
        {
            fevertimer += Time.deltaTime;
            if (fevertimer >= fevertime)
            {
                Debug.Log("Fever End");
                isFever = false;
                needLineCheck = true;
                fevertimer = 0;
            }
        }
    }

    private void LineLightOn()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == line)
            {
                lineLights[i].SetActive(true);
            }
            else
            {
                lineLights[i].SetActive(false);
            }
        }
    }

    private void HitTarget(GameObject target)
    {
        int type = target.GetComponent<EnemyState>().type;
        if (type % 3 == 0)
        {
            target.GetComponent<MeleeEnemy>().Hit();
            FindNearestObject();
        }
        else if (type % 3 == 1)
        {
            target.GetComponent<ArmoredEnemy>().Hit();
            FindNearestObject();
        }
        else if (type % 3 == 2)
        {
            target.GetComponent<RangedEnemy>().Hit();
            FindNearestObject();
        }
    }

    private void ParryTarget(GameObject target)
    {
        int type = target.GetComponent<EnemyState>().type;
        if (type % 3 == 0)
        {
            target.GetComponent<MeleeEnemy>().Parry();
            FindNearestObject();
        }
        else if (type % 3 == 1)
        {
            target.GetComponent<ArmoredEnemy>().Parry();
            FindNearestObject();
        }
        else if (type % 3 == 2)
        {
            target.GetComponent<RangedEnemy>().Parry();
            FindNearestObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        nearbyObjects.Add(other.gameObject); // ���� ������Ʈ�� Transform�� ����Ʈ�� �߰�
        FindNearestObject(); // ���� ����� ������Ʈ�� ã��
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        nearbyObjects.Remove(other.gameObject); // ���� ������Ʈ�� Transform�� ����Ʈ���� ����
        FindNearestObject(); // ���� ����� ������Ʈ�� ã��
    }

    private void FindNearestObject()
    {
        float minDistance = Mathf.Infinity;
        nearestObject = null;
        
        foreach (GameObject obj in nearbyObjects)
        {
            EnemyState enemyState = obj.GetComponent<EnemyState>();

            //���� üũ
            if(needLineCheck && line != enemyState.line) { continue; }

            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestObject = obj;
            }
        }
    }
}
