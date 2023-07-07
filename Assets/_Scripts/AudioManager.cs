using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
  private static AudioManager _instance;

  [SerializeField] private Sound[] sounds;

  public static AudioManager Instance
  {
    get
    {
      if (_instance is null)
      {
        Debug.Log("Creating new GameManager Instance");
        GameObject audioManagerInstance = Instantiate(Resources.Load("AudioManager") as GameObject);
        DontDestroyOnLoad(audioManagerInstance);
      }

      return _instance;
    }
  }

  private void Awake()
  {
    _instance = this;

    DontDestroyOnLoad(this.gameObject);



    foreach (Sound sound in sounds)
    {
      sound.source = gameObject.AddComponent<AudioSource>();
      sound.source.clip = sound.clip;
      sound.source.volume = sound.volume;
      sound.source.pitch = sound.pitch;
      sound.source.loop = sound.loop;
    }

  }

  // private void Start()
  // {

  // }

  public void Play(string name)
  {
    Sound sound = Array.Find(sounds, sound => sound.name == name);
    if (sound == null) return;
    sound.source.Play();
  }



}