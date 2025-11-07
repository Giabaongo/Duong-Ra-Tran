using UnityEngine;
using System.Collections.Generic;

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
    
    // ★ ANTI-RESPAWN: Track enemies đã chết để ngăn spawn lại
    private static HashSet<string> deadEnemies = new HashSet<string>();
    private string enemyID;
    
    private void Start()
    {
        // ★ ANTI-RESPAWN: Tạo unique ID cho enemy
        enemyID = $"{gameObject.name}_{transform.position.x:F2}_{transform.position.y:F2}";
        
        // ★ ANTI-RESPAWN: Check nếu enemy này đã chết rồi
        if (deadEnemies.Contains(enemyID))
        {
            Debug.LogWarning($"[EnemyHealth] ⚠️ {gameObject.name} đã chết trước đó! DESTROYING RESPAWN CLONE!");
            Destroy(gameObject);
            return;
        }
        
        // ★ Check nếu là clone (phát hiện respawn) - XÓA NGAY LẬP TỨC!
        if (gameObject.name.Contains("(Clone)"))
        {
            Debug.LogWarning($"[EnemyHealth] ⚠️ Detected CLONE enemy: {gameObject.name}!");
            Debug.LogWarning($"[EnemyHealth] Position: {transform.position}");
            
            // ★ CRITICAL FIX: Bất kỳ enemy nào có "(Clone)" đều là respawn → XÓA!
            Debug.LogError($"[EnemyHealth] 🚫 BLOCKING RESPAWN! Destroying {gameObject.name}");
            Debug.LogError($"[EnemyHealth] → ANY clone is forbidden in this game mode!");
            Destroy(gameObject);
            return;
        }
        
        // ★ BACKUP CHECK: Kiểm tra vị trí gần với enemy đã chết (tolerance 1 unit)
        foreach (string deadID in deadEnemies)
        {
            // Parse position từ ID (format: "Name_X_Y")
            string[] parts = deadID.Split('_');
            if (parts.Length >= 3)
            {
                string deadName = parts[0];
                if (float.TryParse(parts[1], out float deadX) && float.TryParse(parts[2], out float deadY))
                {
                    // Check nếu tên giống và vị trí gần (trong vòng 1 unit)
                    string currentName = gameObject.name.Replace("(Clone)", "");
                    float distX = Mathf.Abs(transform.position.x - deadX);
                    float distY = Mathf.Abs(transform.position.y - deadY);
                    
                    if (deadName == currentName && distX < 1f && distY < 1f)
                    {
                        Debug.LogError($"[EnemyHealth] 🚫 Detected respawn at same position! Destroying!");
                        Debug.LogError($"[EnemyHealth] → Dead at: ({deadX:F2}, {deadY:F2}), Spawn at: ({transform.position.x:F2}, {transform.position.y:F2})");
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }
        
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
        else
        {
            // ★ THÊM: Chạy animation HIT nếu chưa chết
            Debug.Log($"[EnemyHealth] 💥 {gameObject.name} is HIT! Playing hit animation...");
            PlayHitAnimation();
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
        
        // ★ ANTI-RESPAWN: Đánh dấu enemy này đã chết
        if (!string.IsNullOrEmpty(enemyID))
        {
            deadEnemies.Add(enemyID);
            Debug.Log($"[EnemyHealth] 🔒 Locked enemy ID: {enemyID} (Total dead: {deadEnemies.Count})");
        }
        
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
    
    private void PlayHitAnimation()
    {
        // Get animator
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            // ★ FIX: Force play animation thay vì dùng trigger/bool
            // Vì Enemy animator không có Any State → EnemyHit transition
            animator.Play("EnemyHit", 0, 0f);
            Debug.Log($"[EnemyHealth] ✅ Force playing EnemyHit animation for {gameObject.name}");
            
            // Backup: Vẫn set bool để transition work nếu có
            animator.SetBool("isHitting", true);
            
            // Reset hit animation sau 0.35 giây (EnemyHit animation duration)
            Invoke(nameof(ResetHitAnimation), 0.35f);
        }
        else
        {
            Debug.LogWarning($"[EnemyHealth] {gameObject.name} has no Animator!");
        }
        
        // Get Enermy1968Controller và dừng enemy trong lúc hit
        Enermy1968Controller controller = GetComponent<Enermy1968Controller>();
        if (controller != null)
        {
            // Nếu controller có public method/property để set hit state
            // controller.SetHit(true); // Nếu có method này
            Debug.Log($"[EnemyHealth] Found Enermy1968Controller on {gameObject.name}");
        }
    }
    
    private void ResetHitAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isHitting", false);
            Debug.Log($"[EnemyHealth] Reset isHitting = false for {gameObject.name}");
        }
    }
    
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

