using UnityEngine;
using Unity.Netcode;

public class LevelWinTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D triggerCollider;
    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        triggered = true;
        triggerCollider.enabled = false;
        GameplayBootstrap.Instance.OnLevelWin();
    }
}
