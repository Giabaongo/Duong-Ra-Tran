using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 5f;
    
    void Start()
    {
        // ★ FIX: Set continuous collision detection (prevent tunneling)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        
        // Auto destroy after lifetime
        Destroy(gameObject, lifeTime);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ★ DEBUG: Log mọi va chạm
        Debug.Log($"★★★ ENEMY BULLET HIT: {collision.gameObject.name}, Layer: {collision.gameObject.layer} ({LayerMask.LayerToName(collision.gameObject.layer)}), Tag: '{collision.tag}'");
        
        // Check if hit player
        if (collision.CompareTag("Player"))
        {
            // Try to damage player
            LinhGiaiPhong1968 player = collision.GetComponent<LinhGiaiPhong1968>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Enemy bullet hit player!");
            }
            
            // Destroy bullet
            Destroy(gameObject);
        }
        // Destroy if hit wall or obstacle (check if tag exists first)
        else if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}

