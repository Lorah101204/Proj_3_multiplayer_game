using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : NetworkBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool CrouchHeld { get; private set; }

    private SimpleInputActions input;

    private void Awake()
    {
        input = new SimpleInputActions();

        input.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        input.Move.canceled += _ => MoveInput = Vector2.zero;

        input.Jump.performed += _ => JumpPressed = true;

        input.Crouch.performed += _ => CrouchHeld = true;
        input.Crouch.canceled += _ => CrouchHeld = false;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            input.Enable();
        }
        else
        {
            input.Disable();
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        // Reset jump mỗi frame để tránh spam
        JumpPressed = false;
    }

    private void OnDestroy()
    {
        input.Disable();
    }
}
