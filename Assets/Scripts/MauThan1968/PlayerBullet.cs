using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;
    
    private Vector2 direction;
    private Rigidbody2D rb;
    
    void Awake()
    {
        // Awake runs BEFORE Start and BEFORE any other scripts can call methods
        rb = GetComponent<Rigidbody2D>();
        
        // Debug: Check components
        Collider2D bulletCollider = GetComponent<Collider2D>();
        Debug.Log($"★ PlayerBullet spawned! Has RB: {rb != null}, Has Collider: {bulletCollider != null}, Is Trigger: {(bulletCollider != null ? bulletCollider.isTrigger : false)}");
        
        // Ignore collision with player who shot this bullet
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null && bulletCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
                Debug.Log("★ Ignored collision with player");
            }
        }
        
        // Auto destroy after lifetime
        Destroy(gameObject, lifeTime);
    }
    
    // Set bullet direction (called by player when shooting)
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
            Debug.Log($"★ PlayerBullet velocity set to: {rb.linearVelocity}");
        }
        else
        {
            Debug.LogError("★ PlayerBullet: Rigidbody2D is NULL!");
        }
        
        // Rotate bullet sprite to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collision with player
        if (collision.CompareTag("Player"))
        {
            return;
        }
        
        // Ignore collision with other player bullets (check by name)
        if (collision.gameObject.name.Contains("Bullet_Player"))
        {
            return;
        }
        
        Debug.Log($"Player bullet hit: {collision.gameObject.name} with tag: {collision.tag}");
        
        // Try to damage enemy by checking for component first
        Enermy1968Controller enemy = collision.GetComponent<Enermy1968Controller>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("★★★ Player bullet hit enemy! Enemy taking damage! ★★★");
            Destroy(gameObject);
            return;
        }
        
        // Check if hit enemy by tag as backup
        if (collision.CompareTag("Enemy"))
        {
            Debug.LogWarning("Enemy tag found but Enermy1968Controller component is missing!");
            Destroy(gameObject);
            return;
        }
        
        // Destroy if hit wall or obstacle
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}

