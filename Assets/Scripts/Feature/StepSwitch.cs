using UnityEngine;

public class StepSwitch : SwitchBase
{
    private int activatorCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ISwitchActivator>(out var activator))
        {
            if (activator.CanActiveSwitch())
            {
                activatorCount++;
                SetState(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<ISwitchActivator>(out var activator))
        {
            if (activator.CanActiveSwitch())
            {
                activatorCount--;
                if (activatorCount <= 0)
                {
                    activatorCount = 0;
                    SetState(false);
                }
            }
        }
    }
}
