using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;

public class NetworkGameController : NetworkBehaviour
{
    public static NetworkGameController Instance;

    private NetworkVariable<int> currentLevelIndex = new NetworkVariable<int>();
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public override void OnNetworkSpawn()
    {
    }

    public void SetCurrentLevel(int index)
    {
        if (!IsServer) return;
        currentLevelIndex.Value = index;
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex.Value;
    }

    public void OnLevelWin()
    {
        if (!IsServer) return;
        if (isTransitioning) return;

        isTransitioning = true;

        Debug.Log("Level Won!");
        LevelManager.Instance.UnlockNextLevel();
        currentLevelIndex.Value++;

        if (currentLevelIndex.Value >=
            LevelManager.Instance.levelData.GetTotalLevels())
        {
            Debug.Log("ALL LEVELS COMPLETED");
            return;
        }
        StartCoroutine(LoadLobby());
    }


    private IEnumerator LoadLobby()
    {
        yield return null;
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName.LOBBY, LoadSceneMode.Single);
    }
}
    