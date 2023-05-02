using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    [SerializeField] private int PlayerDefaultLives = 3;
    [SerializeField] private GameObject playerPrefab;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.Log("Creating new GameManager Instance");
                GameObject gameManagerInstance = new GameObject("GameManager");
                DontDestroyOnLoad(gameManagerInstance);
                gameManagerInstance.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        TotalScenes = SceneManager.sceneCountInBuildSettings - 1;
        PlayerLives = PlayerDefaultLives;
    }

    public int PlayerScore { get; set; } = 0;
    public int PlayerLives { get; set; }
    public int TotalScenes { get; private set; }
    public bool PlayerAlive { get; set; }
    public bool GameOver { get; set; }

    public void LoadLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    private void LoadMainMenu()
    {
        GameManager.Instance.LoadLevel(0);
        GameManager.Instance.PlayerLives = PlayerDefaultLives;
        GameManager.Instance.GameOver = false;
    }

    public void Respawn()
    {
        GameManager.Instance.PlayerLives--;
        if (_instance.PlayerLives > 0)
        {
            GameObject respawn = GameObject.FindWithTag("Respawn");
            GameManager.Instance.GameOver = false;
            if (!GameObject.FindWithTag("Player"))
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
}
