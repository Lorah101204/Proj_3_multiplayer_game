using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Services.Relay;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TextMeshProUGUI countPlayersText;
    private void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            joinCodeText.text = LobbyRelayManager.Instance.LobbyCode;
            Debug.Log("Host in lobby — waiting for players...");
        }
        
        UpdatePlayerCount();
    }

    private void UpdatePlayerCount()
    {
        int currentPlayers = NetworkManager.Singleton.ConnectedClientsIds.Count;
        countPlayersText.text = $"{currentPlayers}/4";
    }
}
