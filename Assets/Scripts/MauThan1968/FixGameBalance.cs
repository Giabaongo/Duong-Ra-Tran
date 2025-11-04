using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Tool để tự động fix game balance values trong Unity Inspector
/// Vì Unity KHÔNG tự động cập nhật Inspector khi sửa code!
/// </summary>
public class FixGameBalance : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/MauThan1968/Fix Game Balance - Update All Values ⚡")]
    public static void FixAllValues()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   FIXING GAME BALANCE VALUES...      ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        int fixedCount = 0;
        
        // ═══════════════════════════════════════════════
        // 1. FIX ALL PLAYER INSTANCES
        // ═══════════════════════════════════════════════
        Debug.Log("\n[1] Fixing Player Health...");
        LinhGiaiPhong1968[] players = FindObjectsOfType<LinhGiaiPhong1968>();
        
        if (players.Length == 0)
        {
            Debug.LogWarning("⚠️ No Player found in scene!");
        }
        
        foreach (var player in players)
        {
            SerializedObject so = new SerializedObject(player);
            SerializedProperty maxHealthProp = so.FindProperty("maxHealth");
            
            if (maxHealthProp != null)
            {
                int oldValue = maxHealthProp.intValue;
                if (oldValue != 20)
                {
                    Debug.Log($"   🔧 Player '{player.name}': MaxHealth {oldValue} → 20");
                    maxHealthProp.intValue = 20;
                    so.ApplyModifiedProperties();
                    EditorUtility.SetDirty(player);
                    fixedCount++;
                }
                else
                {
                    Debug.Log($"   ✅ Player '{player.name}': MaxHealth already 20");
                }
            }
        }
        
        // ═══════════════════════════════════════════════
        // 2. FIX ALL ENEMY INSTANCES
        // ═══════════════════════════════════════════════
        Debug.Log("\n[2] Fixing Enemy Values...");
        Enermy1968Controller[] enemies = FindObjectsOfType<Enermy1968Controller>(true); // Include inactive
        
        if (enemies.Length == 0)
        {
            Debug.LogWarning("⚠️ No Enemy found in scene!");
        }
        
        foreach (var enemy in enemies)
        {
            SerializedObject so = new SerializedObject(enemy);
            
            // Fix Bullet Speed
            SerializedProperty bulletSpeedProp = so.FindProperty("bulletSpeed");
            if (bulletSpeedProp != null)
            {
                float oldSpeed = bulletSpeedProp.floatValue;
                if (oldSpeed != 6f)
                {
                    Debug.Log($"   🔧 Enemy '{enemy.name}': BulletSpeed {oldSpeed} → 6");
                    bulletSpeedProp.floatValue = 6f;
                    fixedCount++;
                }
            }
            
            // Fix Initial Attack Delay
            SerializedProperty initialDelayProp = so.FindProperty("initialAttackDelay");
            if (initialDelayProp != null)
            {
                float oldDelay = initialDelayProp.floatValue;
                if (oldDelay != 1f)
                {
                    Debug.Log($"   🔧 Enemy '{enemy.name}': InitialAttackDelay {oldDelay} → 1");
                    initialDelayProp.floatValue = 1f;
                    fixedCount++;
                }
            }
            
            // Fix Max Blocked Time
            SerializedProperty maxBlockedProp = so.FindProperty("maxBlockedTime");
            if (maxBlockedProp != null && maxBlockedProp.floatValue != 2f)
            {
                Debug.Log($"   🔧 Enemy '{enemy.name}': MaxBlockedTime {maxBlockedProp.floatValue} → 2");
                maxBlockedProp.floatValue = 2f;
                fixedCount++;
            }
            
            // Fix Give Up Cooldown
            SerializedProperty giveUpProp = so.FindProperty("giveUpCooldown");
            if (giveUpProp != null && giveUpProp.floatValue != 5f)
            {
                Debug.Log($"   🔧 Enemy '{enemy.name}': GiveUpCooldown {giveUpProp.floatValue} → 5");
                giveUpProp.floatValue = 5f;
                fixedCount++;
            }
            
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(enemy);
        }
        
        // ═══════════════════════════════════════════════
        // 3. FIX ENEMY PREFABS
        // ═══════════════════════════════════════════════
        Debug.Log("\n[3] Fixing Enemy Prefabs...");
        
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab Enemy");
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            if (prefab != null)
            {
                Enermy1968Controller enemyController = prefab.GetComponent<Enermy1968Controller>();
                if (enemyController != null)
                {
                    SerializedObject so = new SerializedObject(enemyController);
                    
                    bool changed = false;
                    
                    SerializedProperty bulletSpeedProp = so.FindProperty("bulletSpeed");
                    if (bulletSpeedProp != null && bulletSpeedProp.floatValue != 6f)
                    {
                        Debug.Log($"   🔧 Prefab '{prefab.name}': BulletSpeed {bulletSpeedProp.floatValue} → 6");
                        bulletSpeedProp.floatValue = 6f;
                        changed = true;
                    }
                    
                    SerializedProperty initialDelayProp = so.FindProperty("initialAttackDelay");
                    if (initialDelayProp != null && initialDelayProp.floatValue != 1f)
                    {
                        Debug.Log($"   🔧 Prefab '{prefab.name}': InitialAttackDelay {initialDelayProp.floatValue} → 1");
                        initialDelayProp.floatValue = 1f;
                        changed = true;
                    }
                    
                    if (changed)
                    {
                        so.ApplyModifiedProperties();
                        EditorUtility.SetDirty(prefab);
                        fixedCount++;
                    }
                }
            }
        }
        
        // ═══════════════════════════════════════════════
        // SAVE EVERYTHING
        // ═══════════════════════════════════════════════
        if (fixedCount > 0)
        {
            AssetDatabase.SaveAssets();
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
            );
            Debug.Log("\n💾 Saving assets and marking scene dirty...");
        }
        
        // ═══════════════════════════════════════════════
        // SUMMARY
        // ═══════════════════════════════════════════════
        Debug.Log("\n╔══════════════════════════════════════╗");
        Debug.Log($"║   ✅ FIXED {fixedCount} VALUES!             ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        if (fixedCount > 0)
        {
            Debug.Log("⚠️ REMEMBER: Save scene (Ctrl+S) to apply changes!");
        }
        else
        {
            Debug.Log("✅ All values are already correct!");
        }
    }
    
    [MenuItem("Tools/MauThan1968/Disable Enemy Respawn 🚫")]
    public static void DisableEnemyRespawn()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   DISABLING ENEMY RESPAWN...         ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        // Search in current scene (including inactive)
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>(true);
        
        Debug.Log($"[Debug] Found {spawners.Length} EnemySpawner(s) in scene");
        
        // Also search all GameObjects manually
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
        int spawnerCount = 0;
        foreach (var go in allObjects)
        {
            if (go.GetComponent<EnemySpawner>() != null)
            {
                spawnerCount++;
                Debug.Log($"[Debug] Found EnemySpawner on: {go.name} (Active: {go.activeInHierarchy})");
            }
        }
        Debug.Log($"[Debug] Manual search found {spawnerCount} EnemySpawner(s)");
        
        if (spawners.Length == 0)
        {
            Debug.LogWarning("⚠️ No EnemySpawner found in scene!");
            Debug.LogWarning("→ But enemy IS spawning based on logs!");
            Debug.LogWarning("→ CHECK:");
            Debug.LogWarning("   1. Is game RUNNING? Stop it and run tool again");
            Debug.LogWarning("   2. Search 'Spawner' in Hierarchy manually");
            Debug.LogWarning("   3. Check GameManager for spawn logic");
            return;
        }
        
        int disabledCount = 0;
        
        foreach (var spawner in spawners)
        {
            if (spawner.enabled)
            {
                SerializedObject so = new SerializedObject(spawner);
                
                // Disable the component
                spawner.enabled = false;
                
                // Set maxEnemies to 0 as backup
                SerializedProperty maxEnemiesProp = so.FindProperty("maxEnemies");
                if (maxEnemiesProp != null)
                {
                    maxEnemiesProp.intValue = 0;
                    so.ApplyModifiedProperties();
                }
                
                EditorUtility.SetDirty(spawner);
                
                Debug.Log($"✅ Disabled EnemySpawner on: {spawner.gameObject.name}");
                disabledCount++;
            }
            else
            {
                Debug.Log($"ℹ️ EnemySpawner on {spawner.gameObject.name} already disabled");
            }
        }
        
        if (disabledCount > 0)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
            );
            Debug.Log("\n💾 Marking scene dirty...");
        }
        
        Debug.Log("\n╔══════════════════════════════════════╗");
        Debug.Log($"║   ✅ DISABLED {disabledCount} SPAWNER(S)!           ║");
        Debug.Log("╚══════════════════════════════════════╝");
        Debug.Log("⚠️ REMEMBER: Save scene (Ctrl+S) to apply changes!");
        Debug.Log("→ Now enemies won't respawn after being killed!");
    }
    
    [MenuItem("Tools/MauThan1968/Convert Enemies to Regular GameObjects 🔄")]
    public static void ConvertEnemiesToRegular()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   CONVERTING ENEMIES TO REGULAR...   ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        // Find ALL enemies by component (more reliable than name)
        Enermy1968Controller[] enemies = FindObjectsOfType<Enermy1968Controller>(true);
        
        Debug.Log($"[Debug] Found {enemies.Length} enemy controller(s) in scene");
        
        if (enemies.Length == 0)
        {
            Debug.LogError("❌ NO ENEMIES FOUND!");
            Debug.LogError("   → Check if enemies have Enermy1968Controller component");
            Debug.LogError("   → Check if scene is correct");
            return;
        }
        
        int convertedCount = 0;
        int alreadyRegular = 0;
        
        foreach (Enermy1968Controller controller in enemies)
        {
            GameObject obj = controller.gameObject;
            
            Debug.Log($"[Debug] Checking: {obj.name}");
            
            #if UNITY_2018_3_OR_NEWER
            // Check if it's a prefab instance
            if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(obj))
            {
                Debug.LogWarning($"🔄 Found prefab instance: {obj.name}");
                
                // Unpack prefab completely
                UnityEditor.PrefabUtility.UnpackPrefabInstance(obj, UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.AutomatedAction);
                
                Debug.Log($"✅ Converted to regular GameObject: {obj.name}");
                convertedCount++;
            }
            else
            {
                Debug.Log($"ℹ️ Already regular: {obj.name}");
                alreadyRegular++;
            }
            #endif
        }
        
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log($"║   ✅ RESULTS:                        ║");
        Debug.Log($"║   - Total enemies: {enemies.Length}");
        Debug.Log($"║   - Converted: {convertedCount}");
        Debug.Log($"║   - Already regular: {alreadyRegular}");
        Debug.Log("╚══════════════════════════════════════╝");
        
        if (convertedCount > 0)
        {
            Debug.Log("💾 Saving scene...");
            
            // Save scene
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
            );
            UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
            
            Debug.Log("✅ Scene saved!");
        }
        
        Debug.Log("→ Enemies are now regular GameObjects!");
        Debug.Log("→ They will NOT auto-restore when killed!");
        Debug.Log("");
        Debug.Log("🎯 NEXT STEP:");
        Debug.Log("   1. STOP Play Mode");
        Debug.Log("   2. START Play Mode");
        Debug.Log("   3. Kill enemy → Check console");
    }
    
    [MenuItem("Tools/MauThan1968/DEBUG - Check Enemy Hierarchy 🔍")]
    public static void CheckEnemyHierarchy()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   CHECKING ENEMY HIERARCHY...        ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        Enermy1968Controller[] enemies = FindObjectsOfType<Enermy1968Controller>(true);
        
        foreach (Enermy1968Controller controller in enemies)
        {
            GameObject obj = controller.gameObject;
            
            Debug.Log($"\n[Enemy] {obj.name}");
            Debug.Log($"   → Position: {obj.transform.position}");
            Debug.Log($"   → Parent: {(obj.transform.parent != null ? obj.transform.parent.name : "NULL")}");
            Debug.Log($"   → Children: {obj.transform.childCount}");
            
            // List all children
            if (obj.transform.childCount > 0)
            {
                Debug.LogWarning($"   🚨 HAS {obj.transform.childCount} CHILD(REN):");
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    Transform child = obj.transform.GetChild(i);
                    Debug.LogWarning($"      [{i}] {child.name}");
                    
                    // Check if child also has enemy controller
                    if (child.GetComponent<Enermy1968Controller>() != null)
                    {
                        Debug.LogError($"      ⚠️⚠️⚠️ CHILD HAS ENEMY CONTROLLER! THIS IS THE BUG!");
                    }
                }
            }
            
            // List all components
            Debug.Log($"   → Components:");
            Component[] components = obj.GetComponents<Component>();
            foreach (Component comp in components)
            {
                if (comp != null)
                {
                    Debug.Log($"      - {comp.GetType().Name}");
                }
            }
        }
        
        Debug.Log("\n╔══════════════════════════════════════╗");
        Debug.Log("║   ✅ HIERARCHY CHECK COMPLETE!       ║");
        Debug.Log("╚══════════════════════════════════════╝");
    }
    
    [MenuItem("Tools/MauThan1968/Clear Enemy Tracking (RESET) 🔄")]
    public static void ClearEnemyTracking()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   CLEARING ENEMY TRACKING...         ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        // Find any enemy and call static method to clear
        Enermy1968Controller enemy = FindObjectOfType<Enermy1968Controller>(true);
        
        if (enemy != null)
        {
            // Use reflection to clear the static HashSet
            var field = typeof(Enermy1968Controller).GetField("initializedEnemies", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            
            if (field != null)
            {
                var hashSet = field.GetValue(null) as System.Collections.Generic.HashSet<int>;
                if (hashSet != null)
                {
                    int count = hashSet.Count;
                    hashSet.Clear();
                    Debug.Log($"✅ Cleared {count} tracked enemy instance(s)");
                }
            }
        }
        
        Debug.Log("✅ Enemy tracking reset!");
        Debug.Log("→ All enemies can now initialize again");
    }
    
    [MenuItem("Tools/MauThan1968/DESTROY All EnemySpawners 💣")]
    public static void DestroyAllEnemySpawners()
    {
        if (!EditorUtility.DisplayDialog(
            "⚠️ DESTROY EnemySpawners?",
            "This will PERMANENTLY DELETE all EnemySpawner components!\n\n" +
            "Only do this if you want to COMPLETELY REMOVE respawn.\n\n" +
            "Scene has 6 enemies placed manually - that's all you need!\n\n" +
            "Are you sure?",
            "Yes, DESTROY!",
            "No, cancel"))
        {
            Debug.Log("❌ Cancelled");
            return;
        }
        
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>(true);
        
        if (spawners.Length == 0)
        {
            Debug.Log("╔══════════════════════════════════════╗");
            Debug.Log("║   ✅ NO SPAWNER FOUND!               ║");
            Debug.Log("╚══════════════════════════════════════╝");
            Debug.Log("→ Already clean! No respawn possible!");
            return;
        }
        
        int destroyedCount = 0;
        foreach (EnemySpawner spawner in spawners)
        {
            string goName = spawner.gameObject.name;
            DestroyImmediate(spawner);
            destroyedCount++;
            Debug.Log($"💣 Destroyed EnemySpawner on: {goName}");
        }
        
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log($"║   ✅ DESTROYED {destroyedCount} SPAWNER(S)!          ║");
        Debug.Log("╚══════════════════════════════════════╝");
        Debug.Log("→ No more respawn possible!");
        
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );
        AssetDatabase.SaveAssets();
        Debug.Log("💾 Scene saved!");
    }
    
    [MenuItem("Tools/MauThan1968/Show Current Values 📊")]
    public static void ShowCurrentValues()
    {
        Debug.Log("╔══════════════════════════════════════╗");
        Debug.Log("║   CURRENT GAME BALANCE VALUES        ║");
        Debug.Log("╚══════════════════════════════════════╝");
        
        // Show Player Values
        LinhGiaiPhong1968[] players = FindObjectsOfType<LinhGiaiPhong1968>();
        Debug.Log($"\n[PLAYER] Found {players.Length} player(s):");
        foreach (var player in players)
        {
            Debug.Log($"   • {player.name}: MaxHealth = {player.GetMaxHealth()}");
        }
        
        // Show Enemy Values
        Enermy1968Controller[] enemies = FindObjectsOfType<Enermy1968Controller>(true);
        Debug.Log($"\n[ENEMY] Found {enemies.Length} enemy(ies):");
        
        int wrongSpeedCount = 0;
        int wrongDelayCount = 0;
        
        foreach (var enemy in enemies)
        {
            SerializedObject so = new SerializedObject(enemy);
            float bulletSpeed = so.FindProperty("bulletSpeed")?.floatValue ?? -1;
            float initialDelay = so.FindProperty("initialAttackDelay")?.floatValue ?? -1;
            
            string status = "";
            if (bulletSpeed != 6f)
            {
                status += " ⚠️ WRONG SPEED!";
                wrongSpeedCount++;
            }
            if (initialDelay != 1f)
            {
                status += " ⚠️ WRONG DELAY!";
                wrongDelayCount++;
            }
            if (status == "")
            {
                status = " ✅ OK";
            }
            
            Debug.Log($"   • {enemy.name}: Speed={bulletSpeed}, Delay={initialDelay}{status}");
        }
        
        Debug.Log("\n╔══════════════════════════════════════╗");
        if (wrongSpeedCount > 0 || wrongDelayCount > 0)
        {
            Debug.LogWarning($"║ ⚠️  {wrongSpeedCount} enemies with WRONG SPEED");
            Debug.LogWarning($"║ ⚠️  {wrongDelayCount} enemies with WRONG DELAY");
            Debug.LogWarning("║ → RUN FIX SCRIPT NOW!");
        }
        else
        {
            Debug.Log("║ ✅ ALL VALUES CORRECT!");
        }
        Debug.Log("╚══════════════════════════════════════╝");
    }
#endif
}

