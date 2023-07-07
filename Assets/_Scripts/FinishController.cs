using UnityEngine;

public class FinishController : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      AudioManager.Instance.Play("FinishReached");
      GameManager.Instance.LoadNextLevel();
    }
  }
}
