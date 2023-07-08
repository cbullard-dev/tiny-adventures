using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

  [SerializeField] private Slider volumeSlider;

  private void Start()
  {
    volumeSlider.value = AudioListener.volume;
  }

  public void QuitGame()
  {
    Application.Quit();
  }

  public void AdjustVolume(float volume)
  {
    AudioListener.volume = volume;
  }

  public void PlayGame()
  {
    GameManager.Instance.GameOver = false;
    GameManager.Instance.PlayerScore = 0;
    GameManager.Instance.LoadLevelByIndex(2);
  }
}
