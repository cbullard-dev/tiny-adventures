using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PhysicsObject
{

    PlayerControls controls;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 15f;
    [Range(0f, 1f)]
    [SerializeField] private float BounceRate = 0.75f;

    private Rigidbody2D playerRigidbody;
    private Transform playerTransform;
    private Animator playerAnimator;
    private float horizontalMovement;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool jump;
    private bool facingRight = true;
    private bool flipSprite;
    private bool playerDead = false;
    private GameObject SpawnPoint;

    private void Awake()
    {
        playerRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        playerTransform = this.gameObject.GetComponent<Transform>();
        playerAnimator = this.gameObject.GetComponent<Animator>();
        SpawnPoint = GameObject.FindWithTag("Respawn");
    }

    protected override void Update()
    {
        base.Update();
        if (horizontalMovement > 0.1f && !facingRight)
        {
            FlipCharacter();
        }
        else if (horizontalMovement < -0.1f && facingRight)
        {
            FlipCharacter();
        }

        AnimationHandler();
    }

    private void FixedUpdate()
    {
        playerRigidbody.velocity = new Vector2(horizontalMovement * moveSpeed, playerRigidbody.velocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        // Old Movement
        // playerRigidbody.velocity = new Vector2(horizontalMovement * moveSpeed, playerRigidbody.velocity.y);
        if (!playerDead)
        {
            horizontalMovement = context.ReadValue<float>();
        }
    }

    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !playerDead)
        {
            playerRigidbody.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (context.canceled && playerRigidbody.velocity.y > 0f)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y * 0.5f);
        }
    }

    private void AnimationHandler()
    {
        playerAnimator.SetFloat("xVelocity", Mathf.Abs(horizontalMovement));
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetBool("isDead", playerDead);
    }

    private void JumpAnimationHandler()
    {
        if (!playerAnimator.GetBool("isGrounded"))
        {
            playerAnimator.SetBool("isGrounded", true);
            playerAnimator.SetFloat("xVelocity", Mathf.Abs(horizontalMovement));
        }
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        Vector2 scale = playerTransform.localScale;
        scale.x *= -1;
        playerTransform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform") && playerRigidbody.velocity.y <= 0)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            isGrounded = false;
        }
    }

    public void PlayerDeath()
    {
        if (!playerDead)
        {
            playerDead = true;
            horizontalMovement = 0;
            Bounce();
        }
        // this.enabled = false;
    }

    private void OnDestroy()
    {
        if (!GameManager.Instance.GameOver && GameManager.Instance.PlayerLives > 0)
        {
            GameManager.Instance.Respawn();
        }
        else if (GameManager.Instance.PlayerLives <= 0)
        {
            GameManager.Instance.GameOver = true;
            GameManager.Instance.LoadMainMenu();
        }
    }

    public void Bounce()
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
        playerRigidbody.AddRelativeForce(Vector2.up * Mathf.Ceil(jumpForce * BounceRate), ForceMode2D.Impulse);
    }

}