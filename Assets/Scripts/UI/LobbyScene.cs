using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Services.Relay;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TextMeshProUGUI countPlayersText;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private InGamePausePopup inGamePausePopup;

    private void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            joinCodeText.text = LobbyRelayManager.Instance.LobbyCode;
            Debug.Log("Host in lobby — waiting for players...");
        }
        ResetPlayerPos();
        UpdatePlayerCount();
    }

    private void UpdatePlayerCount()
    {
        int currentPlayers = NetworkManager.Singleton.ConnectedClientsIds.Count;
        countPlayersText.text = $"{currentPlayers}/4";
    }

    private void ResetPlayerPos()
    {
        playerSpawner.ResetAllPlayers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inGamePausePopup != null)
            {
                inGamePausePopup.TogglePause();
            }
        }
    }
}
