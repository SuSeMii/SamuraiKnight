using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Transform target;
    public Vector2 spawnPoint;
    public Vector2 actionPoint;
    public Tween tween;

    public GameObject parryingParticle;
    public GameObject attackParticle;

    private void Start()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
            Debug.Log("Player ������Ʈ�� ��ũ���� �ʾҽ��ϴ�.");
        }
        target = player.GetComponent<Transform>();
    }

    public void Die()
    {
        tween.Kill(false);
        gameObject.SetActive(false);
    }

    public void AttackPlayer()
    {
        tween.Kill(false);
        gameObject.SetActive(false);
        StagePlayManager.Instance.PlayerHit();
    }

    // public void Update()
    // {
    //     transform.Rotate(new Vector3(0, 0, Time.deltaTime * 360));
    // }

    public virtual int Hit() { return 0;}
    public virtual int Parry() { return 0; }

    protected virtual int HitAction() { return 0; }

    protected virtual int ParryAction() { return 0; }


    protected void ParriedParticle()
    {
        GameObject hitParticlesInstance = Instantiate(parryingParticle, transform.position + new Vector3(0,0,-1), Quaternion.identity);
        ParticleSystem ps = hitParticlesInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
        Destroy(ps.gameObject, ps.main.duration);
    }
    protected void AttackedParticle()
    {
        GameObject hitParticlesInstance = Instantiate(attackParticle, transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        ParticleSystem ps = hitParticlesInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }
        Destroy(ps.gameObject, ps.main.duration);
    }
}
