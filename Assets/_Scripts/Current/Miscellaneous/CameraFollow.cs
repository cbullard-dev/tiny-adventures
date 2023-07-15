using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  [SerializeField] private Transform cameraPosition;
  [SerializeField] private Transform playerPosition;
  [SerializeField] private Vector3 movementZone;
  [SerializeField]
  [Min(-100)]
  private float xMinPoint = -20;

  private const float YMinPoint = 6;

  // private AudioSource _player;


  private void Awake()
  {
    // _player = this.gameObject.AddComponent<AudioSource>() as AudioSource;
    playerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
  }

  // Start is called before the first frame update
  private void Start()
  {
    Transform currentTransform = this.transform;
    Vector3 position = playerPosition.position;
    currentTransform.position = new Vector3(position.x, position.y > YMinPoint ? playerPosition.position.y : YMinPoint, currentTransform.position.z);
  }

  // Update is called once per frame
  private void Update()
  {
    if (playerPosition == null && GameObject.FindWithTag("Player"))
    {
      playerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    if (playerPosition == null) return;
    Transform currentTransform = this.transform;
    currentTransform.position = new Vector3(playerPosition.position.x > xMinPoint ? playerPosition.position.x : xMinPoint, playerPosition.position.y > YMinPoint ? playerPosition.position.y : YMinPoint, currentTransform.position.z);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(cameraPosition.position, movementZone);
  }
}
