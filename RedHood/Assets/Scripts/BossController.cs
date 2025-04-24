using UnityEditor;
using UnityEngine;



public enum BossStateType
{
    Idle,
    Patrol,
    Chase,
    Attack
}
public class BossController : MonoBehaviour
{
    private int bossHp;
    private int bossAttack;
    public Transform[] path;

    private float speed = 3.0f;

    //hp가 10% 남았을 때 빨간색으로 물들이기 /  데미지 받았을 때 알파값 깜빡거리기
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f;

    //멈춤구간
    private bool isStop;

    public Transform player;
    public BossStateType stateType = BossStateType.Idle;
    public float chaseRange = 5.0f;
    public float attackRange = 1.5f;
    //죽음 판정
    private bool isDead;
    public bool isAttacking = false;

    private float stateChangeInterval = 3.0f;
    private Coroutine stateChangeRoutine;
    private Coroutine deadCoroutine;


    private void Awake()
    {
        bossHp = 30;
        bossAttack = 5;
    }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

    }

    //void Update()
    //{
    //    if (isDead)
    //    {
    //        Dead();
    //        return;
    //    }
    //    if (isStop)
    //    {
    //        return;
    //    }

       

    //    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    //    if (distanceToPlayer <= attackRange && !isAttacking)
    //    {
    //        if (stateType != BossStateType.Attack)
    //        {
    //            //StopAllCoroutines();
    //            if (stateChangeRoutine != null)
    //            {
    //                StopCoroutine(stateChangeRoutine);
    //            }
    //            stateType = BossStateType.Attack;
    //            StartCoroutine(AttackRoutine());
    //        }
    //        return;
    //    }
    //    if (distanceToPlayer <= chaseRange)
    //    {
    //        if (stateType != BossStateType.Chase)
    //        {
    //            if (stateChangeRoutine != null)
    //            {
    //                StopCoroutine(stateChangeRoutine);
    //            }

    //            stateType = BossStateType.Chase;
    //        }
    //        Vector3 directionToPlayer = new Vector3(player.position.x - transform.position.x, 0, 0).normalized;
    //        //Vector3 directionToPlayer = (player.position - transform.position + new Vector3(0, 1, 0)).normalized;
    //        float chaseSpeed = speed;
    //        transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;
    //        return;
    //    }

    //    if (stateType == BossStateType.Chase && distanceToPlayer > chaseRange)
    //    {
    //        stateType = BossStateType.Idle;
    //        if (stateChangeRoutine == null)
    //        {
    //            stateChangeRoutine = StartCoroutine(RandomStateChanger());
    //        }
    //    }
    //    if (stateType == BossStateType.Attack) return;

    //    PatrolMovement();
    //}

    //private void PatrolMovement()
    //{
    //    if (currentGroundType == GroundType.UpGround)
    //    {
    //        if (stateType == BossStateType.Patrol)
    //        {
    //            if (transform.position.y > startPos.y + maxDistance)
    //            {
    //                direction = -1;
    //            }
    //            else if (transform.position.y < startPos.y - maxDistance)
    //            {
    //                direction = 1;
    //            }
    //            float moveSpeed = stateType == StateType.PatrolRun ? speed * 2 : speed;
    //            transform.position += new Vector3(0, moveSpeed * direction * Time.deltaTime, 0);

    //        }
    //    }
    //    else
    //    {
    //        if (transform.position.x > startPos.x + maxDistance)
    //        {
    //            direction = -1;
    //            GetComponent<SpriteRenderer>().flipX = true;
    //        }
    //        else if (transform.position.x < startPos.x - maxDistance)
    //        {
    //            direction = 1;
    //            GetComponent<SpriteRenderer>().flipX = false;
    //        }

    //        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
    //    }
    //}
}
