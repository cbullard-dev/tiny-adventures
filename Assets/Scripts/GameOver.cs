using UnityEngine;

public class GameOver : MonoBehaviour
{
  public void MainMenu()
  {
    GameManager.Instance.Resume();
    GameManager.Instance.LoadMainMenu();
  }
}
