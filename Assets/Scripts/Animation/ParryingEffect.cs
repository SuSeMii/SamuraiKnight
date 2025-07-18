using UnityEngine;

public class ParryingEffect : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
