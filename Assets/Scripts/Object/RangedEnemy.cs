using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RangedEnemy : Enemy
{
    private EnemyState enemyState;
    public GameObject[] parryingEffect;
    
    private void Awake()
    {
        enemyState = GetComponent<EnemyState>();
    }

    public void Update()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * 360));
    }

    public override int Parry()
    {
        StagePlayManager.Instance.JudgeNote(gameObject);
        StagePlayManager.Instance.PlaySound("Parrying");
        Die();
        Instantiate(parryingEffect[enemyState.line], transform.position, Quaternion.identity);
        ParriedParticle();
        return 1;
    }

    public void Shot()
    {
        tween = transform.DOMove(target.position, SpawnManager.Instance.noteArriveTime / 4)
            .SetDelay(SpawnManager.Instance.noteArriveTime / 4)
            .SetEase(Ease.Linear)
            .OnComplete(AttackPlayer);
    }
}
