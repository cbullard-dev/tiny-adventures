using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  private static GameManager _instance;

  [SerializeField] private int PlayerDefaultLives = 3;
  [SerializeField] private GameObject playerPrefab;

  [HideInInspector] public AudioManager AudioInstance;

  public bool isPaused { get; private set; }
  public bool isMainMenu { get; private set; }
  public bool PlayerAlive { get; set; }
  public bool GameOver { get; set; }

  public int PlayerScore { get; set; } = 0;
  public int PlayerLives { get; set; }
  public int TotalScenes { get; private set; }

  private int[] pauseableScenes = { 0, 1, 4 };



  public static GameManager Instance
  {
    get
    {
      if (_instance is null)
      {
        GameObject gameManagerInstance = Instantiate(Resources.Load("GameManager") as GameObject);
        DontDestroyOnLoad(gameManagerInstance);
      }

      return _instance;
    }
  }

  private void Awake()
  {
    if (_instance is null && _instance != this)
    {
      _instance = this;
    }
    else
    {
      Destroy(this.gameObject);
    }
    TotalScenes = SceneManager.sceneCountInBuildSettings - 1;
    PlayerLives = PlayerDefaultLives;
    AudioInstance = FindObjectOfType<AudioManager>();
  }



  private void Start()
  {
    AudioManager.Instance.Play("MainTheme");
  }


  public void LoadLevel(int levelId)
  {
    SceneManager.LoadScene(levelId);
  }

  public void LoadMainMenu()
  {
    GameManager.Instance.LoadLevel(1);
    GameManager.Instance.PlayerLives = PlayerDefaultLives;
  }

  public void Respawn()
  {
    GameManager.Instance.PlayerLives--;
    if (_instance.PlayerLives > 0)
    {
      GameObject respawn = GameObject.FindWithTag("Respawn");
      GameManager.Instance.GameOver = false;
      GameObject player = GameObject.FindWithTag("Player");
      if (respawn != null)
      {
        Instantiate(playerPrefab, respawn.transform.position, respawn.transform.rotation);
      }
    }
    else
    {
      GameManager.Instance.PlayerLives = PlayerDefaultLives;
      GameManager.Instance.LoadMainMenu();
    }
  }

  public void LoadNextLevel()
  {
    int currentLevel = SceneManager.GetActiveScene().buildIndex;

    if (currentLevel == TotalScenes)
    {
      GameManager.Instance.GameOver = true;
      GameManager.Instance.LoadMainMenu();
    }
    else if (currentLevel < TotalScenes)
    {
      GameManager.Instance.LoadLevel(currentLevel + 1);
    }
    else
    {
      Debug.LogError("Tried to load a unavailable scene");
    }
  }

  private bool CanPause()
  {
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    int index = Array.IndexOf(pauseableScenes, currentSceneIndex);
    if (index < 0) return true;
    return false;
  }

  public void Pause()
  {
    if (!isPaused && CanPause())
    {
      Instance.isPaused = true;
      Time.timeScale = 0;
    }
  }

  public void Resume()
  {
    if (isPaused)
    {
      Time.timeScale = 1;
      Instance.isPaused = false;
    }
  }
}
