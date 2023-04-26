using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D[] PlayerKillZones;
    [SerializeField]
    private BoxCollider2D EnemyKillZone;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < -50)
        {
            Destroy(this.gameObject);
        }
    }

    public void KillEnemy()
    {
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        collider.enabled = false;
        collider.isTrigger = true;

        this.transform.rotation = new Quaternion(180, 0, 0, 0);
    }
}
