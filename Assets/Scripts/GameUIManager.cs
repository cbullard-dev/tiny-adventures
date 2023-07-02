using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GameUIManager : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private Image healthBar;
  [SerializeField] private GameObject PauseMenu, resumeButton;
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
    GameManager.Instance.Resume();
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
      SetMenuInput(resumeButton);
    }

    if (!GameManager.Instance.isPaused)
    {
      PauseMenu.SetActive(false);
    }
  }

  private void SetMenuInput(GameObject buttonObject)
  {
    EventSystem.current.firstSelectedGameObject = null;
    EventSystem.current.firstSelectedGameObject = buttonObject;
  }
}
