using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

  // GameManager as singleton
  private static GameManager _instance;

  // Configure the default number of player lives
  [FormerlySerializedAs("PlayerDefaultLives")] [SerializeField] private int playerDefaultLives = 3;
  [SerializeField] private GameObject playerPrefab;


  // ToDo: Implement a hardcore game mode
  public int PlayerHardcoreLives { get; } = 1;

  [HideInInspector] public AudioManager audioInstance;

  public bool IsPaused { get; private set; }
  public bool IsMainMenu { get; private set; }
  public bool IsHardcore { get; set; } = false;
  public bool PlayerAlive { get; set; }
  public bool GameOver { get; set; }

  public int PlayerScore { get; set; } = 0;
  public int PlayerLives { get; set; }
  public int TotalScenes { get; private set; }

  private readonly int[] _pauseableScenes = { 0, 1, 4 };

  public static GameManager Instance
  {
    get
    {
      if (_instance is not null) return _instance;
      GameObject gameManagerInstance = Instantiate(Resources.Load("GameManager") as GameObject);
      DontDestroyOnLoad(gameManagerInstance);

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
    PlayerLives = playerDefaultLives;
    audioInstance = FindObjectOfType<AudioManager>();
  }



  private void Start()
  {
    AudioManager.Instance.Play("MainTheme");
  }


  public void LoadLevelByIndex(int levelId)
  {
    ResetPlayerLives();
    SceneManager.LoadScene(levelId);
  }

  public void LoadLevelByName(string levelName)
  {
    ResetPlayerLives();
    SceneManager.LoadScene(levelName);
  }

  private void ResetPlayerLives()
  {
    PlayerLives = IsHardcore ? PlayerHardcoreLives : playerDefaultLives;
  }

  public void LoadMainMenu()
  {
    GameManager.Instance.LoadLevelByIndex(1);
    GameManager.Instance.PlayerLives = playerDefaultLives;
  }

  public void Respawn()
  {
    GameManager.Instance.PlayerLives--;
    GameObject respawn = GameObject.FindWithTag("Respawn");
    GameObject player = GameObject.FindWithTag("Player");
    if (_instance.PlayerLives > 0)
    {
      GameManager.Instance.GameOver = false;
      if (respawn != null)
      {
        Instantiate(playerPrefab, respawn.transform.position, respawn.transform.rotation);
      }
    }
    else
    {
      GameManager.Instance.PlayerLives = playerDefaultLives;
      GameManager.Instance.LoadMainMenu();
    }
  }

  public void LoadNextLevel()
  {
    int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

    if (currentLevelIndex == TotalScenes)
    {
      GameManager.Instance.GameOver = true;
      GameManager.Instance.LoadMainMenu();
    }
    else if (currentLevelIndex < TotalScenes)
    {
      GameManager.Instance.LoadLevelByIndex(currentLevelIndex + 1);
    }
    else
    {
      Debug.LogError("Tried to load a unavailable scene");
    }
  }

  public bool PlayableLevel(int level)
  {
    int index = Array.IndexOf(_pauseableScenes, level);
    return index < 0;
  }

  private bool CanPause()
  {
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    int index = Array.IndexOf(_pauseableScenes, currentSceneIndex);
    return index < 0;
  }

  public void Pause()
  {
    if (IsPaused || !CanPause()) return;
    Instance.IsPaused = true;
    Time.timeScale = 0;
  }

  public void Resume()
  {
    if (!IsPaused) return;
    Time.timeScale = 1;
    Instance.IsPaused = false;
  }
}
