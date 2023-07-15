using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerLimit : MonoBehaviour
{
  // private void OnCollisionEnter2D(Collision2D other)
  // {
  //    DestroyNonPlayer(other.gameObject);
  // }

  private void OnTriggerEnter2D(Collider2D other)
  {
    // DestroyNonPlayer(other.gameObject);
  }

  private bool CheckPlayer(string tag)
  {
    Debug.Log(tag);
    return tag == "Player";
  }

  // This is old code that used to handle the destruction of game objects below a certain height.
  // private void DestroyNonPlayer(GameObject other)
  // {
  //   if (CheckPlayer(other.tag) && !GameManager.Instance.GameOver)
  //   {
  //     // GameManager.Instance.GameOver = true;
  //     // other.SetActive(false);
  //     // other.GetComponent<PlayerController>().DestroyPlayer();
  //     // Destroy(other);
  //     // GameManager.Instance.Respawn();
  //   }
  //   else
  //   {
  //     Destroy(other);
  //   }
  // }
}
