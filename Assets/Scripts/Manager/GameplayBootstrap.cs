using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class GameplayBootstrap : NetworkBehaviour
{
    public static GameplayBootstrap Instance;

    [SerializeField] private PlayerSpawner playerSpawner;

    private LevelGameObj currentLevel;
    
    private int levelIndex => NetworkGameController.Instance.GetCurrentLevelIndex();

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        SpawnLevel(levelIndex);
        StartCoroutine(ResetPlayers());
    }


    public void SpawnLevel(int levelIndex)
    {
        LevelData levelData = LevelManager.Instance.levelData.GetLevelData(levelIndex);

        GameObject level = Instantiate(levelData.levelPrefab);
        level.GetComponent<NetworkObject>().Spawn();
        currentLevel = level.GetComponent<LevelGameObj>();

        Debug.Log($"Spawned Level: {levelData.levelID} at index {levelIndex}");
    }

    public void OnLevelWin()
    {
        currentLevel.OnWinLevel();
        NetworkGameController.Instance.OnLevelWin();
    }

    private IEnumerator ResetPlayers()
    {
        yield return new WaitForEndOfFrame();
        playerSpawner.ResetAllPlayers();
    }

    public void ResetAllPlayersRpc()
    {
        playerSpawner.ResetAllPlayers();
    }
}
