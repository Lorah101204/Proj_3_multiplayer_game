using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;

public class LobbyRelayManager : MonoBehaviour
{
    public static LobbyRelayManager Instance { get; private set; }

    public Lobby CurrentLobby { get; private set; }
    public string RelayJoinCode { get; private set; }
    public string LobbyCode => CurrentLobby?.LobbyCode;

    [Header("Config")] public int MaxPlayers = 4;
    public float HeartbeatInterval = 15f;

    private float _heartbeatTimer;

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

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    private void HandleLobbyHeartbeat()
    {
        if (CurrentLobby == null) return;
        if (CurrentLobby.HostId != AuthenticationService.Instance.PlayerId) return;

        _heartbeatTimer -= Time.deltaTime;
        if (_heartbeatTimer <= 0f)
        {
            _heartbeatTimer = HeartbeatInterval;
            _ = SendHeartbeat();
        }
    }

    private async Task SendHeartbeat()
    {
        try
        {
            await LobbyService.Instance.SendHeartbeatPingAsync(CurrentLobby.Id);
        }
        catch (Exception e)
        {
            Debug.LogError($"Lobby heartbeat failed: {e.Message}");
        }
    }

    public async Task CreateLobby(string lobbyName)
    {
        try
        {
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MaxPlayers, new CreateLobbyOptions
            {
                IsPrivate = false
            });

            CurrentLobby = lobby;
            Debug.Log($"Lobby created — Code: {lobby.LobbyCode}");

            await CreateRelayAllocation();
        }
        catch (Exception e)
        {
            Debug.LogError($"CreateLobby error: {e.Message}");
        }
    }

    public async Task JoinLobby(string lobbyCode)
    {
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            CurrentLobby = lobby;
            Debug.Log("Joined lobby!");

            if (lobby.Data.ContainsKey("joinCode"))
            {
                RelayJoinCode = lobby.Data["joinCode"].Value;
                Debug.Log($"Joining relay with code: {RelayJoinCode}");
                // Game start handled outside
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"JoinLobby error: {e.Message}");
        }
    }

    private async Task CreateRelayAllocation()
    {
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(MaxPlayers - 1);
            RelayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log($"Relay created. Join Code: {RelayJoinCode}");

            await LobbyService.Instance.UpdateLobbyAsync(CurrentLobby.Id, new UpdateLobbyOptions
            {
                Data = new System.Collections.Generic.Dictionary<string, DataObject>
                {
                    {
                        "joinCode", new DataObject(DataObject.VisibilityOptions.Public, RelayJoinCode)
                    }
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay create error: {e.Message}");
        }
    }

    public async Task DeleteLobby()
    {
        if (CurrentLobby == null) return;
        if (CurrentLobby.HostId != AuthenticationService.Instance.PlayerId) return;

        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(CurrentLobby.Id);
            Debug.Log("Lobby deleted.");
        }
        catch (Exception e)
        {
            Debug.LogError($"DeleteLobby error: {e.Message}");
        }
        finally
        {
            CurrentLobby = null;
        }
    }
}
