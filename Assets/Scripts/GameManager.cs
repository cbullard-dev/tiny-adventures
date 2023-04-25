using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Creating new GameManager Instance");
                GameObject gameManagerInstance = new GameObject("GameManager");
                gameManagerInstance.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public int PlayerScore { get; set; }
    public bool PlayerAlive { get; set; }
    public bool GameOver { get; set; }
}
