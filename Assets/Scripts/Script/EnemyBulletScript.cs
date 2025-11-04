using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // ✅ BẬT CONTINUOUS COLLISION DETECTION để không xuyên qua collider
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.gravityScale = 0; // Đảm bảo không bị rơi
            Debug.Log("🔫 Bullet Rigidbody2D: Collision Detection = Continuous");
        }
        
        // Kiểm tra collider
        Collider2D bulletCollider = GetComponent<Collider2D>();
        if (bulletCollider != null)
        {
            bulletCollider.isTrigger = true; // Đảm bảo là trigger
            Debug.Log("🔫 Bullet Collider: Is Trigger = " + bulletCollider.isTrigger);
        }
        else
        {
            Debug.LogError("❌ Bullet không có Collider2D! Thêm CircleCollider2D hoặc BoxCollider2D vào bullet prefab!");
        }
        
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null)
        {
            Debug.LogError("❌ Không tìm thấy Player! Đảm bảo Player có tag 'Player'");
            return;
        }

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
        
        Debug.Log("🔫 Bullet fired! Velocity: " + rb.linearVelocity + " | Speed: " + force);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("🔫 Bullet collision with: " + other.gameObject.name + " | Tag: " + other.tag + " | Layer: " + LayerMask.LayerToName(other.gameObject.layer));
        
        // Kiểm tra nếu trúng Player hoặc TakeDamageCollider của Player
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("✅ Detected Player tag!");
            
            // Thử tìm PlayerHealth trực tiếp
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            
            // Nếu không có (vì đây là child collider), tìm từ parent
            if (playerHealth == null)
            {
                playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();
                Debug.Log("Searching PlayerHealth in parent...");
            }
            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
                Debug.Log("🎯 Enemy bullet hit player! Damage: 10");
            }
            else
            {
                Debug.LogWarning("⚠️ Player doesn't have PlayerHealth component!");
            }
            
            Destroy(gameObject);
            return;
        }
        
        // Hủy đạn khi chạm tường hoặc obstacle
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("💥 Bullet hit wall/obstacle");
            Destroy(gameObject);
        }
    }
}
