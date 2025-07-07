using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public GameObject attackEffect;
    public void Update()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * 360));
    }
    
    public override int Hit()
    {
        StagePlayManager.Instance.JudgeNote(gameObject);
        StagePlayManager.Instance.PlaySound("Attack");
        Die();
        Instantiate(attackEffect, transform.position, Quaternion.identity);
        AttackedParticle();
        return 1;
    }
}
