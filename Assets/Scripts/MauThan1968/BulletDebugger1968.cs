using UnityEngine;

/// <summary>
/// Debug helper để kiểm tra setup của Bullet
/// Attach vào Bullet prefab để xem thông tin chi tiết
/// </summary>
public class BulletDebugger1968 : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("==================== BULLET DEBUG INFO ====================");
        Debug.Log($"Bullet Name: {gameObject.name}");
        Debug.Log($"Bullet Tag: {gameObject.tag}");
        Debug.Log($"Bullet Layer: {LayerMask.LayerToName(gameObject.layer)} (index: {gameObject.layer})");
        
        // Check Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Debug.Log($"✅ Rigidbody2D found:");
            Debug.Log($"   - Body Type: {rb.bodyType}");
            Debug.Log($"   - Simulated: {rb.simulated}");
            Debug.Log($"   - Collision Detection: {rb.collisionDetectionMode}");
            Debug.Log($"   - Gravity Scale: {rb.gravityScale}");
            Debug.Log($"   - Constraints: {rb.constraints}");
        }
        else
        {
            Debug.LogError("❌ NO RIGIDBODY2D FOUND!");
        }
        
        // Check Colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        if (colliders.Length > 0)
        {
            Debug.Log($"✅ Found {colliders.Length} Collider(s):");
            foreach (var col in colliders)
            {
                Debug.Log($"   - Type: {col.GetType().Name}");
                Debug.Log($"   - Is Trigger: {col.isTrigger}");
                Debug.Log($"   - Enabled: {col.enabled}");
                Debug.Log($"   - Offset: {col.offset}");
            }
        }
        else
        {
            Debug.LogError("❌ NO COLLIDER2D FOUND!");
        }
        
        Debug.Log("==========================================================");
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[BulletDebugger] 🔴 COLLISION with: {collision.gameObject.name}");
        Debug.Log($"   - Tag: {collision.gameObject.tag}");
        Debug.Log($"   - Layer: {LayerMask.LayerToName(collision.gameObject.layer)}");
        Debug.Log($"   - Contact Points: {collision.contactCount}");
        
        // Check collider của đối tượng va chạm
        Collider2D col = collision.collider;
        if (col != null)
        {
            Debug.Log($"   - Collider Type: {col.GetType().Name}");
            Debug.Log($"   - Is Trigger: {col.isTrigger}");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[BulletDebugger] 🟡 TRIGGER with: {collision.gameObject.name}");
        Debug.Log($"   - Tag: {collision.gameObject.tag}");
        Debug.Log($"   - Layer: {LayerMask.LayerToName(collision.gameObject.layer)}");
        Debug.Log($"   - Collider Type: {collision.GetType().Name}");
        Debug.Log($"   - Is Trigger: {collision.isTrigger}");
    }
}

