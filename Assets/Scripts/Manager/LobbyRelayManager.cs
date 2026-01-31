using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class LobbyRelayManager : MonoBehaviour
{
    public static LobbyRelayManager Instance;

    public Lobby CurrentLobby;
    public string RelayJoinCode;
    public string LobbyCode => CurrentLobby?.LobbyCode;

    public Action OnHostReady;
    public Action OnClientReady;

    private async void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("UGS Ready");
    }

    public async Task CreateLobbyAndRelay(int maxPlayers = 4)
    {
        try
        {
            // --- Create Relay allocation ---
            var alloc = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);
            RelayJoinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);

            Debug.Log($"[Relay] Join Code = {RelayJoinCode}");

            CurrentLobby = await LobbyService.Instance.CreateLobbyAsync(
                "My Coop Room",
                maxPlayers,
                new CreateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { "joinCode", new DataObject(DataObject.VisibilityOptions.Public, RelayJoinCode) }
                    }
                });

            Debug.Log($"[Lobby] Created: {CurrentLobby.Id}");
            Debug.Log($"[Lobby] Lobby Code (SHARE THIS): {CurrentLobby.LobbyCode}");
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(
                alloc.RelayServer.IpV4,
                (ushort)alloc.RelayServer.Port,
                alloc.AllocationIdBytes,
                alloc.Key,
                alloc.ConnectionData
            );

            NetworkManager.Singleton.StartHost();
            Debug.Log("[Netcode] HOST started");
            
            // Ensure NetworkConnectionHandler is created and registered
            EnsureNetworkConnectionHandler();
            
            OnHostReady?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"CreateLobbyAndRelay FAILED: {e}");
            throw;
        }
    }

    // ================= CLIENT ==================
    public async Task JoinLobbyAndRelay(string lobbyCode)
    {
        try
        {
            CurrentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            Debug.Log($"[Lobby] Joined lobby: {CurrentLobby.Id}");
            Debug.Log($"[Lobby] Host Relay Code = {CurrentLobby.Data["joinCode"].Value}");

            string relayCode = CurrentLobby.Data["joinCode"].Value;

            var alloc = await RelayService.Instance.JoinAllocationAsync(relayCode);

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(
                alloc.RelayServer.IpV4,
                (ushort)alloc.RelayServer.Port,
                alloc.AllocationIdBytes,
                alloc.Key,
                alloc.ConnectionData,
                alloc.HostConnectionData
            );

            NetworkManager.Singleton.StartClient();
            Debug.Log("[Netcode] CLIENT started");
            EnsureNetworkConnectionHandler();

            OnClientReady?.Invoke();
        }
        catch (LobbyServiceException le)
        {
            Debug.LogError($"JoinLobby failed: {le.Reason} - {le.Message}");
            throw;
        }
        catch (Exception e)
        {
            Debug.LogError($"JoinLobbyAndRelay FAILED: {e}");
            throw;
        }
    }

    /// <summary>
    /// Ensure NetworkConnectionHandler exists and is registered
    /// </summary>
    private void EnsureNetworkConnectionHandler()
    {
        NetworkConnectionHandler.Instance.RegisterConnectionCallbacks();
    }
}
