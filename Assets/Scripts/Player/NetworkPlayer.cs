using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(Rigidbody2D))]
public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private PlayerStateMachine stateMachine;
    [SerializeField] private Rigidbody2D rb;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            stateMachine.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            stateMachine.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
