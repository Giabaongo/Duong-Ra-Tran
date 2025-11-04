using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private bool spawnAtStart = true;
    
    private float lastSpawnTime;
    private int currentEnemyCount = 0;
    
    void Start()
    {
        // ★★★ COMPLETELY DISABLED ★★★
        // No enemy spawning at all!
        // Scene already has 6 enemies placed manually
        return;
        
        /* ORIGINAL CODE (DISABLED):
        if (spawnAtStart && spawnPoints.Length > 0)
        {
            SpawnEnemy();
        }
        */
    }
    
    void Update()
    {
        // ★★★ COMPLETELY DISABLED ★★★
        // No auto-spawn, no respawn!
        return;
        
        /* ORIGINAL CODE (DISABLED):
        // Auto spawn enemies if below max
        if (currentEnemyCount < maxEnemies && 
            Time.time - lastSpawnTime >= spawnInterval)
        {
            SpawnEnemy();
        }
        */
    }
    
    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab not assigned to EnemySpawner!");
            return;
        }
        
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned to EnemySpawner!");
            return;
        }
        
        // Choose random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // Spawn enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        
        // Track enemy count
        currentEnemyCount++;
        lastSpawnTime = Time.time;
        
        Debug.Log($"Enemy spawned at {spawnPoint.position}. Total enemies: {currentEnemyCount}");
        
        // Listen for enemy death to decrease count
        Enermy1968Controller enemyController = enemy.GetComponent<Enermy1968Controller>();
        if (enemyController != null)
        {
            // We'll use OnDestroy in enemy to notify spawner
            StartCoroutine(WaitForEnemyDeath(enemy));
        }
    }
    
    private System.Collections.IEnumerator WaitForEnemyDeath(GameObject enemy)
    {
        // Wait until enemy is destroyed
        yield return new WaitUntil(() => enemy == null);
        
        // Decrease count when enemy dies
        currentEnemyCount--;
        Debug.Log($"Enemy died. Remaining enemies: {currentEnemyCount}");
    }
    
    // Public method to spawn enemy at specific position
    public void SpawnEnemyAt(Vector3 position)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab not assigned to EnemySpawner!");
            return;
        }
        
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        currentEnemyCount++;
        
        Debug.Log($"Enemy spawned at {position}. Total enemies: {currentEnemyCount}");
        
        // Track enemy
        StartCoroutine(WaitForEnemyDeath(enemy));
    }
    
    // Getters
    public int GetCurrentEnemyCount() => currentEnemyCount;
    public int GetMaxEnemies() => maxEnemies;
    
    // Debug visualization
    private void OnDrawGizmos()
    {
        if (spawnPoints == null) return;
        
        Gizmos.color = Color.magenta;
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
                Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + Vector3.up * 1f);
            }
        }
    }
}

