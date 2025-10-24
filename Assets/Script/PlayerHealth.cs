using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public Image healthBar;

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
        if (health <= 0)
        {
            Debug.Log("Player Defeated");
            // Có thể thêm hiệu ứng chết, load lại scene...
        }
    }
}
