using Unity.Netcode;
using UnityEngine;

public class LevelGameObj : NetworkBehaviour
{ 
    [SerializeField] private LevelWinTrigger levelWinTrigger;

    public void OnWinLevel()
    {
        levelWinTrigger.gameObject.SetActive(false);
        GetComponent<NetworkObject>().Despawn();
    }
}