using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  [SerializeField] private Transform cameraPosition;
  [SerializeField] private Transform playerPosition;
  [SerializeField] private Vector3 MovementZone;
  [SerializeField]
  [Min(0)]
  private float xMinPoint = 0;

  private float yMinPoint = 6;



  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(cameraPosition.position, MovementZone);
  }
}
