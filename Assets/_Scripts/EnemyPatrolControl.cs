using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyPatrolControl : MonoBehaviour
{
    [FormerlySerializedAs("PatrolPoints")] [SerializeField] GameObject[] patrolPoints;
    [SerializeField] float gizmoSize = 1;

    public GameObject[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    private void OnDrawGizmos()
    {
        foreach (GameObject point in patrolPoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(point.transform.position, gizmoSize);
        }
    }
}
