using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;

    void Start()
    {
        if (health <= 0)
            health = maxHealth;
        
        Debug.Log("Enemy " + gameObject.name + " initialized with HP: " + health);
        
        // Kiểm tra collider
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("Enemy " + gameObject.name + " không có Collider2D! Thêm BoxCollider2D hoặc CircleCollider2D.");
        }
        else
        {
            Debug.Log("Enemy collider: Is Trigger = " + col.isTrigger + ", Layer = " + LayerMask.LayerToName(gameObject.layer));
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        float oldHealth = health;
        health -= amount;
        Debug.Log("=== DAMAGE TAKEN ===");
        Debug.Log("Enemy: " + gameObject.name);
        Debug.Log("Damage: " + amount);
        Debug.Log("HP: " + oldHealth + " → " + health);
        
        // Visual feedback (optional - có thể thêm flash effect)
        // StartCoroutine(FlashRed());
    }

    void Die()
    {
        Debug.Log("☠ Enemy Defeated: " + gameObject.name);
        GameManager.Instance.EnemyDefeated(); // Gọi phương thức EnemyDefeated từ GameManager
        Destroy(gameObject);
    }
}
