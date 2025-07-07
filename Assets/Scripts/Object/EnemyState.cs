using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public Sprite[] sprites;
    PlayerControl playerControl;
    //���� ����, 0:����, 1:�߾�, 2:������
    public int line = 0;

    // ���� Ÿ��, 0:�Ϲ�, 1:�߰�, 2:���Ÿ�, 3:����ü
    public int type = 0;

    void Start()
    {
        playerControl = GameObject.Find("PlayerCharacter").GetComponent<PlayerControl>();

    }
    void Update()
    {
        if(line == playerControl.line)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = sprites[0];
        }

        if(type % 3 == 1 && GetComponent<ArmoredEnemy>().hp == 1)
        {
            if(line == playerControl.line)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
        }
    }
}
