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
      if (_instance is not null) return _instance;
      GameObject audioManagerInstance = Instantiate(Resources.Load("AudioManager") as GameObject);
      DontDestroyOnLoad(audioManagerInstance);

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

  public void Play(string soundName)
  {
    Sound sound = Array.Find(sounds, sound => sound.name == soundName);
    sound?.source.Play();
  }
}