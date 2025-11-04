using UnityEngine;

/// <summary>
/// Script này gắn vào GameObject có TakeDamageCollider
/// Khi bị trúng đạn, nó sẽ forward damage lên PlayerHealth của parent
/// </summary>
public class PlayerDamageReceiver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    
    private void Awake()
    {
        // Tự động tìm PlayerHealth từ parent nếu chưa assign
        if (playerHealth == null)
        {
            playerHealth = GetComponentInParent<PlayerHealth>();
        }
        
        if (playerHealth == null)
        {
            Debug.LogError("PlayerDamageReceiver: Không tìm thấy PlayerHealth! Assign trong Inspector hoặc đảm bảo script này là child của Player.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu là đạn của enemy
        if (other.CompareTag("EnemyBullet") || other.GetComponent<EnemyBulletScript>() != null)
        {
            Debug.Log("🎯 PlayerDamageReceiver: Trúng đạn enemy! GameObject: " + other.gameObject.name);
            
            // Forward damage lên PlayerHealth
            if (playerHealth != null)
            {
                // Đạn enemy gây 10 damage (hoặc lấy từ bullet script)
                float damage = 10f;
                
                // Thử lấy damage từ bullet script nếu có
                EnemyBulletScript bullet = other.GetComponent<EnemyBulletScript>();
                if (bullet != null)
                {
                    // Nếu bullet có property damage thì dùng, không thì dùng 10
                    damage = 10f; // Mặc định
                }
                
                playerHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("PlayerDamageReceiver: playerHealth is NULL!");
            }
        }
        
        // Kiểm tra nếu là vũ khí của enemy (melee attack)
        DamageSource damageSource = other.GetComponent<DamageSource>();
        if (damageSource != null && damageSource.CompareTag("EnemyWeapon"))
        {
            Debug.Log("⚔️ PlayerDamageReceiver: Bị enemy đánh! GameObject: " + other.gameObject.name);
            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageSource.DamageAmount);
            }
        }
    }
}
