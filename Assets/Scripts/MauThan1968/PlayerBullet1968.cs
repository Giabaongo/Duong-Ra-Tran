using UnityEngine;

/// <summary>
/// Script cho bullet của player
/// Attach vào prefab bullet
/// </summary>
public class PlayerBullet1968 : MonoBehaviour, IBullet
{
    [Header("Bullet Settings")]
    [SerializeField] private int damage = 25;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f; // Tự hủy sau 5 giây
    
    [Header("Target Settings")]
    [SerializeField] private bool destroyOnImpact = true;
    
    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 customDirection = Vector2.zero; // ★ NEW: Custom direction for bullet
    private bool hasCustomDirection = false; // ★ NEW: Flag to check if custom direction is set
    
    private void Awake()
    {
        // Validate setup ngay khi khởi tạo
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        
        if (rb == null)
        {
            Debug.LogError($"[PlayerBullet] ❌ MISSING RIGIDBODY2D on {gameObject.name}! Bullet will not work properly!");
            return;
        }
        
        if (col == null)
        {
            Debug.LogError($"[PlayerBullet] ❌ MISSING COLLIDER2D on {gameObject.name}! Bullet will not collide with anything!");
            return;
        }
        
        // QUAN TRỌNG: Đảm bảo collider KHÔNG phải trigger để có collision
        if (col.isTrigger)
        {
            Debug.LogWarning($"[PlayerBullet] ⚠️ Collider is set to TRIGGER! This may cause issues. Consider using non-trigger collider.");
        }
        
        // Đảm bảo Rigidbody2D setup đúng
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f; // Không bị trọng lực
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Collision detection tốt hơn
        
        Debug.Log($"[PlayerBullet] ✅ Initialized: Tag={gameObject.tag}, Layer={LayerMask.LayerToName(gameObject.layer)}");
    }
    
    private void Start()
    {
        if (rb == null) return; // Không làm gì nếu thiếu components
        
        // ★ NEW: Use custom direction if set, otherwise use transform.right
        Vector2 direction = hasCustomDirection ? customDirection : (Vector2)transform.right;
        
        // Tự động di chuyển theo hướng đã set
        rb.linearVelocity = direction.normalized * speed;
        Debug.Log($"[PlayerBullet] 🚀 Launched with velocity: {rb.linearVelocity} (custom: {hasCustomDirection})");
        
        // Tự hủy sau lifetime giây
        Destroy(gameObject, lifetime);
    }
    
    /// <summary>
    /// ★ NEW: Set custom direction for bullet (call BEFORE Start())
    /// </summary>
    public void SetDirection(Vector2 direction)
    {
        customDirection = direction.normalized;
        hasCustomDirection = true;
        Debug.Log($"[PlayerBullet] ✅ Custom direction set: {customDirection}");
    }
    
    private bool hasHit = false; // Đảm bảo chỉ hit 1 lần
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit)
        {
            Debug.LogWarning($"[PlayerBullet] Already hit something, ignoring {collision.gameObject.name}");
            return;
        }
        
        Debug.Log($"[PlayerBullet] 🎯 Hit: {collision.gameObject.name} (Tag: {collision.gameObject.tag}, Layer: {LayerMask.LayerToName(collision.gameObject.layer)})");
        
        // IGNORE: Không va chạm với Player, PlayerBullet, EnemyBullet
        if (collision.gameObject.CompareTag("Player") || 
            collision.gameObject.CompareTag("PlayerBullet") ||
            collision.gameObject.CompareTag("EnemyBullet") ||
            collision.gameObject.name.Contains("Bullet"))
        {
            Debug.Log($"[PlayerBullet] ⚪ Ignoring collision with: {collision.gameObject.name}");
            return; // Bỏ qua, không làm gì
        }
        
        // Nếu trúng enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hasHit = true;
            
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth1968>();
            if (enemyHealth != null)
            {
                Debug.Log($"[PlayerBullet] ⚔️ Dealing {damage} damage to {collision.gameObject.name}");
                enemyHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogError($"[PlayerBullet] Enemy {collision.gameObject.name} has no EnemyHealth1968!");
            }
            
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
        // Nếu trúng tường hoặc vật cản
        else
        {
            hasHit = true;
            Debug.Log($"[PlayerBullet] 🧱 Hit wall/obstacle: {collision.gameObject.name}, destroying bullet");
            
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
        {
            Debug.LogWarning($"[PlayerBullet] Already hit something, ignoring trigger with {collision.gameObject.name}");
            return;
        }
        
        Debug.Log($"[PlayerBullet] 🎯 Trigger Hit: {collision.gameObject.name} (Tag: {collision.gameObject.tag}, Layer: {LayerMask.LayerToName(collision.gameObject.layer)})");
        
        // IGNORE: CameraBounds/Cameraborder - để bullet có thể bay xuyên qua
        if (collision.gameObject.layer == LayerMask.NameToLayer("Cameraborder") ||
            collision.gameObject.name.Contains("CameraBounds"))
        {
            Debug.Log($"[PlayerBullet] ⚪ Ignoring CameraBounds - bullet passing through");
            return;
        }
        
        // IGNORE: Không va chạm với Player, PlayerBullet, EnemyBullet
        if (collision.CompareTag("Player") || 
            collision.CompareTag("PlayerBullet") ||
            collision.CompareTag("EnemyBullet") ||
            collision.gameObject.name.Contains("Bullet"))
        {
            Debug.Log($"[PlayerBullet] ⚪ Ignoring trigger with: {collision.gameObject.name}");
            return; // Bỏ qua, không làm gì
        }
        
        // Nếu trúng enemy
        if (collision.CompareTag("Enemy"))
        {
            hasHit = true;
            
            var enemyHealth = collision.GetComponent<EnemyHealth1968>();
            if (enemyHealth != null)
            {
                Debug.Log($"[PlayerBullet] ⚔️ Dealing {damage} damage to {collision.gameObject.name}");
                enemyHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogError($"[PlayerBullet] Enemy {collision.gameObject.name} has no EnemyHealth1968!");
            }
            
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
        // Nếu trúng tường hoặc vật cản
        else
        {
            hasHit = true;
            Debug.Log($"[PlayerBullet] 🧱 Trigger hit wall/obstacle: {collision.gameObject.name}, destroying bullet");
            
            if (destroyOnImpact)
            {
                // Dừng chuyển động ngay
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.simulated = false; // Tắt physics
                }
                
                // Tắt collider
                if (col != null)
                {
                    col.enabled = false;
                }
                
                Destroy(gameObject);
            }
        }
    }
    
    // Implement interface IBullet
    public int GetDamage()
    {
        return damage;
    }
    
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}

