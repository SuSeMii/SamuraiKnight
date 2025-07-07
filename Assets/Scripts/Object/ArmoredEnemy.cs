using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredEnemy : Enemy
{
    private EnemyState enemyState;
    bool isRun = true;
    public GameObject attackEffect;
    public GameObject[] parryingEffect;
    public GameObject[] parriedEffect;

    private GameObject parriedEffectObject;
    public bool isParriedEffectCreate = false;
    // public Animation anim;

    public int hp = 2;

    private void Awake()
    {
        enemyState = GetComponent<EnemyState>();
    }

    private void OnEnable()
    {
        isParriedEffectCreate = false;
    }
    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * 360));

        if(!isParriedEffectCreate)
        {
            if(hp == 1 && transform.position.y >= 14)
            {
                ParriedEffect();
            }
        }

        if(parriedEffectObject != null)
        {
            switch(enemyState.line)
            {
                case 0:
                    parriedEffectObject.transform.position = transform.position + new Vector3(-1, 1, 0.1f);
                break;

                case 1:
                    parriedEffectObject.transform.position = transform.position + new Vector3(0, 1, 0.1f);
                break;

                case 2:
                    parriedEffectObject.transform.position = transform.position + new Vector3(1f, 1.5f, 0.1f);
                    break;
            }
        }
    }

    public override int Hit()
    {
        if (isRun && hp <= 1)
        {
            //����
            StagePlayManager.Instance.JudgeNote(gameObject);
            StagePlayManager.Instance.PlaySound("Attack");
            Instantiate(attackEffect, transform.position, Quaternion.identity);
            AttackedParticle();
            Die();
        }

        return 1;
    }

    public override int Parry()
    {
        if (isRun && hp > 1)
        {
            isRun = false;
            hp--;
            StagePlayManager.Instance.PlaySound("Parrying");
            tween.Kill(false);
            Instantiate(parryingEffect[enemyState.line], transform.position, Quaternion.identity);
            ParriedParticle();
            tween = transform.DOMove(actionPoint, SpawnManager.Instance.noteArriveTime / 2)
                .SetEase(Ease.Unset)
                .OnComplete(AfterParry);
        }
        
        return 1;
    }

    protected override int ParryAction()
    {
        return 1;
    }

    private void AfterParry()
    {
        isRun = true;
        tween = transform.DOMove(target.position, SpawnManager.Instance.noteArriveTime / 2).SetEase(Ease.Linear).OnComplete(Die);
    }


    private void ParriedEffect() 
    {
        if(isParriedEffectCreate)
        {return;}
        Vector3 effectSpawnPos = transform.position;

        switch (enemyState.line)
        {
            case 0:
                effectSpawnPos = transform.position + new Vector3(-1,1,0.1f);
                break;
            case 1:
                effectSpawnPos = transform.position + new Vector3(0,1,0.1f);
                break;
            case 2:
                effectSpawnPos = transform.position + new Vector3(1f,1.5f,0.1f);
                break;
        }

        isParriedEffectCreate = true;
        parriedEffectObject = Instantiate(parriedEffect[enemyState.line], effectSpawnPos, Quaternion.identity);

        switch (enemyState.line)
        {
            case 0:
                parriedEffectObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                break;
            case 1:
                parriedEffectObject.transform.localScale = new Vector3(0.8f, 1, 1);
                break;
            case 2:
                parriedEffectObject.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                parriedEffectObject.transform.localRotation = Quaternion.Euler(0, 0, 25);
                break;
        }
    }
}
