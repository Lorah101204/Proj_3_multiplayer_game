using UnityEngine;
using DesignPattern;
using System.Diagnostics;

public abstract class SwitchBase : MonoBehaviour, ISwitchable
{
    protected bool currentState = false;

    public virtual void SetState(bool isOn)
    {
        if (currentState == isOn) return;

        currentState = isOn;
        EventDispatcher.Instance.PostEvent(EventID.OnSwitchChanged, this);
    }

    public bool GetState() => currentState;
}
