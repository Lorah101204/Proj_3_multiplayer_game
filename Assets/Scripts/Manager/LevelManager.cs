using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public LevelDataSO levelData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private int GetCurrentLevelIndex()
    {
        return PlayerPrefs.GetInt(GameConstant.CURRENT_LEVEL_INDEX, 0);
    }

    public void UnlockNextLevel()
    {
        int currentLevelIndex = GetCurrentLevelIndex();
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex < levelData.GetTotalLevels())
        {
            PlayerPrefs.SetInt(GameConstant.CURRENT_LEVEL_INDEX, nextLevelIndex);
            PlayerPrefs.Save();
        }
    }

    public LevelData GetCurrentLevelData()
    {
        int currentLevelIndex = GetCurrentLevelIndex();
        return levelData.GetLevelData(currentLevelIndex);
    }
}
