using UnityEngine;

public class CheckpointController : MonoBehaviour
{

  private bool activated = false;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag != "Player") return;
    if (activated) return;
    activated = true;
    GameObject respawn = GameObject.FindWithTag("Respawn");
    if (!respawn) return;
    respawn.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
  }
}
