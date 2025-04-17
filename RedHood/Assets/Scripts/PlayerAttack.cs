using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;
    public List<GameObject> attackObjList = new List<GameObject>();

    private bool isAttacking = false;

    [Header("애니메이션 상태 이름")]
    public string attackStateName = "Attack";

    public int attack = 5;

    //3단공격 관련 변수(몬스터 3마리 죽이면 공격 가능)
    
    private int playerMp = 0;
    private int maxPlayerMp = 3;
    public GameObject[] mpUI;



    void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
        MonsterManager.onMonsterDied += AddPlayerMp;
    }

    private void AddPlayerMp()
    {
        if(playerMp >= maxPlayerMp)
        {
            playerMp = maxPlayerMp;
            return;
        }
        playerMp++;
        UpdateMpUI();
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
        attackStateName = "Attack";
        StartCoroutine(AttackCooldownByAnimation());
    }

    public void PerformStrongAttack()
    {
        if(playerMp < maxPlayerMp || isAttacking)
        {
            return;
        }
        if (playerAnimation != null)
        {
            playerAnimation.TriggerStrongAttack();

        }
        playerMp = 0;
        UpdateMpUI();
        attackStateName = "StrongAttack";
        StartCoroutine(AttackCooldownByAnimation());
    }

    private void UpdateMpUI()
    {
        for(int i = 0; i < playerMp; i++)
        {
            mpUI[i].SetActive(true);
        }
        for(int i = playerMp; i < maxPlayerMp; i++)
        {
            mpUI[i].SetActive(false);
        }
    }

    private IEnumerator AttackCooldownByAnimation()
    {
        isAttacking = true;
        //StartCoroutine(Shake(shakeDuration, shakeMagnitude)); //카메라 쉐이킹 코드로 구현한 부분
        
        //ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, transform.position, new Vector3(3, 3, 0));

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

    

    public void AttackStart()
    {
        bool isFacingLft = GetComponent<SpriteRenderer>().flipX;
        if (isFacingLft)
        {
            if(attackObjList.Count > 0)
            {
                attackObjList[0].SetActive(true);
                //ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, attackObjList[0].transform.position, new Vector3(3, 3, 0));
            }
        }
        else
        {
            if (attackObjList.Count > 0)
            {
                attackObjList[1].SetActive(true);
                //ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, attackObjList[1].transform.position, new Vector3(3, 3, 0));
            }
        }
    }

    public void AttackEnd()
    {
        bool isFacingLft = GetComponent<SpriteRenderer>().flipX;
        if (isFacingLft)
        {
            if (attackObjList.Count > 0)
            {
                attackObjList[0].SetActive(false);
            }

        }
        else
        {
            if (attackObjList.Count > 0)
            {
                attackObjList[1].SetActive(false);
            }
        }

    }

    
}
