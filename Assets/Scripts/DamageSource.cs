using UnityEngine;

public class DameSource : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (collision.CompareTag("Enemy"))
        //{
        //    Debug.Log("Hit enemy: " + collision.name);
        //    // Here you can add code to apply damage to the enemy
        //}

        if (other.gameObject.GetComponent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            Debug.Log("Hit enemy: " + other.name);
            enemyHealth.TakeDamage(25f); // Apply 25 damage to the enemy
        }
    }
}
