using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void TriggerStrongAttack()
    {
        animator.SetTrigger("StrongAttack");
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void SetJumping(bool isJumping)
    {
        animator.SetBool("IsJumping", isJumping);
    }

    public void SetFalling(bool isFalling)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("PlayerJumpMiddleAni") == true)
        {
            return;
        }

        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", isFalling);
    }

    public void PlayLanding()
    {
        animator.SetTrigger("Land");
        animator.SetBool("IsFalling", false);
    }
    

    public void TriggerDead()
    {
        animator.SetTrigger("Dead");
    }

    public void TriggerHurt()
    {
        animator.SetTrigger("Hurt");
    }

    public bool IsJumpState()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("PlayerJumpStartAni") || info.IsName("PlayerJumpMiddleAni"))
        {
            return true;
        }

        return false;
    }

    #region OnAnimationEvent
    public void PlayerFootStepSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.PlayerFootStep);
    }

    public void PlayerAttackSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.PlayerAttack);
    }
    #endregion
}
