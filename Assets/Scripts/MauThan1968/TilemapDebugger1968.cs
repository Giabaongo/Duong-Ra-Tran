using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Debug helper để kiểm tra setup của Tilemap
/// Attach vào GameObject có Tilemap để xem thông tin
/// </summary>
public class TilemapDebugger1968 : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("==================== TILEMAP DEBUG INFO ====================");
        Debug.Log($"GameObject Name: {gameObject.name}");
        Debug.Log($"Tag: {gameObject.tag}");
        Debug.Log($"Layer: {LayerMask.LayerToName(gameObject.layer)} (index: {gameObject.layer})");
        
        // Check Tilemap
        Tilemap tilemap = GetComponent<Tilemap>();
        if (tilemap != null)
        {
            Debug.Log($"✅ Tilemap found");
        }
        else
        {
            Debug.LogWarning("⚠️ No Tilemap component found");
        }
        
        // Check TilemapCollider2D
        TilemapCollider2D tilemapCollider = GetComponent<TilemapCollider2D>();
        if (tilemapCollider != null)
        {
            Debug.Log($"✅ TilemapCollider2D found:");
            Debug.Log($"   - Enabled: {tilemapCollider.enabled}");
            Debug.Log($"   - Is Trigger: {tilemapCollider.isTrigger}");
            Debug.Log($"   - Offset: {tilemapCollider.offset}");
            
            if (tilemapCollider.isTrigger)
            {
                Debug.LogError("❌ TilemapCollider2D is set to TRIGGER! This will cause bullets to pass through!");
                Debug.LogError("   FIX: Uncheck 'Is Trigger' on TilemapCollider2D component!");
            }
        }
        else
        {
            Debug.LogError("❌ NO TilemapCollider2D found!");
            Debug.LogError("   FIX: Add TilemapCollider2D component to this GameObject!");
        }
        
        // Check CompositeCollider2D (optional but recommended)
        CompositeCollider2D compositeCollider = GetComponent<CompositeCollider2D>();
        if (compositeCollider != null)
        {
            Debug.Log($"✅ CompositeCollider2D found:");
            Debug.Log($"   - Enabled: {compositeCollider.enabled}");
            Debug.Log($"   - Is Trigger: {compositeCollider.isTrigger}");
            Debug.Log($"   - Geometry Type: {compositeCollider.geometryType}");
        }
        else
        {
            Debug.LogWarning("⚠️ No CompositeCollider2D (optional, but recommended for performance)");
        }
        
        // Check Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Debug.Log($"✅ Rigidbody2D found:");
            Debug.Log($"   - Body Type: {rb.bodyType}");
            if (rb.bodyType != RigidbodyType2D.Static)
            {
                Debug.LogWarning($"⚠️ Body Type is {rb.bodyType}, should be Static for walls!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ No Rigidbody2D (required if using CompositeCollider2D)");
        }
        
        Debug.Log("============================================================");
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[TilemapDebugger] 🔴 Tilemap collided with: {collision.gameObject.name}");
        Debug.Log($"   - Tag: {collision.gameObject.tag}");
        Debug.Log($"   - Layer: {LayerMask.LayerToName(collision.gameObject.layer)}");
    }
}

