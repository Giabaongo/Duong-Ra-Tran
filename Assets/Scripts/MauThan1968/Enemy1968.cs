using UnityEngine;

public class Enemy1968 : MonoBehaviour, IEnemy
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private bool continuousDamage = false; // Bật để gây damage liên tục
    
    [Header("Continuous Damage Settings")]
    [SerializeField] private float damageInterval = 2f; // Thời gian giữa các lần gây sát thương
    private float damageTimer = 0f;
    private bool isCollidingWithPlayer = false;
    private PlayerHealth1968 playerHealth;
    
    private void Update()
    {
        // CHỈ gây sát thương liên tục NẾU BẬT continuousDamage
        if (continuousDamage && isCollidingWithPlayer && playerHealth != null)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                playerHealth.TakeDamage(damageAmount);
                damageTimer = damageInterval;
                Debug.Log($"[Enemy1968] Continuous damage to player");
            }
        }
    }
    
    // Collision với player - CHỈ GÂY DAMAGE 1 LẦN
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // IGNORE: Không gây damage cho Enemy khác
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"[Enemy1968] Ignoring collision with enemy: {collision.gameObject.name}");
            return;
        }
        
        // CHECK: Chỉ gây damage nếu enemy còn sống
        if (!IsAlive())
        {
            Debug.LogWarning($"[Enemy1968] {gameObject.name} is dead, cannot deal damage!");
            return;
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth = collision.gameObject.GetComponent<PlayerHealth1968>();
            if (playerHealth != null)
            {
                // GÂY DAMAGE 1 LẦN khi bắt đầu chạm
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"[Enemy1968] ⚔️ One-time damage {damageAmount} to player!");
                
                // Setup cho continuous damage nếu bật
                if (continuousDamage)
                {
                    isCollidingWithPlayer = true;
                    damageTimer = damageInterval;
                    Debug.Log($"[Enemy1968] Continuous damage enabled, next in {damageInterval}s");
                }
            }
            else
            {
                Debug.LogError($"[Enemy1968] Player doesn't have PlayerHealth1968 component!");
            }
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
            playerHealth = null;
        }
    }
    
    // Trigger với player - CHỈ GÂY DAMAGE 1 LẦN
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // IGNORE: Không gây damage cho Enemy khác
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"[Enemy1968] Ignoring trigger with enemy: {collision.gameObject.name}");
            return;
        }
        
        // CHECK: Chỉ gây damage nếu enemy còn sống
        if (!IsAlive())
        {
            Debug.LogWarning($"[Enemy1968] {gameObject.name} is dead, cannot deal damage!");
            return;
        }
        
        if (collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<PlayerHealth1968>();
            if (playerHealth != null && !isCollidingWithPlayer)
            {
                // GÂY DAMAGE 1 LẦN khi trigger
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"[Enemy1968] ⚔️ Trigger one-time damage {damageAmount} to player!");
                
                // Setup cho continuous damage nếu bật
                if (continuousDamage)
                {
                    isCollidingWithPlayer = true;
                    damageTimer = damageInterval;
                }
            }
        }
    }
    
    // Trigger exit
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"[Enemy1968] Trigger exit Player!");
            isCollidingWithPlayer = false;
            playerHealth = null;
        }
    }
    
    // Implement interface IEnemy
    public int GetDamage()
    {
        return damageAmount;
    }
    
    // Phương thức để set damage từ ngoài (nếu cần)
    public void SetDamage(int damage)
    {
        damageAmount = damage;
    }
    
    // Kiểm tra enemy còn sống không
    private bool IsAlive()
    {
        // Check nếu script này bị disabled
        if (!enabled) return false;
        
        // Check nếu EnemyHealth1968 tồn tại và chưa chết
        var health = GetComponent<EnemyHealth1968>();
        if (health != null)
        {
            return !health.IsDead();
        }
        
        // Nếu không có health script, coi như còn sống
        return true;
    }
}

