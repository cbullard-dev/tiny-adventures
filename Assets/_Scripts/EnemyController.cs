using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PhysicsObject
{
  [SerializeField] private EnemyPatrolControl enemyPatrolController;
  [SerializeField] private bool patrolEnemy = false;
  [SerializeField] private float moveSpeed = 2f;

  [Space(10), Header("Debug fields")]
  [ReadOnly, SerializeField] private bool isAlive = true;

  private GameObject[] _enemyPatrolPoints;
  private Rigidbody2D _enemyRigidbody;
  private Transform _enemyTransform;
  private Animator _enemyAnimator;

  private AudioSource _enemySounds;

  private Vector2 _currentTarget;
  private Vector2 _nextTarget;
  private Vector2 _previousTarget;

  private bool _facingRight = false;

  private void Awake()
  {
    _enemyTransform = this.GetComponent<Transform>();
    _enemyRigidbody = this.GetComponent<Rigidbody2D>();
    _enemyAnimator = this.gameObject.GetComponent<Animator>();
    _enemySounds = this.gameObject.GetComponent<AudioSource>();
    if (enemyPatrolController == null) patrolEnemy = false;
    if (enemyPatrolController != null)
    {
      _enemyPatrolPoints = enemyPatrolController.GetPatrolPoints();
    }
    if (_enemyPatrolPoints != null)
    {
      _currentTarget = _enemyPatrolPoints[1].transform.position;
      _nextTarget = _enemyPatrolPoints[0].transform.position;
    }
  }
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  protected override void Update()
  {
    base.Update();
    _enemyAnimator.SetFloat("xVelocity", Mathf.Abs(_enemyRigidbody.velocity.x));
    if (_enemyRigidbody.velocity.x > 0.1f && !_facingRight)
    {
      FlipEnemy();
    }
    else if (_enemyRigidbody.velocity.x < -0.1f && _facingRight)
    {
      FlipEnemy();
    }
  }

  private void FixedUpdate()
  {
    if (isAlive && patrolEnemy && Mathf.Abs(this.transform.position.x - _currentTarget.x) >= 0.5f)
    {
      MoveToTarget();
    }
    else if (isAlive)
    {
      StopMoving();
      ChangeTarget();
    }
    else
    {
      StopMoving();
    }
  }
  public void KillEnemy()
  {
    AudioManager.Instance.Play("EnemyDeath");
    isAlive = false;
    BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
    collider.enabled = false;
    collider.isTrigger = true;

    this.transform.rotation = new Quaternion(180, 0, 0, 0);
  }

  public void MoveToTarget()
  {
    _enemyRigidbody.velocity = new Vector2(moveSpeed * MoveDirection(this.transform.position.x, _currentTarget.x), _enemyRigidbody.velocity.y);
  }

  private float MoveDirection(float enemyPosition, float targetPosition)
  {
    float distance = targetPosition - enemyPosition;
    if (distance < 0)
    {
      return -1;
    }
    else if (distance > 0)
    {
      return 1;
    }
    else
    {
      return 0;
    }
  }

  private void ChangeTarget()
  {
    _previousTarget = _currentTarget;
    _currentTarget = _nextTarget;
    _nextTarget = _previousTarget;
  }

  private void StopMoving()
  {
    _enemyRigidbody.velocity = new Vector2(0, _enemyRigidbody.velocity.y);
  }

  private void FlipEnemy()
  {
    _facingRight = !_facingRight;
    Vector2 scale = _enemyTransform.localScale;
    scale.x *= -1;
    _enemyTransform.localScale = scale;
  }

  public bool IsAlive()
  {
    return isAlive;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (!isAlive) return;
    if (other.gameObject.tag != "Player") return;
    if (!other.gameObject.GetComponent<PlayerController>()) return;

    float xNormal = other.contacts[0].normal.x;
    float yNormal = other.contacts[0].normal.y;
    PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

    if (xNormal < 1f && xNormal > 0.9f || xNormal > -1f && xNormal < -0.9f)
    {
      Debug.Log("Kill the player!");
      playerController.KillPlayer();
    }

    if (yNormal >= -1f && yNormal < -0.9f)
    {
      Debug.Log("Kill the enemy!");
      KillEnemy();
      playerController.Bounce();
    }

  }
}
