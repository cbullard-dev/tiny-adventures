using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
  [SerializeField] private Transform cameraPosition;
  [SerializeField] private Transform playerPosition;
  [SerializeField] private Vector3 movementZone;
  [SerializeField]
  [Min(-100)]
  private float xMinPoint = -20;

  private float yMinPoint = 6;

  private AudioSource _player;


  private void Awake()
  {
    _player = this.gameObject.AddComponent<AudioSource>() as AudioSource;
    playerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
  }

  // Start is called before the first frame update
  private void Start()
  {
    this.transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y > yMinPoint ? playerPosition.position.y : yMinPoint, this.transform.position.z);
  }

  // Update is called once per frame
  private void Update()
  {
    if (playerPosition == null && GameObject.FindWithTag("Player"))
    {
      playerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    if (playerPosition != null)
    {
      this.transform.position = new Vector3(playerPosition.position.x > xMinPoint ? playerPosition.position.x : xMinPoint, playerPosition.position.y > yMinPoint ? playerPosition.position.y : yMinPoint, this.transform.position.z);
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(cameraPosition.position, movementZone);
  }
}
