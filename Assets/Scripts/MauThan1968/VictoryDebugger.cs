using UnityEngine;
using TMPro;

/// <summary>
/// Script debug để hiển thị số enemy đã giết TRÊN SCREEN
/// Attach vào Canvas
/// </summary>
public class VictoryDebugger : MonoBehaviour
{
    [Header("Debug Display")]
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private bool showDebug = true;
    
    private GameManager1968 gameManager;
    
    private void Start()
    {
        gameManager = GameManager1968.Instance;
        
        // Tạo debug text nếu chưa có
        if (debugText == null && showDebug)
        {
            CreateDebugText();
        }
    }
    
    private void Update()
    {
        if (!showDebug || debugText == null || gameManager == null) return;
        
        int killed = gameManager.GetEnemiesKilled();
        int total = gameManager.GetTotalEnemies();
        
        // Đếm enemy còn sống trong scene
        int aliveCount = CountAliveEnemies();
        
        debugText.text = $"DEBUG INFO:\n" +
                        $"Killed: {killed}/{total}\n" +
                        $"Alive in scene: {aliveCount}\n" +
                        $"Victory: {gameManager.HasWon()}\n" +
                        $"Game Over: {gameManager.IsGameOver()}";
        
        // WARNING nếu số không khớp
        if (killed + aliveCount != total && total > 0)
        {
            debugText.text += $"\n⚠️ WARNING: Numbers don't match!";
            debugText.text += $"\n{killed} + {aliveCount} ≠ {total}";
        }
    }
    
    private int CountAliveEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = 0;
        
        foreach (var enemy in enemies)
        {
            var health = enemy.GetComponent<EnemyHealth1968>();
            if (health != null && !health.IsDead())
            {
                count++;
            }
        }
        
        return count;
    }
    
    private void CreateDebugText()
    {
        // Tạo Text object
        GameObject textObj = new GameObject("DebugText");
        textObj.transform.SetParent(transform, false);
        
        debugText = textObj.AddComponent<TextMeshProUGUI>();
        debugText.fontSize = 24;
        debugText.color = Color.yellow;
        debugText.alignment = TextAlignmentOptions.TopLeft;
        
        // Position ở góc trên bên trái
        RectTransform rt = debugText.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(10, -10);
        rt.sizeDelta = new Vector2(400, 200);
        
        Debug.Log("[VictoryDebugger] Created debug text display!");
    }
    
    [ContextMenu("Manual Count Check")]
    public void ManualCountCheck()
    {
        Debug.Log("=== MANUAL COUNT CHECK ===");
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log($"Total GameObjects with 'Enemy' tag: {enemies.Length}");
        
        int validCount = 0;
        int aliveCount = 0;
        
        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];
            var health = enemy.GetComponent<EnemyHealth1968>();
            
            if (health != null)
            {
                validCount++;
                bool isDead = health.IsDead();
                if (!isDead) aliveCount++;
                
                Debug.Log($"{i + 1}. {enemy.name} - HP: {health.GetCurrentHealth()}/{health.GetMaxHealth()} - Dead: {isDead}");
            }
            else
            {
                Debug.LogWarning($"{i + 1}. {enemy.name} - NO EnemyHealth1968!");
            }
        }
        
        Debug.Log($"Valid enemies: {validCount}");
        Debug.Log($"Alive enemies: {aliveCount}");
        
        if (gameManager != null)
        {
            Debug.Log($"GameManager killed count: {gameManager.GetEnemiesKilled()}");
            Debug.Log($"GameManager total count: {gameManager.GetTotalEnemies()}");
        }
    }
}

