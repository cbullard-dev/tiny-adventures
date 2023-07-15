using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public LevelButton[] levelButtons;
    private void OnEnable()
    {
        var level = 0;
        for (var i = 0; i < GameManager.Instance.TotalScenes; i++)
        {
            if (!GameManager.Instance.PlayableLevel(i)) continue;
            var levelName = (level+1).ToString();
            levelButtons[level].gameObject.SetActive(true);
            levelButtons[level].buttonText.text = levelName;
            levelButtons[level].GetComponent<Button>().onClick.RemoveAllListeners();
            levelButtons[level].GetComponent<Button>().onClick.AddListener(() => SelectLevel("Level"+levelName));
            level++;
        }
    }

    private static void SelectLevel(string levelName)
    {
        GameManager.Instance.GameOver = false;
        GameManager.Instance.LoadLevelByName(levelName);
    }
    
}
