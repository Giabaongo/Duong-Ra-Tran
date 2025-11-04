using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;
    public Image healthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth; // Khởi tạo máu đầy
            
        if (healthBar == null)
        {
            Debug.LogError("❌ HealthBar Image chưa được assign trong Inspector!");
        }
        else
        {
            Debug.Log("✅ HealthBar Image đã được assign!");
            Debug.Log("   - HealthBar Name: " + healthBar.gameObject.name);
            Debug.Log("   - HealthBar Type: " + healthBar.type);
            Debug.Log("   - HealthBar FillMethod: " + healthBar.fillMethod);
            UpdateHealthBar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Không cần update mỗi frame nữa vì đã update trong TakeDamage()
        // if (healthBar != null)
        // {
        //     float newFillAmount = Mathf.Clamp01(health / maxHealth);
        //     healthBar.fillAmount = newFillAmount;
        // }
        
        // Check for death
        if (health <= 0)
        {
            Die();
        }
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0); // Không để health < 0
        Debug.Log("💔 Player took " + damage + " damage. Current HP: " + health + "/" + maxHealth);
        
        // Force update health bar ngay lập tức
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float newFillAmount = Mathf.Clamp01(health / maxHealth);
            healthBar.fillAmount = newFillAmount;
            
            Debug.Log("📊 Updating HealthBar:");
            Debug.Log("   - Health: " + health + "/" + maxHealth);
            Debug.Log("   - Calculated FillAmount: " + newFillAmount);
            Debug.Log("   - Actual FillAmount: " + healthBar.fillAmount);
            Debug.Log("   - HealthBar GameObject: " + healthBar.gameObject.name);
            Debug.Log("   - HealthBar Active: " + healthBar.gameObject.activeInHierarchy);
        }
        else
        {
            Debug.LogError("❌ HealthBar is NULL! Cannot update!");
        }
    }
    
    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth); // Không để health > maxHealth
        Debug.Log("💚 Player healed " + amount + ". Current HP: " + health);
        UpdateHealthBar();
    }
    
    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: Implement death logic (game over screen, respawn, etc.)
    }
}
