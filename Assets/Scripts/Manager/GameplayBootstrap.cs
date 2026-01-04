using UnityEngine;
using Unity.Netcode;

public class GameplayBootstrap : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        SpawnLevel();
    }

    private void SpawnLevel()
    {
        LevelData levelData = LevelManager.Instance.levelData.GetLevelData(LevelsPopup.SelectedLevelIndex);
        GameObject level = Instantiate(levelData.levelPrefab);
        level.GetComponent<NetworkObject>().Spawn();
    }
}
