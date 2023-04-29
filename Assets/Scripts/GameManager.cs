using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    [SerializeField] private int PlayerDefaultLives = 3;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.Log("Creating new GameManager Instance");
                GameObject gameManagerInstance = new GameObject("GameManager");
                gameManagerInstance.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        TotalScenes = SceneManager.sceneCountInBuildSettings;
    }

    public int PlayerScore { get; set; }
    public int PlayerLives { get; set; } = GameManager.Instance.PlayerDefaultLives;
    public int TotalScenes { get; private set; }
    public bool PlayerAlive { get; set; }
    public bool GameOver { get; set; }

    public void LoadLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    public void Respawn()
    {
        GameManager.Instance.PlayerLives--;
        if (_instance.PlayerLives > 0)
        {
            GameManager.Instance.GameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            GameManager.Instance.PlayerLives = PlayerDefaultLives;
            SceneManager.LoadScene(0);
        }
    }
}
