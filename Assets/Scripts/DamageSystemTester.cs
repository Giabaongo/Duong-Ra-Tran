using UnityEngine;

/// <summary>
/// Script test nhanh - Attach vào Player để test damage system
/// Nhấn phím T để test damage
/// </summary>
public class DamageSystemTester : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private float testDamage = 25f;
    [SerializeField] private KeyCode testKey = KeyCode.T;
    
    private void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            TestDamageSystem();
        }
    }

    private void TestDamageSystem()
    {
        Debug.Log("=== TESTING DAMAGE SYSTEM ===");
        
        // Tìm tất cả enemies
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        
        if (enemies.Length == 0)
        {
            Debug.LogWarning("Không tìm thấy Enemy nào trong scene!");
            return;
        }
        
        Debug.Log($"Tìm thấy {enemies.Length} enemy(s)");
        
        // Test damage lên enemy gần nhất
        EnemyHealth nearestEnemy = null;
        float nearestDistance = float.MaxValue;
        
        foreach (EnemyHealth enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        
        if (nearestEnemy != null)
        {
            Debug.Log($"Testing damage on nearest enemy: {nearestEnemy.name} at distance: {nearestDistance:F2}");
            nearestEnemy.TakeDamage(testDamage);
        }
    }
}
