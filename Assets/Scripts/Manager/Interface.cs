using System;
using UnityEngine;

public interface IPushable
{
    void Push(Vector2 direction, float force);
}

public interface ISwitchable
{
    void SetState(bool on);
}

public interface ISwitchActivator
{
    bool CanActiveSwitch();
}


public interface IDoor
{
    void Open();
    void Close();
    bool IsOpen { get; }
}

public interface IInteractable
{
    void Interact();
}