using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private Image healthBar;
  [SerializeField] private GameObject PauseMenu;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    scoreText.text = GameManager.Instance.PlayerScore.ToString();
    healthBar.fillAmount = GameManager.Instance.PlayerLives / 3f;
    GamePaused();
  }

  public void MainMenu()
  {
    GameManager.Instance.LoadMainMenu();
  }

  public void Resume()
  {
    GameManager.Instance.Resume();
  }

  private void GamePaused()
  {
    if (GameManager.Instance.isPaused)
    {
      PauseMenu.SetActive(true);
    }

    if (!GameManager.Instance.isPaused)
    {
      PauseMenu.SetActive(false);
    }
  }
}
