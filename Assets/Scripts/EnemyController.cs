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

  private GameObject[] enemyPatrolPoints;
  private Rigidbody2D enemyRigidbody;
  private Transform enemyTransform;
  private Animator enemyAnimator;

  private Vector2 currentTarget;
  private Vector2 nextTarget;
  private Vector2 previousTarget;

  private bool facingRight = false;

  private void Awake()
  {
    enemyTransform = this.GetComponent<Transform>();
    enemyRigidbody = this.GetComponent<Rigidbody2D>();
    enemyAnimator = this.gameObject.GetComponent<Animator>();
    if (enemyPatrolController == null) patrolEnemy = false;
    if (enemyPatrolController != null)
    {
      enemyPatrolPoints = enemyPatrolController.GetPatrolPoints();
    }
    if (enemyPatrolPoints != null)
    {
      currentTarget = enemyPatrolPoints[1].transform.position;
      nextTarget = enemyPatrolPoints[0].transform.position;
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
    enemyAnimator.SetFloat("xVelocity", Mathf.Abs(enemyRigidbody.velocity.x));
    if (enemyRigidbody.velocity.x > 0.1f && !facingRight)
    {
      FlipEnemy();
    }
    else if (enemyRigidbody.velocity.x < -0.1f && facingRight)
    {
      FlipEnemy();
    }
  }

  private void FixedUpdate()
  {
    if (isAlive && patrolEnemy && Mathf.Abs(this.transform.position.x - currentTarget.x) >= 0.5f)
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
    isAlive = false;
    BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
    collider.enabled = false;
    collider.isTrigger = true;

    this.transform.rotation = new Quaternion(180, 0, 0, 0);
  }

  public void MoveToTarget()
  {
    enemyRigidbody.velocity = new Vector2(moveSpeed * MoveDirection(this.transform.position.x, currentTarget.x), enemyRigidbody.velocity.y);
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
    previousTarget = currentTarget;
    currentTarget = nextTarget;
    nextTarget = previousTarget;
  }

  private void StopMoving()
  {
    enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
  }

  private void FlipEnemy()
  {
    facingRight = !facingRight;
    Vector2 scale = enemyTransform.localScale;
    scale.x *= -1;
    enemyTransform.localScale = scale;
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
      playerController.PlayerDeath();
    }

    if (yNormal >= -1f && yNormal < -0.9f)
    {
      Debug.Log("Kill the enemy!");
      KillEnemy();
      playerController.Bounce();
    }

  }
}
