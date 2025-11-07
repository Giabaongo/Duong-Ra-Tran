using UnityEngine;

/// <summary>
/// Script tự động fix tag cho enemy clones
/// Attach vào GameObject bất kỳ hoặc vào GameManager
/// </summary>
public class EnemyTagFixer : MonoBehaviour
{
    [Header("Auto Fix Settings")]
    [SerializeField] private bool fixOnStart = true;
    [SerializeField] private string requiredTag = "Enemy";
    
    private void Start()
    {
        if (fixOnStart)
        {
            FixAllEnemyTags();
        }
    }
    
    [ContextMenu("Fix All Enemy Tags")]
    public void FixAllEnemyTags()
    {
        Debug.Log("=== FIXING ENEMY TAGS ===");
        
        int fixedCount = 0;
        
        // Tìm tất cả GameObject có EnemyHealth1968 hoặc Enemy1968
        EnemyHealth1968[] enemyHealths = FindObjectsOfType<EnemyHealth1968>();
        Enemy1968[] enemies = FindObjectsOfType<Enemy1968>();
        
        // Fix tag cho EnemyHealth1968
        foreach (var enemy in enemyHealths)
        {
            if (!enemy.CompareTag(requiredTag))
            {
                Debug.Log($"Fixing tag for: {enemy.gameObject.name} (was: {enemy.tag})");
                enemy.gameObject.tag = requiredTag;
                fixedCount++;
            }
        }
        
        // Fix tag cho Enemy1968
        foreach (var enemy in enemies)
        {
            if (!enemy.CompareTag(requiredTag))
            {
                Debug.Log($"Fixing tag for: {enemy.gameObject.name} (was: {enemy.tag})");
                enemy.gameObject.tag = requiredTag;
                fixedCount++;
            }
        }
        
        if (fixedCount > 0)
        {
            Debug.Log($"✅ Fixed tags for {fixedCount} enemies!");
        }
        else
        {
            Debug.Log($"✅ All enemies already have correct tag '{requiredTag}'!");
        }
        
        // Đếm lại số enemy
        GameObject[] taggedEnemies = GameObject.FindGameObjectsWithTag(requiredTag);
        Debug.Log($"📊 Total enemies with tag '{requiredTag}': {taggedEnemies.Length}");
    }
}

