using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;

    private bool isAttacking = false;

    [Header("애니메이션 상태 이름")]
    public string attackStateName = "Attack";
    
    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        
    }

    public void PerformAttack()
    {
        if (isAttacking)
        {
            return;
        }
        if (playerAnimation != null)
        {
            playerAnimation.TriggerAttack();
            
        }
        StartCoroutine(AttackCooldownByAnimation());
    }

    private IEnumerator AttackCooldownByAnimation()
    {
        isAttacking = true;

        yield return null; //안넣어도 상관은 없지만 안정성을 위해 추가한 부분
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(attackStateName))
        {
            float animationLength = stateInfo.length; //애니메이션의 전체길이
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        isAttacking = false;
    }

    
}
