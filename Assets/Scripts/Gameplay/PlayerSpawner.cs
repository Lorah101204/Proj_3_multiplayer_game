using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private Transform spawnPoints;

    public void ResetAllPlayers()
    {
        int index = 0;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            NetworkObject player = client.PlayerObject;
            if (player == null) continue;
     
            player.transform.SetPositionAndRotation(spawnPoints.position, spawnPoints.rotation);
            index++;
        }
    }
}
