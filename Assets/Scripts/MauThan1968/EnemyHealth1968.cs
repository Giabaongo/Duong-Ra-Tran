using UnityEngine;

public class EnemyHealth1968 : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 50;
    private int currentHealth; // KHÔNG serialize để tránh lưu giá trị cũ
    
    [Header("Visual Feedback (Optional)")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float damageFlashDuration = 0.1f;
    private Color originalColor;
    private bool isFlashing = false;
    
    [Header("Death Effects (Optional)")]
    [SerializeField] private GameObject deathEffect; // Particle effect khi chết
    [SerializeField] private float deathDelay = 0.2f; // Delay trước khi destroy
    [SerializeField] private bool fadeOutOnDeath = true; // Fade out khi chết
    [SerializeField] private bool hideImmediately = true; // Ẩn ngay lập tức
    
    private bool isDead = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
        
        // 🔍 DEBUG: Kiểm tra health ban đầu
        Debug.Log($"[EnemyHealth] {gameObject.name} initialized with MaxHP: {maxHealth}, CurrentHP: {currentHealth}");
        
        if (maxHealth <= 0)
        {
            Debug.LogError($"[EnemyHealth] ❌ {gameObject.name} has maxHealth = {maxHealth}! This will cause instant death!");
            Debug.LogError($"[EnemyHealth] → Fix: Set maxHealth > 0 in Inspector or prefab!");
        }
        
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        
        // 🔍 Kiểm tra ngay xem enemy có chết luôn không
        if (currentHealth <= 0)
        {
            Debug.LogError($"[EnemyHealth] ❌ {gameObject.name} spawned with HP <= 0, calling Die()!");
            Die();
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            Debug.LogWarning($"[EnemyHealth] {gameObject.name} already dead, ignoring damage!");
            return;
        }
        
        // ⚔️ TRỪ HEALTH (QUAN TRỌNG!)
        currentHealth -= damage;
        
        // 🔍 DEBUG: In ra SAU KHI trừ máu
        Debug.Log($"[EnemyHealth] 💥 {gameObject.name} took {damage} damage. HP: {currentHealth}/{maxHealth}");
        
        // Hiệu ứng nhận sát thương
        if (spriteRenderer != null && !isFlashing)
        {
            StartCoroutine(DamageFlash());
        }
        
        // Kiểm tra chết
        if (currentHealth <= 0)
        {
            Debug.Log($"[EnemyHealth] {gameObject.name} HP reached 0, calling Die()...");
            Die();
        }
    }
    
    private System.Collections.IEnumerator DamageFlash()
    {
        isFlashing = true;
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
        isFlashing = false;
    }
    
    private void Die()
    {
        if (isDead)
        {
            Debug.LogWarning($"[EnemyHealth] {gameObject.name} Die() called but already dead!");
            return;
        }
        
        isDead = true;
        Debug.Log($"💀💀💀 [EnemyHealth] {gameObject.name} has been killed!");
        
        // Thông báo cho GameManager (CHỈ 1 LẦN)
        if (GameManager1968.Instance != null)
        {
            Debug.Log($"[EnemyHealth] Notifying GameManager about {gameObject.name} death...");
            GameManager1968.Instance.EnemyKilled();
        }
        else
        {
            Debug.LogError("[EnemyHealth] GameManager1968 not found!");
        }
        
        // TẮT CÁC COMPONENTS TRƯỚC TIÊN (QUAN TRỌNG!)
        DisableEnemy();
        
        // ẨN NGAY LẬP TỨC nếu bật hideImmediately
        if (hideImmediately && spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
            Debug.Log($"[EnemyHealth] {gameObject.name} sprite hidden immediately!");
        }
        
        // Spawn death effect nếu có
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        
        // Fade out effect HOẶC destroy ngay
        if (fadeOutOnDeath && spriteRenderer != null && !hideImmediately)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            // Destroy ngay
            Destroy(gameObject, deathDelay);
        }
    }
    
    private System.Collections.IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color startColor = spriteRenderer.color;
        
        while (elapsed < deathDelay)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / deathDelay);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        
        // Destroy sau khi fade xong
        Destroy(gameObject);
    }
    
    private void DisableEnemy()
    {
        Debug.Log($"[EnemyHealth] Disabling {gameObject.name} completely...");
        
        // TẮT TẤT CẢ COLLIDERS NGAY LẬP TỨC (QUAN TRỌNG!)
        Collider2D[] allColliders = GetComponents<Collider2D>();
        foreach (var col in allColliders)
        {
            if (col != null)
            {
                col.enabled = false;
                Debug.Log($"[EnemyHealth] Disabled collider: {col.GetType().Name}");
            }
        }
        
        // Tắt các scripts khác
        Enemy1968 enemyScript = GetComponent<Enemy1968>();
        if (enemyScript != null)
        {
            enemyScript.enabled = false;
            Debug.Log($"[EnemyHealth] Disabled Enemy1968 script");
        }
        
        // Tắt movement scripts nếu có
        var movementScripts = GetComponents<MonoBehaviour>();
        foreach (var script in movementScripts)
        {
            if (script != this && 
                (script.GetType().Name.Contains("Movement") || 
                 script.GetType().Name.Contains("AI") ||
                 script.GetType().Name.Contains("Enemy")))
            {
                script.enabled = false;
            }
        }
        
        // Tắt Rigidbody2D nếu có
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }
        
        // ĐẶT LAYER VÀO IGNORE (backup safety)
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }
    
    // XÓA METHOD NÀY - Bullet tự xử lý damage rồi!
    // Collision với bullet (DISABLED - PlayerBullet1968 handles this)
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // KHÔNG DÙNG - Bullet script tự xử lý
    }
    */
    
    // XÓA METHOD NÀY - Bullet tự xử lý damage rồi!
    // Trigger với bullet (DISABLED - PlayerBullet1968 handles this)
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // KHÔNG DÙNG - Bullet script tự xử lý
    }
    */
    
    // Getters
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsDead() => isDead;
    public float GetHealthPercentage() => (float)currentHealth / maxHealth;
}

// Interface cho Bullet (tạo file riêng nếu cần)
public interface IBullet
{
    int GetDamage();
}

