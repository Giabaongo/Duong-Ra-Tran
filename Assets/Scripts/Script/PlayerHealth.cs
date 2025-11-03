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
        if (health <= 0)
            health = maxHealth;
            
        if (healthBar == null)
        {
            Debug.LogWarning("HealthBar Image chưa được assign trong Inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
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
        Debug.Log("Player took " + damage + " damage. Current HP: " + health);
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
