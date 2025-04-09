using UnityEngine;

public class WolfAnimation : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void TirggerDamage()
    {
        animator.SetTrigger("Damage");
    }

    public void TriggerDead()
    {
        animator.SetTrigger("Dead");
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalk", isWalking);
    }

   
}
