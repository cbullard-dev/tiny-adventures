using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class IntroTransition : MonoBehaviour
{
  [SerializeField] private VideoPlayer introVideo;

  private bool _playedOnce;

  // Start is called before the first frame update
  private void Start()
  {
    _playedOnce = false;
  }

  // Update is called once per frame
  private void Update()
  {
    if (_playedOnce && !introVideo.isPlaying) GameManager.Instance.LoadMainMenu();

    if (!_playedOnce && !introVideo.isPlaying) return;

    if (_playedOnce && introVideo.isPlaying) return;

    if (!_playedOnce && introVideo.isPlaying) _playedOnce = true;
  }
  
  public void Any(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      introVideo.Stop();
    }
  }
}
