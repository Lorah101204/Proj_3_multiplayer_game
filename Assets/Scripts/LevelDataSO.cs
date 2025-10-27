using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Level Data")]
public class LevelDataSO : ScriptableObject
{
    public List<LevelData> levels;

    public LevelData GetLevelData(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Count)
            return levels[levelIndex];
        return null;
    }

    public int GetLevelByID(int id)
    {
        var leveldata = levels.FirstOrDefault(level => level.levelID == id);
        return leveldata.level;
    }

    public string GetLevelByIndexD(int i)
    {
        var levelData = levels[i];
        return levelData.levelName;
    }

    public int GetTotalLevels() => levels.Count;
}

[Serializable]
public class LevelData
{
    public int levelID;
    public int level;
    public string levelName;
    public GameObject levelPrefab;
}
