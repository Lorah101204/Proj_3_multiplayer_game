using UnityEngine;
using DesignPattern;

public abstract class DoorBase : MonoBehaviour, IDoor
{
    [SerializeField] private SwitchBase linkedSwitch;

    public bool IsOpen { get; private set; }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.OnSwitchChanged, OnSwitchChanged);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.OnSwitchChanged, OnSwitchChanged);
    }

    private void OnSwitchChanged(object param)
    {
        if (param is SwitchBase sw)
        {
            if (sw == linkedSwitch)
            {
                if (sw.GetState()) Open();
                else Close();
            }
        }
    }

    public virtual void Open()
    {
        IsOpen = true;
    }

    public virtual void Close()
    {
        IsOpen = false;
    }
}
