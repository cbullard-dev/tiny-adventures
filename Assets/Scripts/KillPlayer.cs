using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !this.GetComponentInParent<EnemyController>().GetIsDead())
        {
            other.gameObject.GetComponent<PlayerController>().PlayerDeath();
        }
    }
}
