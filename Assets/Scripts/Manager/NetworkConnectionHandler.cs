using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkConnectionHandler : MonoBehaviour
{
    public static NetworkConnectionHandler Instance;

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

    private void OnEnable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }

    private void OnClientDisconnect(ulong clientId)
    {
        Debug.Log($"[NetworkConnectionHandler] Client {clientId} disconnected");

        if (clientId == 0 && NetworkManager.Singleton != null && !NetworkManager.Singleton.IsServer)
        {
            Debug.Log("[NetworkConnectionHandler] HOST DISCONNECTED! Returning to menu...");
            HandleHostDisconnect();
        }
    }

    private void HandleHostDisconnect()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        if (LobbyRelayManager.Instance != null)
        {
            LobbyRelayManager.Instance.CurrentLobby = null;
        }

        if (NetworkGameController.Instance != null)
        {
            Destroy(NetworkGameController.Instance.gameObject);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.MENU);
    }

    public void RegisterConnectionCallbacks()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }
    }
}
