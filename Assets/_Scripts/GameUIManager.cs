using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Serialization;

public class GameUIManager : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private Image healthBar;
  [FormerlySerializedAs("PauseMenu")] [SerializeField] private GameObject pauseMenu;
  [SerializeField] private GameObject resumeButton;

  [SerializeField] private Slider volumeSlider;
  // Start is called before the first frame update
  void Awake()
  {
    volumeSlider.value = AudioListener.volume;
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

  public void ChangeVolume(float volume)
  {
    AudioListener.volume = volume;
  }

  private void GamePaused()
  {
    if (GameManager.Instance.IsPaused)
    {
      pauseMenu.SetActive(true);
      SetMenuInput(resumeButton);
    }

    if (!GameManager.Instance.IsPaused)
    {
      pauseMenu.SetActive(false);
    }
  }

  private void SetMenuInput(GameObject buttonObject)
  {
    EventSystem.current.firstSelectedGameObject = null;
    EventSystem.current.firstSelectedGameObject = buttonObject;
  }
}
