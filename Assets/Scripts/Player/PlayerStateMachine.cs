using UnityEngine;
using Unity.Netcode;

public class PlayerStateMachine : NetworkBehaviour
{
    public PlayerMovement Movement { get; private set; }
    public InputReader Input { get; private set; }

    public int jumpCount = 0;

    private PlayerState currentState;

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
        Input = GetComponent<InputReader>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        if (!IsOwner) return;

        currentState?.LogicUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        newState.Enter();
    }
}

public class IdleState : PlayerState
{
    public IdleState(PlayerStateMachine sm) : base(sm) { }

    public override void LogicUpdate()
    {
        movement.SetMove(input.MoveInput.x);

        if (Mathf.Abs(input.MoveInput.x) > 0.01f)
            stateMachine.ChangeState(new MoveState(stateMachine));

        if (input.JumpPressed)
        {
            stateMachine.jumpCount = 1;
            movement.RequestJump();
            stateMachine.ChangeState(new JumpState(stateMachine));
        }
    }
}

public class MoveState : PlayerState
{
    public MoveState(PlayerStateMachine sm) : base(sm) { }

    public override void LogicUpdate()
    {
        movement.SetMove(input.MoveInput.x);

        if (Mathf.Abs(input.MoveInput.x) <= 0.01f)
            stateMachine.ChangeState(new IdleState(stateMachine));

        if (input.JumpPressed)
        {
            stateMachine.jumpCount = 1;
            movement.RequestJump();
            stateMachine.ChangeState(new JumpState(stateMachine));
        }
    }
}

public class JumpState : PlayerState
{
    public JumpState(PlayerStateMachine sm) : base(sm) { }

    public override void LogicUpdate()
    {
        movement.SetMove(input.MoveInput.x);

        if (movement.IsGrounded)
        {
            stateMachine.jumpCount = 0;
            stateMachine.ChangeState(new IdleState(stateMachine));
        }
    }
}

