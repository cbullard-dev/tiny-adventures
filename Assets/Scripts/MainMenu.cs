using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{

  // [SerializeField] private GameObject playGameButton, levelSelectButton;

  public void QuitGame()
  {
    Application.Quit();
  }

  public void PlayGame()
  {
    GameManager.Instance.GameOver = false;
    GameManager.Instance.LoadLevel(2);
  }
}
