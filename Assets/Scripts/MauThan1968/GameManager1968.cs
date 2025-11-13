using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1968 : MonoBehaviour
{
    [Header("Game Over Settings")]
    [SerializeField] private bool autoRestart = false; // Tự động restart hay chờ người chơi nhấn nút
    [SerializeField] private float restartDelay = 3f;
    private bool isGameOver = false;
    
    [Header("Victory Settings")]
    [SerializeField] private bool autoCountEnemies = true; // Tự động đếm enemy?
    [SerializeField] private int manualEnemyCount = 6; // Số enemy nếu không auto count
    private int totalEnemies = 0; // Tổng số enemy cần tiêu diệt
    private int enemiesKilled = 0;
    private bool hasWon = false;
    
    [Header("UI References")]
    [SerializeField] private GameObject gameOverUI; // Assign in Inspector if you have UI
    [SerializeField] private GameObject victoryUI; // Assign Victory UI Panel
    
    private static GameManager1968 instance;
    
    private void Awake()
    {
        // ★★★ CRITICAL FIX: Clear dead enemies NGAY KHI SCENE LOAD! ★★★
        // Điều này đảm bảo enemies luôn spawn lại, BẤT KỂ cách nào load scene!
        EnemyHealth1968.ClearDeadEnemies();
        Debug.Log("[GameManager1968] 🔄 Dead enemies cleared in Awake!");
        
        // Singleton pattern (optional)
        if (instance == null)
        {
            instance = this;
            Debug.Log($"[GameManager1968] Initialized on GameObject: {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"[GameManager1968] Duplicate instance found on {gameObject.name}, destroying...");
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        // Hide game over UI at start
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
            Debug.Log($"[GameManager1968] Game Over UI '{gameOverUI.name}' hidden at start");
        }
        else
        {
            Debug.LogError("[GameManager1968] ❌ GAME OVER UI NOT ASSIGNED! Kéo Game Over Panel vào Inspector!");
        }
        
        // Hide victory UI at start
        if (victoryUI != null)
        {
            victoryUI.SetActive(false);
            Debug.Log($"[GameManager1968] Victory UI '{victoryUI.name}' hidden at start");
        }
        else
        {
            Debug.LogWarning("[GameManager1968] ⚠️ VICTORY UI NOT ASSIGNED! Kéo Victory Panel vào Inspector!");
        }
        
        if (autoCountEnemies)
        {
            // Đếm số enemy SAU 2 giây để đảm bảo TẤT CẢ enemy clone đã spawn
            Invoke(nameof(CountEnemies), 2f);
            Debug.Log("[GameManager1968] ⏳ Auto counting enemies in 2 seconds...");
        }
        else
        {
            // Dùng số enemy thủ công
            totalEnemies = manualEnemyCount;
            Debug.Log($"[GameManager1968] 📊 Manual enemy count set to: {totalEnemies}");
        }
    }
    
    public void GameOver()
    {
        if (isGameOver)
        {
            Debug.LogWarning("[GameManager1968] GameOver already called!");
            return;
        }
        
        isGameOver = true;
        Debug.Log("=== GAME OVER ===");
        
        // Show game over UI if available
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Debug.Log($"[GameManager1968] ✅ Game Over UI '{gameOverUI.name}' is now VISIBLE");
            
            // Kiểm tra xem UI có thực sự active không
            if (!gameOverUI.activeInHierarchy)
            {
                Debug.LogError("[GameManager1968] ❌ Game Over UI set to active but still not visible! Check if parent is disabled!");
            }
        }
        else
        {
            Debug.LogError("[GameManager1968] ❌ Cannot show Game Over UI - NOT ASSIGNED in Inspector!");
        }
        
        // Dừng thời gian (optional - bỏ comment nếu muốn)
        // Time.timeScale = 0f;
        
        // Restart game after delay (nếu bật autoRestart)
        if (autoRestart)
        {
            Debug.Log($"[GameManager1968] Auto restart in {restartDelay} seconds...");
            Invoke(nameof(RestartGame), restartDelay);
        }
        else
        {
            Debug.Log("[GameManager1968] Waiting for player to press Restart button...");
        }
    }
    
    public void RestartGame()
    {
        Debug.Log("🔄 Restarting game...");
        
        // ★ CRITICAL FIX: Clear dead enemies tracking TRƯỚC KHI reload scene!
        EnemyHealth1968.ClearDeadEnemies();
        
        // Đặt lại time scale nếu đã dừng
        Time.timeScale = 1f;
        
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        Debug.Log("✅ Scene reloaded! Enemies will spawn again!");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    private void CountEnemies()
    {
        // Tự động đếm số enemy có tag "Enemy" trong scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        // CHỈ đếm enemy CÓ EnemyHealth1968 (đảm bảo là enemy thật)
        int validEnemyCount = 0;
        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<EnemyHealth1968>() != null)
            {
                validEnemyCount++;
            }
        }
        
        totalEnemies = validEnemyCount;
        // KHÔNG reset enemiesKilled nếu đang chơi game
        if (enemiesKilled == 0)
        {
            // Chỉ reset khi bắt đầu game
        }
        
        Debug.Log($"[GameManager1968] 📊 Counting enemies in scene...");
        Debug.Log($"[GameManager1968] Total VALID enemies found: {totalEnemies}");
        Debug.Log($"[GameManager1968] Currently killed: {enemiesKilled}");
        
        // List ra tên các enemy hợp lệ
        int count = 1;
        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<EnemyHealth1968>() != null)
            {
                Debug.Log($"[GameManager1968]   {count}. {enemy.name}");
                count++;
            }
        }
    }
    
    // Method public để recount nếu cần
    [ContextMenu("Recount Enemies")]
    public void RecountEnemies()
    {
        CountEnemies();
    }
    
    public void EnemyKilled()
    {
        if (hasWon || isGameOver)
        {
            Debug.LogWarning("[GameManager1968] EnemyKilled called but game already ended!");
            return;
        }
        
        enemiesKilled++;
        Debug.Log($"[GameManager1968] ⚔️ Enemy killed! Progress: {enemiesKilled}/{totalEnemies}");
        
        // Kiểm tra điều kiện thắng
        if (enemiesKilled >= totalEnemies)
        {
            Debug.Log($"[GameManager1968] Victory condition met: {enemiesKilled} >= {totalEnemies}");
            Victory();
        }
        else
        {
            Debug.Log($"[GameManager1968] {totalEnemies - enemiesKilled} enemies remaining...");
        }
    }
    
    public void Victory()
    {
        if (hasWon || isGameOver) return;
        
        hasWon = true;
        Debug.Log("🎉🎉🎉 === VICTORY! === 🎉🎉🎉");
        
        // Show victory UI if available
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
            Debug.Log($"[GameManager1968] ✅ Victory UI '{victoryUI.name}' is now VISIBLE");
            
            // Kiểm tra xem UI có thực sự active không
            if (!victoryUI.activeInHierarchy)
            {
                Debug.LogError("[GameManager1968] ❌ Victory UI set to active but still not visible! Check if parent is disabled!");
            }
        }
        else
        {
            Debug.LogWarning("[GameManager1968] ⚠️ Cannot show Victory UI - NOT ASSIGNED in Inspector!");
        }
        
        // Dừng thời gian (optional)
        // Time.timeScale = 0f;
        
        // Auto restart nếu bật
        if (autoRestart)
        {
            Debug.Log($"[GameManager1968] Auto restart in {restartDelay} seconds...");
            Invoke(nameof(RestartGame), restartDelay);
        }
        else
        {
            Debug.Log("[GameManager1968] Waiting for player to press button...");
        }
    }
    
    public bool IsGameOver() => isGameOver;
    public bool HasWon() => hasWon;
    public int GetEnemiesKilled() => enemiesKilled;
    public int GetTotalEnemies() => totalEnemies;
    
    public static GameManager1968 Instance => instance;
}

