using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : PhysicsObject
{

  PlayerControls _controls;

  [Header("Movement")]
  [SerializeField] private float moveSpeed = 2f;
  [Space(10), Header("Jumping")]
  [SerializeField] private float jumpForce = 15f;
  [SerializeField] private float coyoteTime = 0.2f;
  [SerializeField] private float jumpBuffer = 0.2f;
  [Range(0.5f, 5f)]
  [SerializeField] private float playerFallGravityModifier = 1;
  [FormerlySerializedAs("BounceRate")]
  [Range(0f, 1f)]
  [SerializeField] private float bounceRate = 0.75f;

  [Header("Layer selects")]
  [Space(10)]
  [SerializeField] private LayerMask groundLayer;
  [SerializeField] private LayerMask platformLayer;

  [Header("Debug fields")]
  [ReadOnly, SerializeField] private LayerMask groundCombinedLayer;
  [ReadOnly, SerializeField] private bool isGrounded;
  [ReadOnly, SerializeField] private bool jump;

  private Rigidbody2D _playerRigidbody;
  private Transform _playerTransform;
  private Animator _playerAnimator;

  private GameObject _spawnPoint;

  private float _defaultGravityScale;
  private float _horizontalMovement;
  private float _coyoteTimeCounter;
  private float _jumpBufferCounter;

  private bool _facingRight = true;
  private bool _playerAlive = true;
  private bool _jumping = false;
  private bool _flipSprite;


  private void Awake()
  {
    _playerRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    _playerTransform = this.gameObject.GetComponent<Transform>();
    _playerAnimator = this.gameObject.GetComponent<Animator>();
    _defaultGravityScale = _playerRigidbody.gravityScale;
    groundCombinedLayer = groundLayer | platformLayer;
    _spawnPoint = GameObject.FindWithTag("Respawn");
  }

  protected override void Update()
  {
    base.Update();
    _jumpBufferCounter -= Time.deltaTime;
    isGrounded = IsGrounded();
    PlayerGravity();
    CoyoteTime();
    Jump();

    if (_horizontalMovement > 0.1f && !_facingRight)
    {
      FlipCharacter();
    }
    else if (_horizontalMovement < -0.1f && _facingRight)
    {
      FlipCharacter();
    }

    AnimationHandler();
  }

  private void FixedUpdate()
  {
    _playerRigidbody.velocity = new Vector2(_horizontalMovement * moveSpeed, _playerRigidbody.velocity.y);
  }

  public void Move(InputAction.CallbackContext context)
  {
    if (GameManager.Instance.IsPaused) return;
    if (_playerAlive)
    {
      _horizontalMovement = context.ReadValue<float>();
    }
  }

  public void HandleJump(InputAction.CallbackContext context)
  {
    if (GameManager.Instance.IsPaused) return;
    if (context.performed)
    {
      _jumping = true;
      _jumpBufferCounter = jumpBuffer;
    }

    if (context.canceled)
    {
      _jumping = false;
    }

    if (context.canceled && _playerRigidbody.velocity.y > 0f)
    {
      JumpCancel();
    }
  }

  public void HandlePause(InputAction.CallbackContext context)
  {
    if (context.performed && !GameManager.Instance.IsPaused)
    {
      Debug.Log("Escape Pressed and Paused: " + GameManager.Instance.IsPaused);
      _horizontalMovement = 0;
      GameManager.Instance.Pause();
    }
    else if (context.performed && GameManager.Instance.IsPaused)
    {
      Debug.Log("Escape Pressed and Paused: " + GameManager.Instance.IsPaused);
      GameManager.Instance.Resume();
    }
  }

  private void AnimationHandler()
  {
    _playerAnimator.SetFloat("xVelocity", Mathf.Abs(_horizontalMovement));
    _playerAnimator.SetBool("isGrounded", isGrounded);
    _playerAnimator.SetBool("isAlive", _playerAlive);
  }

  private void JumpAnimationHandler()
  {
    if (!_playerAnimator.GetBool("isGrounded"))
    {
      _playerAnimator.SetBool("isGrounded", true);
      _playerAnimator.SetFloat("xVelocity", Mathf.Abs(_horizontalMovement));
    }
  }

  private void PlayerJump()
  {
    AudioManager.Instance.Play("PlayerJump");
    _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, 0);
    _playerRigidbody.AddRelativeForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    _jumpBufferCounter = 0f;
  }

  private void JumpCancel()
  {
    _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, _playerRigidbody.velocity.y * 0.5f);
    _coyoteTimeCounter = 0f;
  }

  private bool CanJump()
  {
    return _playerAlive && _coyoteTimeCounter > 0f;
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
      _coyoteTimeCounter = coyoteTime;
    }
    else
    {
      _coyoteTimeCounter -= Time.deltaTime;
    }
  }

  private bool JumpBuffered()
  {
    return _jumpBufferCounter > 0;
  }

  private void FlipCharacter()
  {
    _facingRight = !_facingRight;
    Vector2 scale = _playerTransform.localScale;
    scale.x *= -1;
    _playerTransform.localScale = scale;
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
    if (!isGrounded && !_jumping)
    {
      _playerRigidbody.gravityScale = _defaultGravityScale * playerFallGravityModifier;
    }
    else
    {
      _playerRigidbody.gravityScale = _defaultGravityScale;
    }
  }

  public void KillPlayer()
  {
    if (_playerAlive)
    {
      AudioManager.Instance.Play("PlayerDeath");
      _playerAlive = false;
      _horizontalMovement = 0;
      Bounce();
    }
  }

  public void DestroyPlayer()
  {
    if (_playerAlive) AudioManager.Instance.Play("ShortPlayerDeath");
  }

  public bool PlayerAlive()
  {
    return _playerAlive;
  }

  public void Bounce()
  {
    _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, 0);
    _playerRigidbody.AddRelativeForce(Vector2.up * Mathf.Ceil(jumpForce * bounceRate), ForceMode2D.Impulse);
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
    Vector2 bottomRight = origin + new Vector2(sizeX, -sizeY);
    Gizmos.color = Color.magenta;
    Gizmos.DrawLine(origin, topRight);
    Gizmos.DrawLine(origin, bottomLeft);
    Gizmos.DrawLine(bottomLeft, bottomRight);
    Gizmos.DrawLine(topRight, bottomRight);
  }

}