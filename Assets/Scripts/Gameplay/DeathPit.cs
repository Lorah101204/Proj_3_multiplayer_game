using UnityEngine;

public class DeathPit : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 1f;
    
    private bool isOnCooldown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOnCooldown) return;
        
        // Reset all players when any player falls into the pit
        if (GameplayBootstrap.Instance != null)
        {
            GameplayBootstrap.Instance.ResetAllPlayersRpc();
            StartCoroutine(CooldownCoroutine());
        }
    }

    private System.Collections.IEnumerator CooldownCoroutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}
