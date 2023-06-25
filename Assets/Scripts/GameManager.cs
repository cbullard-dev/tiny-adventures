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



  public static GameManager Instance
  {
    get
    {
      if (_instance is null)
      {
        Debug.Log("Creating new GameManager Instance");
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
    Debug.Log(TotalScenes);
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
      if (!GameObject.FindWithTag("Player") && respawn != null)
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
    Debug.Log("current level: " + currentLevel + " Total scene: " + TotalScenes);

    if (currentLevel == TotalScenes)
    {
      GameManager.Instance.GameOver = true;
      Debug.Log("Loading main menu");
      GameManager.Instance.LoadMainMenu();
    }
    else if (currentLevel < TotalScenes)
    {
      Debug.Log("Loading next level");
      GameManager.Instance.LoadLevel(currentLevel + 1);
    }
    else
    {
      Debug.LogError("Tried to load a unavailable scene");
    }
  }

  private bool CanPause()
  {
    if (SceneManager.GetActiveScene().buildIndex != 0) return true;
    return false;
  }

  public void Pause()
  {
    if (!isPaused && CanPause())
    {
      Instance.isPaused = true;
      Time.timeScale = 0;
      Debug.Log("IsPaused: " + GameManager.Instance.isPaused);
    }
  }

  public void Resume()
  {
    if (isPaused)
    {
      Time.timeScale = 1;
      Instance.isPaused = false;
      Debug.Log("IsPaused: " + GameManager.Instance.isPaused);
    }
  }
}
