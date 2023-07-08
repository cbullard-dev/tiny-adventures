using UnityEngine;


public class MainMenu : MonoBehaviour
{

 

  private void Start()
  {
    
  }

  public void QuitGame()
  {
    Application.Quit();
  }

  public void PlayGame()
  {
    GameManager.Instance.GameOver = false;
    GameManager.Instance.PlayerScore = 0;
    GameManager.Instance.LoadLevelByName("Level1");
  }
}
