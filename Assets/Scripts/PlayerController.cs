using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PhysicsObject
{

    PlayerControls controls;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 15f;
    [Range(0f, 1f)]
    [SerializeField] private float BounceRate = 0.75f;
    [Space(10)]
    [SerializeField] private Vector2 GroundCheckBox;

    [Space(10)]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask platformLayer;
    [ReadOnly, SerializeField] private LayerMask groundCombinedLayer;

    private Rigidbody2D playerRigidbody;
    private Transform playerTransform;
    private Animator playerAnimator;
    private float horizontalMovement;
    [ReadOnly, SerializeField] private bool isGrounded;
    [ReadOnly, SerializeField] private bool jump;
    private bool facingRight = true;
    private bool flipSprite;
    private bool playerDead = false;
    private GameObject SpawnPoint;

    private void Awake()
    {
        playerRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        playerTransform = this.gameObject.GetComponent<Transform>();
        playerAnimator = this.gameObject.GetComponent<Animator>();
        groundCombinedLayer = groundLayer | platformLayer;
        SpawnPoint = GameObject.FindWithTag("Respawn");
    }

    protected override void Update()
    {
        base.Update();
        isGrounded = CheckGrounded();
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

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if ((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform") && playerRigidbody.velocity.y <= 0)
    //     {
    //         isGrounded = true;
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
    //     {
    //         isGrounded = false;
    //     }
    // }

    private bool CheckGrounded()
    {
        // BoxCollider2D groundCheckBox = this.gameObject.GetComponent<BoxCollider2D>();
        // RaycastHit2D box = Physics2D.BoxCast(groundCheckBox.offset, groundCheckBox.size, 0, Vector2.down, 1f, groundCombinedLayer);
        // Debug.Log(box);

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - this.gameObject.GetComponent<CapsuleCollider2D>().size.y * 0.5f), Vector2.down, 0.3f, groundCombinedLayer);
        if (hit)
        {
            return true;
        }

        return false;
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

    private void OnDrawGizmos()
    {
        BoxCollider2D groundCheckBox = this.gameObject.GetComponent<BoxCollider2D>();
        DrawRaycastBox(new Vector2(this.transform.position.x - 0.36f, this.transform.position.y - 0.5f), groundCheckBox.size);
        Gizmos.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y - this.gameObject.GetComponent<CapsuleCollider2D>().size.y * 0.5f), new Vector2(this.transform.position.x, this.transform.position.y * 0.5f) + Vector2.down);
    }

    private void DrawRaycastBox(Vector2 origin, Vector2 size)
    {
        float sizeX = size.x;
        float sizeY = size.y;
        Vector2 topRight = origin + new Vector2(sizeX, 0);
        Vector2 bottomLeft = origin + new Vector2(0, -sizeY);
        Vector2 BottomRight = origin + new Vector2(sizeX, -sizeY);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(origin, topRight);
        Gizmos.DrawLine(origin, bottomLeft);
        Gizmos.DrawLine(bottomLeft, BottomRight);
        Gizmos.DrawLine(topRight, BottomRight);
    }

}