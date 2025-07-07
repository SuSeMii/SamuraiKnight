using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    public bool isRight= false; 
    public bool isLeft=false;
    public int line = 1;

    public AudioClip attackSound;
    public AudioClip parryingSound;
    void Start()
    {
        animator = GetComponent<Animator>();
        StagePlayManager.Instance.playerAnimation = this;
    }
    void Update(){

        if (animator.GetBool("isDead") || Time.timeScale == 0 || !StagePlayManager.Instance.isControlable)
        {
            return;
        }

        // 방향 전환
        // 왼쪽
        if (Input.GetKeyDown(KeyCode.A))
        {
            isLeft = true;
            isRight = false;
            animator.SetTrigger("left");
            line = 0;
            animator.SetInteger("line", line);
            // 왼쪽 바라보는 애니메이션 재생
            // animator.SetBool("left",true);
            // animator.SetBool("mid",false);
            // animator.SetBool("right",false);

        }
        // 가운데
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            isLeft = false;
            isRight = false;
            animator.SetTrigger("mid");
            line = 1;
            animator.SetInteger("line", line);
            // 가운데 바라보는 애니메이션 재생
            // animator.SetBool("mid",true);
            // animator.SetBool("left",false);
            // animator.SetBool("right",false);


        }
        // 오른쪽
        if (Input.GetKeyDown(KeyCode.D))
        {
            isLeft = false;
            isRight = true;
            animator.SetTrigger("right");
            line = 2;
            animator.SetInteger("line", line);
            // 오른쪽 바라보는 애니메이션 재생
            // animator.SetBool("right",true);
            // animator.SetBool("mid",false);
            // animator.SetBool("left",false);
        }

        // 패링
        if (Input.GetKeyDown(KeyCode.K))
        {
            // 왼쪽 패링
            if (isLeft)
            {
                animator.SetTrigger("left_parrying");
            }
            // 오른쪽 패링
            else if (isRight)
            {
                animator.SetTrigger("right_parrying");
            }
            // 가운데 패링
            else
            {
                animator.SetTrigger("mid_parrying");
            }
            AudioSource.PlayClipAtPoint(parryingSound, Camera.main.transform.position);

        }

        // 공격
        if (Input.GetKeyDown(KeyCode.J))
        {
            // 왼쪽 공격
            if (isLeft)
            {
                animator.SetTrigger("left_attack");
            }
            // 오른쪽 공격
            else if (isRight)
            {
                animator.SetTrigger("right_attack");
            }
            // 가운데 공격
            else
            {
                animator.SetTrigger("mid_attack");
            }
            AudioSource.PlayClipAtPoint(attackSound, Camera.main.transform.position);
        }
    }
    public void deathAnim()
    {
        animator.SetTrigger("death");
        animator.SetBool("isDead", true);
    }
    public void hurtsAnim()
    {
        animator.SetTrigger("hurts");
    }
}