using UnityEngine;

public class LevelsPopup : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private LevelButton levelButtonPrefab;

    private void OnEnable()
    {
        BuildLevelButtons();
    }

    private void BuildLevelButtons()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        int currentUnlocked = PlayerPrefs.GetInt(GameConstant.CURRENT_LEVEL_INDEX, 0);
        int total = LevelManager.Instance.levelData.GetTotalLevels();

        for (int i = 0; i < total; i++)
        {
            LevelButton button = Instantiate(levelButtonPrefab, contentParent);
            bool isLocked = i > currentUnlocked;
            button.SetUp(i, isLocked, OnLevelSelected);
        }
    }

    private void OnLevelSelected(int index)
    {
        Debug.Log($"Selected level: {index}");
        var levelData = LevelManager.Instance.levelData.GetLevelData(index);
        SceneManager.LoadScene(SceneName.GAMEPLAY);
        PlayerPrefs.SetInt(GameConstant.CURRENT_LEVEL_INDEX, index);
    }
}
