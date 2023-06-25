using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroTransition : MonoBehaviour
{
  [SerializeField] private VideoPlayer introVideo;

  private bool playedOnce;

  // Start is called before the first frame update
  void Start()
  {
    playedOnce = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (playedOnce && !introVideo.isPlaying) GameManager.Instance.LoadMainMenu();

    if (!playedOnce && !introVideo.isPlaying) return;

    if (playedOnce && introVideo.isPlaying) return;

    if (!playedOnce && introVideo.isPlaying)
    {
      playedOnce = true;
      return;
    }




  }
}
