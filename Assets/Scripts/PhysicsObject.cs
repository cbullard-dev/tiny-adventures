using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    protected virtual void Update()
    {
        DestroyFallingObject();
    }
    private void DestroyFallingObject()
    {
        if (this.transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }
    }
}