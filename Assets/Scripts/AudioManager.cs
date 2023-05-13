using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
  public static AudioManager _instance;

  [SerializeField] private Sound[] sounds;

  private void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
    }
    else
    {
      Destroy(this.gameObject);
      return;
    }

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

  private void Start()
  {
    Play("MainTheme");
  }

  public void Play(string name)
  {
    Sound sound = Array.Find(sounds, sound => sound.name == name);
    if (sound == null) return;
    sound.source.Play();
  }



}