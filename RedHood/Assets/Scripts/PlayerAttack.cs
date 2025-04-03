using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;

    private bool isAttacking = false;

    [Header("�ִϸ��̼� ���� �̸�")]
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

        yield return null; //�ȳ־ ����� ������ �������� ���� �߰��� �κ�
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(attackStateName))
        {
            float animationLength = stateInfo.length; //�ִϸ��̼��� ��ü����
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        isAttacking = false;
    }

    
}
