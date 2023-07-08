using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Signpost : MonoBehaviour
{

  [FormerlySerializedAs("SignCanvas")] [SerializeField] private GameObject signCanvas;

  private bool IsPlayer(string tag)
  {
    {
      return tag == "Player";
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (!IsPlayer(other.gameObject.tag)) return;

    signCanvas.SetActive(true);
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (!IsPlayer(other.gameObject.tag)) return;

    signCanvas.SetActive(false);
  }
}
