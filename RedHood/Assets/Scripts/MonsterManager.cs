using System;
using System.Collections;
using UnityEngine;


public enum MonsterType
{
    None,
    Undead,
    Wolf
}

public enum StateType
{
    Idle,
    PatrolWalk,
    PatrolRun,
    ChaseWalk,
    ChaseRun,
    StrongAttack,
    Attack
}

public class MonsterManager : MonoBehaviour
{
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f;
    public float monsterHp = 10.0f;

    public float speed = 1.0f;
    public float maxDistance = 2.0f;
    private Vector3 startPos;
    private int direction = 1;
    public GroundType currentGroundType;
    public MonsterType monsterType;
    public float damage = 1.0f;
    public StateType stateType = StateType.Idle;

    public Transform player;
    public float chaseRange = 5.0f;
    public float attackRange = 1.5f;
    public bool isAttacking = false;

    private float stateChangeInterval = 3.0f;
    private Coroutine stateChangeRoutine;

    Animator animator;

    private bool isDead = false;
    private Coroutine deadCoroutine;

    //���� ���� �� �߻���ų �̺�Ʈ
    public static event Action onMonsterDied;

    //���ݹ޾��� �� ��� �����ְ�(�÷��̾ �Ѿư��� �ʰ�) ���� ����
    private bool isStop = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
        startPos = transform.position;
        int randomChoice = UnityEngine.Random.Range(0, 2);
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        stateChangeRoutine = StartCoroutine(RandomStateChanger());

        Debug.Log($"speed : {speed} \n hp : {monsterHp} \n damage : {damage}");
    }

    private void Update()
    {
        if(isDead)
        {
            Dead();
            return;
        }
        if(isStop)
        {
            return;
        }

        if (monsterType == MonsterType.None || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            if (stateType != StateType.Attack)
            {
                //StopAllCoroutines();
                if (stateChangeRoutine != null)
                {
                    StopCoroutine(stateChangeRoutine);
                }
                stateType = StateType.StrongAttack;
                StartCoroutine(AttackRoutine());
            }
            return;
        }
        if (distanceToPlayer <= chaseRange)
        {
            if(stateType != StateType.ChaseWalk && stateType != StateType.ChaseRun)
            {
                if(stateChangeRoutine != null)
                {
                    StopCoroutine(stateChangeRoutine);
                }

                int chaseType = UnityEngine.Random.Range(0, 2);
                stateType = chaseType == 0? StateType.ChaseWalk : StateType.ChaseRun;
                Debug.Log($"[���� ��ȯ] ���� ���� : {stateType}");
            }
            Vector3 directionToPlayer = new Vector3(player.position.x - transform.position.x, 0, 0).normalized;
            //Vector3 directionToPlayer = (player.position - transform.position + new Vector3(0, 1, 0)).normalized;
            float chaseSpeed = stateType == StateType.ChaseRun ? speed * 2 : speed;
            transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;
            return;
        }

        if((stateType == StateType.ChaseWalk || stateType == StateType.ChaseRun) && distanceToPlayer > chaseRange)
        {
            Debug.Log("[���� ����] ���� ����");
            stateType = StateType.Idle;
            if(stateChangeRoutine == null)
            {
                stateChangeRoutine = StartCoroutine(RandomStateChanger());
            }
        }
        if (stateType == StateType.Attack) return;

        PatrolMovement();
    }

    private void PatrolMovement()
    {
        if(currentGroundType == GroundType.UpGround)
        {
            if(stateType == StateType.PatrolWalk || stateType == StateType.PatrolRun )
            {
                if(transform.position.y > startPos.y + maxDistance)
                {
                    direction = -1;
                }
                else if(transform.position.y < startPos.y - maxDistance)
                {
                    direction = 1;
                }
                float moveSpeed = stateType == StateType.PatrolRun ? speed * 2 : speed;
                transform.position += new Vector3(0, moveSpeed * direction * Time.deltaTime, 0);

            }
        }
        else
        {
            if(transform.position.x > startPos.x + maxDistance)
            {
                direction = -1;
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if(transform.position.x < startPos.x - maxDistance)
            {
                direction = 1;
                GetComponent<SpriteRenderer>().flipX = false;
            }

            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
        }
    }

    private void Dead()
    {
        if(deadCoroutine != null)
        {
            return;
        }
        if(onMonsterDied != null)
        {
            onMonsterDied.Invoke();
        }
        animator.SetTrigger("Dead");
        StopAllCoroutines();
        deadCoroutine = StartCoroutine(DestroyObject());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            CameraShake shake = Camera.main.GetComponent<CameraShake>();
            if (shake != null)
            {
                shake.GenerateCameraImpulse(); //�ó׸ӽ����� ī�޶� ����ŷ �����ϱ�
            }
            animator.SetTrigger("Damage");
            StartCoroutine(StopMove());
            StartCoroutine(ChangeColorTemporatily());

            //�÷��̾� ���ݹ����� ���� hp���
            monsterHp = (monsterHp - PlayerController.Instance.attack.attack) > 0 ? monsterHp - PlayerController.Instance.attack.attack : 0;
            if(monsterHp <= 0)
            {
                isDead = true;
            }    
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                player.OnDamaged(damage);
            }
            StartCoroutine(StopMove());
        }
    }

    IEnumerator StopMove()
    {
        isStop = true;
        yield return new WaitForSeconds(1.0f);
        isStop = false;
    }

    IEnumerator ChangeColorTemporatily()
    {
        SoundManager.Instance.PlaySFX(SFXType.EnemyDamage);
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objectRenderer.material.color = originalColor;
    }

    IEnumerator RandomStateChanger()
    {
        while(true)
        {
            yield return new WaitForSeconds(stateChangeInterval);
            int randomState = UnityEngine.Random.Range(0, 3);
            stateType = (StateType)randomState;
            Debug.Log($"[���� ���� ��ȯ] ���� ���� : {stateType}");
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        Debug.Log("[���� ����] ���� ����");
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
        Debug.Log("[���� ����] ���� ����, ���� ����");
        stateChangeRoutine = StartCoroutine(RandomStateChanger());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
