using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        GameManager.Instance.LoadLevel(1);
    }
}
