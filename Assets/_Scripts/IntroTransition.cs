using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroTransition : MonoBehaviour
{
  [SerializeField] private VideoPlayer introVideo;

  private bool _playedOnce;

  // Start is called before the first frame update
  void Start()
  {
    _playedOnce = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (_playedOnce && !introVideo.isPlaying) GameManager.Instance.LoadMainMenu();

    if (!_playedOnce && !introVideo.isPlaying) return;

    if (_playedOnce && introVideo.isPlaying) return;

    if (!_playedOnce && introVideo.isPlaying)
    {
      _playedOnce = true;
      return;
    }




  }
}
