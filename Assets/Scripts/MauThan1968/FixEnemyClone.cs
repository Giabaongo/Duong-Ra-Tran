#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Tool để fix vấn đề enemy spawn clone khi chết
/// </summary>
public class FixEnemyClone : MonoBehaviour
{
    [MenuItem("Tools/MauThan1968/Fix Enemy Clone Issue 🐛")]
    public static void FixCloneIssue()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   FIXING ENEMY CLONE ISSUE...        ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        // 1. Tìm tất cả enemy trong scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int enemyCount = 0;
        int cloneCount = 0;
        int disabledCount = 0;
        
        foreach (GameObject obj in allObjects)
        {
            // Check enemy có component EnemyHealth hoặc Enermy1968Controller
            bool isEnemy = obj.GetComponent<EnemyHealth1968>() != null || 
                          obj.GetComponent<Enermy1968Controller>() != null ||
                          obj.CompareTag("Enemy");
            
            if (isEnemy)
            {
                enemyCount++;
                string objName = obj.name;
                
                // Check nếu là clone
                if (objName.Contains("(Clone)"))
                {
                    cloneCount++;
                    Debug.LogWarning($"⚠️ Found CLONE enemy: {objName}");
                    Debug.Log($"   → Will be DELETED!");
                }
                
                // Check nếu bị disable
                if (!obj.activeInHierarchy)
                {
                    disabledCount++;
                    Debug.Log($"ℹ️ Found disabled enemy: {objName}");
                }
                
                Debug.Log($"Enemy #{enemyCount}: {objName} (Active: {obj.activeInHierarchy})");
            }
        }
        
        Debug.Log($"\n📊 SUMMARY:");
        Debug.Log($"   Total enemies found: {enemyCount}");
        Debug.Log($"   Clone enemies: {cloneCount}");
        Debug.Log($"   Disabled enemies: {disabledCount}");
        
        // 2. Xóa các clone enemy
        if (cloneCount > 0)
        {
            if (EditorUtility.DisplayDialog(
                "Delete Clone Enemies?",
                $"Found {cloneCount} clone enemy(ies).\n\nDelete them?",
                "Yes, DELETE!",
                "No"))
            {
                DeleteCloneEnemies();
            }
        }
        else
        {
            Debug.Log("✅ No clone enemies found!");
        }
        
        // 3. Check EnemySpawner components
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>(true);
        if (spawners.Length > 0)
        {
            Debug.LogWarning($"\n⚠️ WARNING: Found {spawners.Length} EnemySpawner(s) in scene!");
            Debug.LogWarning("   This might cause auto-respawn!");
            
            foreach (var spawner in spawners)
            {
                Debug.LogWarning($"   - {spawner.gameObject.name}");
            }
            
            if (EditorUtility.DisplayDialog(
                "Delete EnemySpawners?",
                $"Found {spawners.Length} EnemySpawner(s).\n\n" +
                "They might be causing auto-respawn!\n\n" +
                "Delete them?",
                "Yes, DELETE!",
                "No"))
            {
                DestroySpawners();
            }
        }
        else
        {
            Debug.Log("✅ No EnemySpawner found in scene!");
        }
        
        Debug.Log("\n✅ Fix complete!");
    }
    
    [MenuItem("Tools/MauThan1968/Delete All Clone Enemies 🗑️")]
    public static void DeleteCloneEnemies()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int deletedCount = 0;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("(Clone)"))
            {
                bool isEnemy = obj.GetComponent<EnemyHealth1968>() != null || 
                              obj.GetComponent<Enermy1968Controller>() != null ||
                              obj.CompareTag("Enemy");
                
                if (isEnemy)
                {
                    Debug.Log($"💣 Deleting clone enemy: {obj.name}");
                    DestroyImmediate(obj);
                    deletedCount++;
                }
            }
        }
        
        if (deletedCount > 0)
        {
            Debug.Log($"✅ Deleted {deletedCount} clone enemy(ies)!");
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
            );
            Debug.Log("💾 Remember to SAVE scene (Ctrl+S)!");
        }
        else
        {
            Debug.Log("✅ No clone enemies found to delete!");
        }
    }
    
    [MenuItem("Tools/MauThan1968/Fix Enemy Destroy Issue 🔧")]
    public static void FixDestroyIssue()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   FIXING DESTROY ISSUE...            ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        // Tìm tất cả EnemyHealth1968 scripts
        EnemyHealth1968[] healthScripts = FindObjectsOfType<EnemyHealth1968>(true);
        
        Debug.Log($"Found {healthScripts.Length} EnemyHealth1968 script(s)");
        
        int fixedCount = 0;
        foreach (var health in healthScripts)
        {
            // Đảm bảo hideImmediately = true để không có fade animation
            // (fade animation có thể gây ra issue với Destroy)
            SerializedObject so = new SerializedObject(health);
            SerializedProperty hideImmediatelyProp = so.FindProperty("hideImmediately");
            
            if (hideImmediatelyProp != null && !hideImmediatelyProp.boolValue)
            {
                hideImmediatelyProp.boolValue = true;
                so.ApplyModifiedProperties();
                fixedCount++;
                Debug.Log($"✅ Fixed {health.gameObject.name}: hideImmediately = true");
            }
        }
        
        if (fixedCount > 0)
        {
            Debug.Log($"✅ Fixed {fixedCount} enemy(ies)!");
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
            );
            Debug.Log("💾 Remember to SAVE scene (Ctrl+S)!");
        }
        else
        {
            Debug.Log("✅ All enemies already have correct settings!");
        }
    }
    
    private static void DestroySpawners()
    {
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>(true);
        int destroyedCount = 0;
        
        foreach (var spawner in spawners)
        {
            string goName = spawner.gameObject.name;
            DestroyImmediate(spawner);
            destroyedCount++;
            Debug.Log($"💣 Destroyed EnemySpawner on: {goName}");
        }
        
        Debug.Log($"✅ Destroyed {destroyedCount} spawner(s)!");
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );
        AssetDatabase.SaveAssets();
        Debug.Log("💾 Scene saved!");
    }
}
#endif

