using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
  protected virtual void Update()
  {
    DestroyFallingObject();
  }

  private bool IsPlayer(string tag)
  {
    return tag == "Player";
  }

  private bool IsBelowLimit()
  {
    return this.transform.position.y < -5.0f;
  }

  private void DestroyFallingObject()
  {
    if (!IsBelowLimit()) return;
    if (!IsPlayer(this.tag))
    {
      Destroy(this.gameObject);
      return;
    }
    if (IsPlayer(this.tag) && !GameManager.Instance.GameOver)
    {
      GameManager.Instance.GameOver = true;
      this.gameObject.SetActive(false);
      this.GetComponent<PlayerController>().DestroyPlayer();
      GameManager.Instance.Respawn();
      Destroy(this.gameObject);
    }
  }
}