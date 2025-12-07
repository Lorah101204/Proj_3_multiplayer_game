using UnityEngine;

public class BoxPush : MonoBehaviour, IPushable
{
    [SerializeField] private float factor = 1f;
    [SerializeField] private Rigidbody2D rb;

    public void Push(Vector2 direction, float force)
    {
        rb.linearVelocity = new Vector2(direction.x * force / factor, rb.linearVelocity.y);
    }
}
