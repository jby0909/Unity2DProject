using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.01f;
    public LayerMask groundLayer;

    private PlayerAnimation playerAnimation;

    private SpriteRenderer spr;
   

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        spr = GetComponent<SpriteRenderer>();
    }

    public void HandleMovement()
    {
        if(!PlayerController.Instance.isKnockback)
        {
            //Movement Logic
            float moveInput = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            if (playerAnimation != null)
            {
                playerAnimation.SetWalking(moveInput != 0);

            }

            if (moveInput != 0)
            {
                GetComponent<SpriteRenderer>().flipX = moveInput < 0;
            }

            //Jump Logic
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (Input.GetButtonDown("Jump") && isGrounded) //점프애니메이션
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                playerAnimation.SetJumping(true);
                SoundManager.Instance.PlaySFX(SFXType.PlayerJump);
               
            }
            else if (!isGrounded && rb.linearVelocity.y < -0.1f) //낙하상태
            {
                playerAnimation?.SetFalling(true);
            }
            else if (isGrounded) //착지상태
            {
                playerAnimation?.PlayLanding();
                
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(-9.35f, -4.04f, 0);
    }
}
