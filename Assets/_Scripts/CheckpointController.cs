using UnityEngine;

public class CheckpointController : MonoBehaviour
{

  [SerializeField] private Animator _animator;
  private bool _activated = false;
  private static readonly int Activeated = Animator.StringToHash("_activated");

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (!other.gameObject.CompareTag("Player")) return;
    if (_activated) return;
    _activated = true;
    _animator.SetBool(Activeated,_activated);
    GameObject respawn = GameObject.FindWithTag("Respawn");
    if (!respawn) return;
    respawn.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
  }
}
