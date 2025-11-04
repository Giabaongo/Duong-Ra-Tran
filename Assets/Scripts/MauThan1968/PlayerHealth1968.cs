using UnityEngine;

public class PlayerHealth1968 : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    
    [Header("Damage Settings")]
    [SerializeField] private float invincibilityTime = 0.5f; // Thời gian bất tử sau khi nhận sát thương
    private float invincibilityTimer = 0f;
    private bool isInvincible = false;
    
    [Header("Visual Feedback (Optional)")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color damageColor = Color.red;
    private Color originalColor;
    
    private bool isDead = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
        
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    
    private void Update()
    {
        // Cập nhật timer bất tử
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = originalColor;
                }
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible) return;
        
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}/{maxHealth}");
        
        // Hiệu ứng nhận sát thương
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
        }
        
        // Kích hoạt thời gian bất tử
        isInvincible = true;
        invincibilityTimer = invincibilityTime;
        
        // Kiểm tra nếu chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log("💀💀💀 PLAYER HAS DIED! 💀💀💀");
        
        // Gọi Game Over từ GameManager
        if (GameManager1968.Instance != null)
        {
            Debug.Log("[PlayerHealth] Calling GameManager1968.GameOver()...");
            GameManager1968.Instance.GameOver();
        }
        else
        {
            Debug.LogError("[PlayerHealth] ❌ GameManager1968.Instance is NULL!");
            Debug.LogError("→ Kiểm tra GameManager1968 có trong scene không!");
        }
        
        // Vô hiệu hóa các component khác của player (optional)
        DisablePlayerControls();
    }
    
    private void DisablePlayerControls()
    {
        // Tắt các script điều khiển player
        var movement = GetComponent<MonoBehaviour>();
        if (movement != null)
        {
            // Tìm và tắt script di chuyển của bạn
            var movementScripts = GetComponents<MonoBehaviour>();
            foreach (var script in movementScripts)
            {
                if (script != this && script.GetType().Name.Contains("Movement") || 
                    script.GetType().Name.Contains("Player"))
                {
                    script.enabled = false;
                }
            }
        }
        
        // Tắt Rigidbody2D nếu có
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }
    
    // Phương thức để hồi máu (nếu cần)
    public void Heal(int amount)
    {
        if (isDead) return;
        
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Player healed {amount}. Current health: {currentHealth}/{maxHealth}");
    }
    
    // Collision với enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        
        Debug.Log($"[PlayerHealth] Collision detected with: {collision.gameObject.name} (Tag: {collision.gameObject.tag})");
        
        // Kiểm tra nếu va chạm với enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            int damage = 10; // Sát thương mặc định
            
            // Thử lấy sát thương từ enemy script nếu có
            var enemy = collision.gameObject.GetComponent<IEnemy>();
            if (enemy != null)
            {
                damage = enemy.GetDamage();
                Debug.Log($"[PlayerHealth] Enemy damage: {damage}");
            }
            else
            {
                Debug.LogWarning($"[PlayerHealth] Enemy {collision.gameObject.name} doesn't have IEnemy component, using default damage: {damage}");
            }
            
            TakeDamage(damage);
        }
        else
        {
            Debug.Log($"[PlayerHealth] Collision object doesn't have 'Enemy' tag");
        }
    }
    
    // Trigger với enemy (nếu dùng trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        
        // Kiểm tra nếu va chạm với enemy
        if (collision.CompareTag("Enemy"))
        {
            int damage = 10; // Sát thương mặc định
            
            // Thử lấy sát thương từ enemy script nếu có
            var enemy = collision.GetComponent<IEnemy>();
            if (enemy != null)
            {
                damage = enemy.GetDamage();
            }
            
            TakeDamage(damage);
        }
    }
    
    // Getters
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsDead() => isDead;
    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
}

// Interface cho Enemy (tạo file riêng nếu cần)
public interface IEnemy
{
    int GetDamage();
}

