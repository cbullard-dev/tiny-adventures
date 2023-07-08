using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public class MainMenu : MonoBehaviour
{

[SerializeField] private EventSystem eventSystem;
[SerializeField] private GameObject mainMenu, optionsMenu, levelSelectMenu,mainMenuInitialButton,optionsInitialButton,levelSelectInitialButton;

private GameObject _currentlyEnabledMenu;


private void Awake()
  {
    _currentlyEnabledMenu = mainMenu;
    eventSystem.SetSelectedGameObject(mainMenuInitialButton);
    eventSystem.firstSelectedGameObject = null;
    eventSystem.firstSelectedGameObject = mainMenuInitialButton;
  }

  public void PlayGame()
  {
    GameManager.Instance.GameOver = false;
    GameManager.Instance.PlayerScore = 0;
    GameManager.Instance.LoadLevelByName("Level1");
  }

  public void OptionsMenu(GameObject button)
  {
    _currentlyEnabledMenu.SetActive(false);
    optionsMenu.SetActive(true);
    _currentlyEnabledMenu = optionsMenu;
    eventSystem.SetSelectedGameObject(optionsInitialButton);
    eventSystem.firstSelectedGameObject = null;
    eventSystem.firstSelectedGameObject = optionsInitialButton;
    mainMenuInitialButton = button;
  }

  public void LevelsMenu(GameObject button)
  {
    _currentlyEnabledMenu.SetActive(false);
    levelSelectMenu.SetActive(true);
    _currentlyEnabledMenu = levelSelectMenu;
    eventSystem.SetSelectedGameObject(levelSelectInitialButton);
    eventSystem.firstSelectedGameObject = null;
    eventSystem.firstSelectedGameObject = levelSelectInitialButton;
    mainMenuInitialButton = button;
  }

  public void BackButton()
  {
    _currentlyEnabledMenu.SetActive(false);
    mainMenu.SetActive(true);
    _currentlyEnabledMenu = mainMenu;
    eventSystem.SetSelectedGameObject(mainMenuInitialButton);
    eventSystem.firstSelectedGameObject = null;
    eventSystem.firstSelectedGameObject = mainMenuInitialButton;
  }
  
  public void QuitGame()
  {
    Application.Quit();
  }
  
}
