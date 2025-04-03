using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAttack attack;
    private PlayerHealth health;

    private Vector3 StartPlayerPos;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        health = GetComponent<PlayerHealth>();
    }
    void Start()
    {
        StartPlayerPos = transform.position;
    }

    void Update()
    {
        movement.HandleMovement();
        if(Input.GetButtonDown("Fire1"))
        {
            attack.PerformAttack();
        }
        
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
}
