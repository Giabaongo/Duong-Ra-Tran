using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private float damageAmount = 25f;
    
    // Public property để truy cập từ bên ngoài
    public float DamageAmount => damageAmount;

    private void OnEnable()
    {
        Debug.Log("DamageSource ENABLED on: " + gameObject.name + " at position: " + transform.position);
    }

    private void OnDisable()
    {
        Debug.Log("DamageSource DISABLED on: " + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("=== COLLISION DETECTED ===");
        Debug.Log("DamageSource triggered by: " + other.name);
        Debug.Log("Tag: " + other.tag);
        Debug.Log("Layer: " + LayerMask.LayerToName(other.gameObject.layer));
        Debug.Log("Position: " + other.transform.position);

        // Kiểm tra xem object có component EnemyHealth không
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        
        if (enemyHealth != null)
        {
            Debug.Log("✓ HIT ENEMY: " + other.name + " - Applying " + damageAmount + " damage!");
            enemyHealth.TakeDamage(damageAmount);
        }
        else
        {
            Debug.LogWarning("✗ Object " + other.name + " không có EnemyHealth component!");
            
            // Kiểm tra parent có EnemyHealth không
            EnemyHealth parentHealth = other.transform.parent?.GetComponent<EnemyHealth>();
            if (parentHealth != null)
            {
                Debug.Log("✓ Found EnemyHealth on parent! Applying damage...");
                parentHealth.TakeDamage(damageAmount);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Log để biết weapon collider vẫn đang chạm enemy
        Debug.Log("Still colliding with: " + other.name);
    }
}
