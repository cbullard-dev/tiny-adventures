using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PhysicsObject
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
    protected override void Update()
    {
        base.Update();
    }

    public void KillEnemy()
    {
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        collider.enabled = false;
        collider.isTrigger = true;

        this.transform.rotation = new Quaternion(180, 0, 0, 0);
    }
}
