using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Services.Relay;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    private void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            joinCodeText.text = LobbyRelayManager.Instance.LobbyCode;
            Debug.Log("Host in lobby — waiting for players...");
        }
    }
}
