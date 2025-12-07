using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerMovement movement;
    protected InputReader input;

    public PlayerState(PlayerStateMachine sm)
    {
        stateMachine = sm;
        movement = sm.Movement;
        input = sm.Input;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void LogicUpdate() { }
}
