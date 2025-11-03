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
            Debug.Log("✅ HealthBar Image đã được assign! Khởi tạo thanh máu...");
            healthBar.fillAmount = 1f; // Set đầy máu
            Debug.Log("Current HP: " + health + "/" + maxHealth + " | FillAmount: " + healthBar.fillAmount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar != null)
        {
            float newFillAmount = Mathf.Clamp01(health / maxHealth);
            healthBar.fillAmount = newFillAmount;
        }
        
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
        
        if (healthBar != null)
        {
            Debug.Log("📊 HealthBar FillAmount: " + healthBar.fillAmount);
        }
    }
    
    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Min(health, maxHealth); // Không để health > maxHealth
        Debug.Log("Player healed " + amount + ". Current HP: " + health);
    }
    
    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: Implement death logic (game over screen, respawn, etc.)
    }
}
