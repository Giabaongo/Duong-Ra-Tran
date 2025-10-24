using UnityEngine;

/// <summary>
/// Script để debug collision - attach vào Player hoặc Enemy để xem collision events
/// </summary>
public class CollisionDebugHelper : MonoBehaviour
{
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] private bool showGizmos = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[{gameObject.name}] OnTriggerEnter2D with: {other.name} (Tag: {other.tag}, Layer: {LayerMask.LayerToName(other.gameObject.layer)})");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Comment out to reduce spam
        // Debug.Log($"[{gameObject.name}] OnTriggerStay2D with: {other.name}");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"[{gameObject.name}] OnTriggerExit2D with: {other.name}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[{gameObject.name}] OnCollisionEnter2D with: {collision.gameObject.name} (Tag: {collision.gameObject.tag})");
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, col.bounds.size);
        }
    }
}
