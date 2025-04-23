using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private PlayerMovement movement;
    public PlayerAttack attack;
    private PlayerHealth health;

    private PlayerAnimation animation;

    //공격받았을 때 알파값 조절 및 무적모드
    private SpriteRenderer spriteRenderer;
    //private Animator animator;
    public bool isInvincible = false;
    public float invincibilityDuration = 1.0f;
    //공격받았을 때 뒤로 살짝 밀리게
    public float knockbackForce = 5.0f; 
    private Rigidbody2D rb;
    public bool isKnockback = false;
    public float knockbackDuration = 0.2f;

    private Vector3 StartPlayerPos;
    private bool isPaused = false;
    public GameObject pauseMenuUI;

    //Hp UI
    public Slider HpBarUI;

    //죽음 판정
    private bool isDead = false;
    private Coroutine deadCoroutine;
    public GameObject gameOverUI;

    //빵 부족 시 활성화할 ui
    public GameObject breadMessageUI;


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
        //animator = GetComponent<Animator>();
        animation = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        health.PlayerHp = 50;
    }
    void Start()
    {
        StartPlayerPos = transform.position;
        
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (!isKnockback)
        {
            movement.HandleMovement();
        }
        if(Input.GetButtonDown("Fire1") && !isKnockback)
        {
            attack.PerformAttack();
        }
        if(Input.GetMouseButtonDown(1))
        {
            attack.PerformStrongAttack();
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

    private void Dead()
    {
        isDead = true;
        animation.TriggerDead();
        SoundManager.Instance.PlaySFX(SFXType.PlayerDead);
        StopAllCoroutines();
        StartCoroutine(GameOverUIActive());

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
            GameManager.Instance.AddBread();
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("DeathZone"))
        {
            SoundManager.Instance.PlaySFX(SFXType.PlayerDamage);
            OnDamaged(10);
            transform.position = StartPlayerPos;
            
        }
        else if(collision.CompareTag("Goal"))
        {
            if(GameManager.Instance.currentBreadCount == GameManager.Instance.stageDataDict[GameManager.Instance.currentStageLevel].breadCount)
            {
                SoundManager.Instance.PlaySFX(SFXType.ArriveGoal);
                SceneManagerController.Instance.StartSceneTransition(SceneManagerController.Instance.nextSceneName);
            }
            else
            {
                //빵 부족하다는 ui 활성화(2초뒤에 사라지기 -> 코루틴으로 구현)
                StartCoroutine(BreadMessageUIActive());
            }
        }
        
    }

    IEnumerator BreadMessageUIActive()
    {
        breadMessageUI.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        breadMessageUI.SetActive(false);
    }

    public void OnDamaged(float attack)
    {
        if(isDead)
        {
            return;
        }
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
            health.PlayerHp -= (int)attack;
            UpdateHpUI();
            if(health.PlayerHp <= 0)
            {
                Dead();
                return;
            }

            SoundManager.Instance.PlaySFX(SFXType.PlayerDamage);
            StartCoroutine(Invincibility());

            Vector2 knockbackDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            animation.TriggerHurt();
            StartCoroutine(KnockbackCoroutine());
        }
    }

    private void UpdateHpUI()
    {
        float hpRate = (float)health.PlayerHp / (float)health.MaxHp;
        Debug.Log($"남은 hp : {health.PlayerHp}/ 비율 : {hpRate}");
        HpBarUI.value = hpRate;
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

    IEnumerator GameOverUIActive()
    {
        yield return new WaitForSeconds(1.0f);
        gameOverUI.SetActive(true);
    }
}
