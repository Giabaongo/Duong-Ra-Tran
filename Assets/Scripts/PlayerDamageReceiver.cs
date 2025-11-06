using UnityEngine;

/// <summary>
/// Gắn vào collider nhận damage của người chơi, chuyển sát thương tới PlayerHealth.
/// </summary>
public class PlayerDamageReceiver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

    private void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerDamageReceiver: không tìm thấy PlayerHealth!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBulletScript bullet = other.GetComponent<EnemyBulletScript>();
        if (other.CompareTag("EnemyBullet") || bullet != null)
        {
            if (playerHealth != null)
            {
                float damage = bullet != null ? bullet.DamageAmount : 10f;
                playerHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("PlayerDamageReceiver: playerHealth is NULL!");
            }
        }

        DamageSource damageSource = other.GetComponent<DamageSource>();
        if (damageSource != null && damageSource.CompareTag("EnemyWeapon"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageSource.DamageAmount);
            }
        }
    }
}

