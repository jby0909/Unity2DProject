using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private PlayerMovement movement;
    public PlayerAttack attack;
    private PlayerHealth health;

    //공격받았을 때 알파값 조절 및 무적모드
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isInvincible = false;
    public float invincibilityDuration = 1.0f;
    //공격받았을 때 뒤로 살짝 밀리게
    public float knockbackForce = 5.0f; 
    private Rigidbody2D rb;
    public bool isKnockback = false;
    public float knockbackDuration = 0.2f;

    private Vector3 StartPlayerPos;
    private bool isPaused = false;
    public GameObject pauseMenuUI;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health.PlayerHp = 20;
    }
    void Start()
    {
        StartPlayerPos = transform.position;
        
    }

    void Update()
    {
        if(!isKnockback)
        {
            movement.HandleMovement();
        }
        if(Input.GetButtonDown("Fire1") && !isKnockback)
        {
            attack.PerformAttack();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ReGame();
            }
            else
            {
                Pause();
            }
        }
        
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        SoundManager.Instance.PlaySFX(SFXType.UIPause);
    }

    public void ReGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        SoundManager.Instance.PlaySFX(SFXType.UIUnpause);
    }

    public void MenuOn()
    {
        SoundManager.Instance.PlaySFX(SFXType.PlayerGetItem);
        SceneManager.LoadScene("Menu");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bread"))
        {
            GameManager.Instance.AddCoin(10);
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("DeathZone"))
        {
            SoundManager.Instance.PlaySFX(SFXType.PlayerDamage);
            transform.position = StartPlayerPos;
        }
        
    }

    public void OnDamaged(float attack)
    {
        CameraShake shake = Camera.main.GetComponent<CameraShake>();
        if(shake != null)
        {
            float shakeDuration = 0.1f;
            float shakeMagnitude = 0.3f;
            StartCoroutine(shake.Shake(shakeDuration, shakeMagnitude));
        }
        if (!isInvincible)
        {
            //몬스터 부딪히면 플레이어 hp 깎기 추가
            health.PlayerHp -= attack;

            SoundManager.Instance.PlaySFX(SFXType.PlayerDamage);
            StartCoroutine(Invincibility());

            Vector2 knockbackDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            animator.SetTrigger("Hurt");
            StartCoroutine(KnockbackCoroutine());
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        Time.timeScale = 0.8f;

        float elapsedTime = 0f;
        float blinkInterval = 0.2f;

        Color originalColor = spriteRenderer.color;

        while(elapsedTime < invincibilityDuration)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.4f); //알파, 컬러 값 나중에 조절하기
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1.0f); 
            elapsedTime += blinkInterval * 2;
        }

        spriteRenderer.color = originalColor;
        isInvincible = false;
        Time.timeScale = 1.0f;
    }

    IEnumerator KnockbackCoroutine()
    {
        isKnockback = true;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockback = false;
    }
}
