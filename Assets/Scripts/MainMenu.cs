using UnityEngine;

public class MainMenu : MonoBehaviour
{

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
