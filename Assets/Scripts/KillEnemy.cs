using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D[] playerKillColliders;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerController>().Bounce();
            foreach (BoxCollider2D collider in playerKillColliders)
            {
                collider.enabled = false;
            }
            this.GetComponentInParent<EnemyController>().KillEnemy();
        }
    }
}
