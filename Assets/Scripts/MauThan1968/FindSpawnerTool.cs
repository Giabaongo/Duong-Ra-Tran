#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;

/// <summary>
/// Tool để TÌM BẤT KỲ SCRIPT NÀO có thể spawn enemy
/// </summary>
public class FindSpawnerTool : Editor
{
    [MenuItem("Tools/MauThan1968/DEBUG - Find All Spawn Logic 🔍")]
    public static void FindAllSpawnLogic()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   SEARCHING FOR SPAWN LOGIC...      ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        int foundCount = 0;
        
        // Tìm TẤT CẢ MonoBehaviour trong scene
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>(true);
        
        Debug.Log($"[Debug] Checking {allScripts.Length} MonoBehaviour(s) in scene...");
        
        foreach (MonoBehaviour script in allScripts)
        {
            if (script == null) continue;
            
            // Lấy type của script
            System.Type type = script.GetType();
            
            // Check nếu có field tên "enemyPrefab"
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            bool hasEnemyPrefab = false;
            foreach (FieldInfo field in fields)
            {
                if (field.Name.ToLower().Contains("enemy") && field.Name.ToLower().Contains("prefab"))
                {
                    hasEnemyPrefab = true;
                    object value = field.GetValue(script);
                    Debug.LogWarning($"🚨 FOUND: {script.gameObject.name} → {type.Name}.{field.Name} = {value}");
                    foundCount++;
                }
            }
            
            // Check nếu có method tên "SpawnEnemy" hoặc "Spawn"
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (MethodInfo method in methods)
            {
                if (method.Name.ToLower().Contains("spawn") && method.Name.ToLower().Contains("enemy"))
                {
                    Debug.LogWarning($"🚨 FOUND METHOD: {script.gameObject.name} → {type.Name}.{method.Name}()");
                    foundCount++;
                }
            }
        }
        
        Debug.Log("\n╔══════════════════════════════════════╗");
        Debug.Log($"║   FOUND {foundCount} SPAWN LOGIC(S)!");
        Debug.Log("╚══════════════════════════════════════╝");
        
        if (foundCount == 0)
        {
            Debug.Log("✅ NO spawn logic found in scene!");
            Debug.Log("⚠️ But enemies are still spawning somehow!");
            Debug.Log("");
            Debug.Log("💡 Possible reasons:");
            Debug.Log("   1. Enemy prefab contains spawn logic");
            Debug.Log("   2. Scene file has corrupted data");
            Debug.Log("   3. Unity is auto-instantiating from prefab");
            Debug.Log("   4. Domain reload is restoring enemies");
        }
    }
    
    [MenuItem("Tools/MauThan1968/DEBUG - List All GameObjects 📋")]
    public static void ListAllGameObjects()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   ALL GAMEOBJECTS IN SCENE:          ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
        
        int enemyCount = 0;
        int spawnerCount = 0;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.ToLower().Contains("enemy"))
            {
                enemyCount++;
                Debug.Log($"[Enemy] {obj.name} (Active: {obj.activeInHierarchy}, Components: {obj.GetComponents<Component>().Length})");
                
                // List all components
                Component[] components = obj.GetComponents<Component>();
                foreach (Component comp in components)
                {
                    if (comp != null)
                    {
                        Debug.Log($"   → {comp.GetType().Name}");
                    }
                }
            }
            
            if (obj.name.ToLower().Contains("spawn"))
            {
                spawnerCount++;
                Debug.LogWarning($"[Spawner?] {obj.name} (Active: {obj.activeInHierarchy})");
                
                Component[] components = obj.GetComponents<Component>();
                foreach (Component comp in components)
                {
                    if (comp != null)
                    {
                        Debug.LogWarning($"   → {comp.GetType().Name}");
                    }
                }
            }
        }
        
        Debug.Log($"\n[Summary] Found {enemyCount} enemy objects, {spawnerCount} spawner objects");
    }
}
#endif

