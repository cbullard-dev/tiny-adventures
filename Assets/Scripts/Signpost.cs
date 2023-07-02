using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour
{

  [SerializeField] private GameObject SignCanvas;

  private bool IsPlayer(string tag)
  {
    {
      return tag == "Player";
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (!IsPlayer(other.gameObject.tag)) return;

    SignCanvas.SetActive(true);
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (!IsPlayer(other.gameObject.tag)) return;

    SignCanvas.SetActive(false);
  }
}
