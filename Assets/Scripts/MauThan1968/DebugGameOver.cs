using UnityEngine;

/// <summary>
/// Script debug để kiểm tra setup Game Over
/// Attach vào bất kỳ GameObject nào trong scene
/// </summary>
public class DebugGameOver : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] private KeyCode testKey = KeyCode.G; // Nhấn G để test game over
    
    private void Start()
    {
        Debug.Log("=== CHECKING GAME OVER SETUP ===");
        CheckSetup();
    }
    
    private void Update()
    {
        // Nhấn phím để test game over trực tiếp
        if (Input.GetKeyDown(testKey))
        {
            TestGameOver();
        }
    }
    
    private void CheckSetup()
    {
        bool allGood = true;
        
        // 1. Kiểm tra GameManager
        Debug.Log("\n--- STEP 1: Checking GameManager ---");
        GameManager1968 gm = FindObjectOfType<GameManager1968>();
        if (gm == null)
        {
            Debug.LogError("❌ GameManager1968 NOT FOUND in scene!");
            Debug.LogError("→ Cần thêm GameManager1968 vào scene!");
            allGood = false;
        }
        else
        {
            Debug.Log("✅ GameManager1968 found!");
            
            // Kiểm tra Game Over UI reference
            var gameOverUI = gm.GetType()
                .GetField("gameOverUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(gm) as GameObject;
            
            if (gameOverUI == null)
            {
                Debug.LogError("❌ Game Over UI NOT ASSIGNED in GameManager!");
                Debug.LogError("→ Cần kéo Game Over Panel vào ô 'Game Over UI' trong Inspector của GameManager!");
                allGood = false;
            }
            else
            {
                Debug.Log($"✅ Game Over UI assigned: {gameOverUI.name}");
                Debug.Log($"   Current state: {(gameOverUI.activeSelf ? "VISIBLE" : "HIDDEN")}");
            }
        }
        
        // 2. Kiểm tra Player
        Debug.Log("\n--- STEP 2: Checking Player ---");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("❌ Player GameObject with tag 'Player' NOT FOUND!");
            Debug.LogError("→ Cần set Tag của Player GameObject thành 'Player'!");
            allGood = false;
        }
        else
        {
            Debug.Log($"✅ Player found: {player.name}");
            
            // Kiểm tra PlayerHealth1968
            PlayerHealth1968 playerHealth = player.GetComponent<PlayerHealth1968>();
            if (playerHealth == null)
            {
                Debug.LogError("❌ PlayerHealth1968 NOT FOUND on Player!");
                Debug.LogError("→ Cần add component PlayerHealth1968 vào Player GameObject!");
                allGood = false;
            }
            else
            {
                Debug.Log($"✅ PlayerHealth1968 found on Player!");
                Debug.Log($"   Current Health: {playerHealth.GetCurrentHealth()}/{playerHealth.GetMaxHealth()}");
            }
            
            // Kiểm tra Collider
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider == null)
            {
                Debug.LogWarning("⚠️ Player doesn't have Collider2D!");
                Debug.LogWarning("→ Player cần Collider2D để va chạm với Enemy!");
            }
            else
            {
                Debug.Log($"✅ Player has {playerCollider.GetType().Name}");
            }
        }
        
        // 3. Kiểm tra Enemy
        Debug.Log("\n--- STEP 3: Checking Enemies ---");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            Debug.LogWarning("⚠️ No Enemy GameObjects with tag 'Enemy' found!");
            Debug.LogWarning("→ Cần set Tag của Enemy GameObject thành 'Enemy'!");
            
            // Kiểm tra có Enemy1968 components không
            Enemy1968[] enemyScripts = FindObjectsOfType<Enemy1968>();
            if (enemyScripts.Length > 0)
            {
                Debug.LogWarning($"⚠️ Found {enemyScripts.Length} Enemy1968 scripts but they don't have 'Enemy' tag!");
                foreach (var enemy in enemyScripts)
                {
                    Debug.LogWarning($"   → {enemy.gameObject.name} needs tag 'Enemy'!");
                }
            }
        }
        else
        {
            Debug.Log($"✅ Found {enemies.Length} Enemy GameObject(s):");
            foreach (var enemy in enemies)
            {
                Debug.Log($"   - {enemy.name}");
                
                // Kiểm tra Enemy1968 script
                Enemy1968 enemyScript = enemy.GetComponent<Enemy1968>();
                if (enemyScript == null)
                {
                    Debug.LogError($"   ❌ {enemy.name} doesn't have Enemy1968 component!");
                    Debug.LogError($"   → Cần add component Enemy1968 vào {enemy.name}!");
                    allGood = false;
                }
                else
                {
                    Debug.Log($"   ✅ {enemy.name} has Enemy1968 (Damage: {enemyScript.GetDamage()})");
                }
                
                // Kiểm tra Collider
                Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
                if (enemyCollider == null)
                {
                    Debug.LogWarning($"   ⚠️ {enemy.name} doesn't have Collider2D!");
                    Debug.LogWarning($"   → Enemy cần Collider2D để va chạm với Player!");
                }
                else
                {
                    Debug.Log($"   ✅ {enemy.name} has {enemyCollider.GetType().Name}");
                }
            }
        }
        
        // Kết luận
        Debug.Log("\n=== SETUP CHECK COMPLETE ===");
        if (allGood)
        {
            Debug.Log("✅✅✅ ALL SETUP CORRECT! ✅✅✅");
            Debug.Log($"→ Nhấn phím [{testKey}] để test Game Over!");
        }
        else
        {
            Debug.LogError("❌ SETUP INCOMPLETE! Xem các lỗi ở trên!");
        }
        Debug.Log("================================\n");
    }
    
    private void TestGameOver()
    {
        Debug.Log(">>> Testing Game Over manually...");
        
        GameManager1968 gm = GameManager1968.Instance;
        if (gm != null)
        {
            gm.GameOver();
            Debug.Log(">>> Game Over called! Check if UI appears.");
        }
        else
        {
            Debug.LogError(">>> Cannot test: GameManager1968 not found!");
        }
    }
}

