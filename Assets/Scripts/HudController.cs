using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField]  private Text livesText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameManager.Instance.PlayerScore.ToString();
        livesText.text = GameManager.Instance.PlayerLives.ToString();
    }
}
