using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        int index = Random.Range(1, 4);
        string str = "AttackEffect" + index;
        animator.SetTrigger(str);
    }

    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Destroy(gameObject);
        }
    }
}
