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
        DestroyNonPlayer(other.gameObject);
    }

    private bool CheckPlayer(string tag)
    {
        return tag == "Player";
    }

    private void DestroyNonPlayer(GameObject other)
    {
        if (CheckPlayer(other.tag) && !GameManager.Instance.GameOver)
        {
            GameManager.Instance.GameOver = true;
            other.SetActive(false);
            Destroy(other);
            GameManager.Instance.Respawn();
        }
        else
        {
            Destroy(other);
        }
    }
}
