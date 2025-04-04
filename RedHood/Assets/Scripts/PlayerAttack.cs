using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;
    public List<GameObject> attackObjList = new List<GameObject>();

    private bool isAttacking = false;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originPos;

    [Header("�ִϸ��̼� ���� �̸�")]
    public string attackStateName = "Attack";

    [Header("ī�޶� ����ũ ����")]
    public CinemachineImpulseSource impulseSource;
    
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
        //StartCoroutine(Shake(shakeDuration, shakeMagnitude)); //ī�޶� ����ŷ �ڵ�� �������� �κ�
        GenerateCameraImpulse(); //�ó׸ӽ����� ī�޶� ����ŷ �����ϱ�
        //ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, transform.position, new Vector3(3, 3, 0));

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


    private IEnumerator Shake(float duration, float magnitude)
    {
        if (Camera.main == null)
        {
            yield break;
        }
        originPos = Camera.main.transform.localPosition;

        float elapsed = 0.0f;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x + x, originPos.y + y, -10);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originPos;
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }

    private void GenerateCameraImpulse()
    {
        if(impulseSource != null)
        {
            Debug.Log("ī�޶� ���޽� �߻�");
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("ImpulseSource�� ������ �ȵǾ��ֽ��ϴ�.");
        }
    }

    
}
