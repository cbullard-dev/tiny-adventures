using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PhysicsObject
{

  PlayerControls controls;

  [Header("Movement")]
  [SerializeField] private float moveSpeed = 2f;
  [Space(10), Header("Jumping")]
  [SerializeField] private float jumpForce = 15f;
  [SerializeField] private float coyoteTime = 0.2f;
  [SerializeField] private float jumpBuffer = 0.2f;
  [Range(0.5f, 5f)]
  [SerializeField] private float playerFallGravityModifier = 1;
  [Range(0f, 1f)]
  [SerializeField] private float BounceRate = 0.75f;

  [Header("Layer selects")]
  [Space(10)]
  [SerializeField] private LayerMask groundLayer;
  [SerializeField] private LayerMask platformLayer;

  [Header("Debug fields")]
  [ReadOnly, SerializeField] private LayerMask groundCombinedLayer;
  [ReadOnly, SerializeField] private bool isGrounded;
  [ReadOnly, SerializeField] private bool jump;

  private Rigidbody2D playerRigidbody;
  private Transform playerTransform;
  private Animator playerAnimator;

  private GameObject SpawnPoint;

  private float defaultGravityScale;
  private float horizontalMovement;
  private float coyoteTimeCounter;
  private float jumpBufferCounter;

  private bool facingRight = true;
  private bool playerAlive = true;
  private bool jumping = false;
  private bool flipSprite;


  private void Awake()
  {
    playerRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    playerTransform = this.gameObject.GetComponent<Transform>();
    playerAnimator = this.gameObject.GetComponent<Animator>();
    defaultGravityScale = playerRigidbody.gravityScale;
    groundCombinedLayer = groundLayer | platformLayer;
    SpawnPoint = GameObject.FindWithTag("Respawn");
  }

  protected override void Update()
  {
    base.Update();
    jumpBufferCounter -= Time.deltaTime;
    isGrounded = IsGrounded();
    PlayerGravity();
    CoyoteTime();
    Jump();

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
    if (GameManager.Instance.isPaused) return;
    if (playerAlive)
    {
      horizontalMovement = context.ReadValue<float>();
    }
  }

  public void HandleJump(InputAction.CallbackContext context)
  {
    if (GameManager.Instance.isPaused) return;
    if (context.performed)
    {
      jumping = true;
      jumpBufferCounter = jumpBuffer;
    }

    if (context.canceled)
    {
      jumping = false;
    }

    if (context.canceled && playerRigidbody.velocity.y > 0f)
    {
      JumpCancel();
    }
  }

  public void HandlePause(InputAction.CallbackContext context)
  {
    if (context.performed && !GameManager.Instance.isPaused)
    {
      Debug.Log("Escape Pressed and Paused: " + GameManager.Instance.isPaused);
      horizontalMovement = 0;
      GameManager.Instance.Pause();
    }
    else if (context.performed && GameManager.Instance.isPaused)
    {
      Debug.Log("Escape Pressed and Paused: " + GameManager.Instance.isPaused);
      GameManager.Instance.Resume();
    }
  }

  private void AnimationHandler()
  {
    playerAnimator.SetFloat("xVelocity", Mathf.Abs(horizontalMovement));
    playerAnimator.SetBool("isGrounded", isGrounded);
    playerAnimator.SetBool("isAlive", playerAlive);
  }

  private void JumpAnimationHandler()
  {
    if (!playerAnimator.GetBool("isGrounded"))
    {
      playerAnimator.SetBool("isGrounded", true);
      playerAnimator.SetFloat("xVelocity", Mathf.Abs(horizontalMovement));
    }
  }

  private void PlayerJump()
  {
    AudioManager.Instance.Play("PlayerJump");
    playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
    playerRigidbody.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    jumpBufferCounter = 0f;
  }

  private void JumpCancel()
  {
    playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y * 0.5f);
    coyoteTimeCounter = 0f;
  }

  private bool CanJump()
  {
    return playerAlive && coyoteTimeCounter > 0f;
  }

  private void Jump()
  {
    if (CanJump() && JumpBuffered())
    {
      PlayerJump();
    }
  }

  private void CoyoteTime()
  {
    if (IsGrounded())
    {
      coyoteTimeCounter = coyoteTime;
    }
    else
    {
      coyoteTimeCounter -= Time.deltaTime;
    }
  }

  private bool JumpBuffered()
  {
    return jumpBufferCounter > 0;
  }

  private void FlipCharacter()
  {
    facingRight = !facingRight;
    Vector2 scale = playerTransform.localScale;
    scale.x *= -1;
    playerTransform.localScale = scale;
  }

  private bool IsGrounded()
  {
    RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - this.gameObject.GetComponent<CapsuleCollider2D>().size.y * 0.5f), Vector2.down, 0.3f, groundCombinedLayer);
    if (hit)
    {
      return true;
    }

    return false;
  }

  private void PlayerGravity()
  {
    if (!isGrounded && !jumping)
    {
      playerRigidbody.gravityScale = defaultGravityScale * playerFallGravityModifier;
    }
    else
    {
      playerRigidbody.gravityScale = defaultGravityScale;
    }
  }

  public void KillPlayer()
  {
    if (playerAlive)
    {
      AudioManager.Instance.Play("PlayerDeath");
      playerAlive = false;
      horizontalMovement = 0;
      Bounce();
    }
  }

  public void DestroyPlayer()
  {
    if (playerAlive) AudioManager.Instance.Play("ShortPlayerDeath");
  }

  public bool PlayerAlive()
  {
    return playerAlive;
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