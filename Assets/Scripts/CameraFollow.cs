using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  [SerializeField] private Transform cameraPosition;
  [SerializeField] private Transform playerPosition;
  [SerializeField] private Vector3 MovementZone;
  [SerializeField]
  [Min(-100)]
  private float xMinPoint = -20;

  private float yMinPoint = 6;


  void awake()
  {
    // playerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
  }

  // Start is called before the first frame update
  void Start()
  {
    this.transform.position = new Vector3(playerPosition.position.x,playerPosition.position.y > yMinPoint ? playerPosition.position.y : yMinPoint,this.transform.position.z);
  }

  // Update is called once per frame
  void Update()
  {
    this.transform.position = new Vector3(playerPosition.position.x > xMinPoint ? playerPosition.position.x : xMinPoint,playerPosition.position.y > yMinPoint ? playerPosition.position.y : yMinPoint,this.transform.position.z);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(cameraPosition.position, MovementZone);
  }
}
