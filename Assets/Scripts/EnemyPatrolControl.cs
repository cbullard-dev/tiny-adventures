using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolControl : MonoBehaviour
{
    [SerializeField] GameObject[] PatrolPoints;
    [SerializeField] float gizmoSize = 1;

    public GameObject[] GetPatrolPoints()
    {
        return PatrolPoints;
    }

    private void OnDrawGizmos()
    {
        foreach (GameObject point in PatrolPoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(point.transform.position, gizmoSize);
        }
    }
}
