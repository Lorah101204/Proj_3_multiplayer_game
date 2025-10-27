using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level Data")]
public class LevelDataSO : ScriptableObject
{
    public string levelName;
    public int levelID;
    public int maxPlayers;
}
