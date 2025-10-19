using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;

    void Start()
    {
        if (health <= 0)
            health = 100;
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
        health -= amount;
        Debug.Log("Enemy HP: " + health);
    }

    void Die()
    {
        Debug.Log("Enemy Defeated");
        Destroy(gameObject);
    }
}
