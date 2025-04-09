using System.Collections;
using UnityEngine;


public enum MonsterType
{
    None,
    Undead,
    Wolf
}

public class ZombieManager : MonoBehaviour
{
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f;
    private float zombieHp = 10.0f;

    public float speed = 1.0f;
    public float maxDistance = 3.0f;
    private Vector3 startPos;
    private int direction = 1;
    public GroundType currentGroundType;
    public MonsterType monsterType;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
        startPos = transform.position;
    }

    private void Update()
    {
        if(monsterType != MonsterType.None)
        {
            if (currentGroundType == GroundType.UpGround)
            {
                if (transform.position.y > startPos.y + maxDistance)
                {
                    direction = -1;
                }
                else if (transform.position.y < startPos.y - maxDistance)
                {
                    direction = 1;
                }

                transform.position += new Vector3(0, speed * direction * Time.deltaTime, 0);
            }
            else
            {
                if (transform.position.x > startPos.x + maxDistance)
                {
                    direction = -1;
                    GetComponent<SpriteRenderer>().flipX = true;
                    animator.SetBool("isWalk", true);

                }
                else if (transform.position.x < startPos.x - maxDistance)
                {
                    direction = 1;
                    GetComponent<SpriteRenderer>().flipX = false;
                    animator.SetBool("isWalk", true);
                }

                transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
            }
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            CameraShake shake = Camera.main.GetComponent<CameraShake>();
            if (shake != null)
            {
                shake.GenerateCameraImpulse(); //시네머신으로 카메라 쉐이킹 구현하기
            }
            StartCoroutine(ChangeColorTemporatily());      
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                player.OnDamaged();
            }
        }
    }

    IEnumerator ChangeColorTemporatily()
    {
        SoundManager.Instance.PlaySFX(SFXType.EnemyDamage);
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objectRenderer.material.color = originalColor;
    }
}
