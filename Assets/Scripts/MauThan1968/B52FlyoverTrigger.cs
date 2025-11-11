using UnityEngine;

/// <summary>
/// Script để spawn và trigger B52 flyover effect
/// Có thể gọi từ script khác hoặc trigger theo thời gian/event
/// </summary>
public class B52FlyoverTrigger : MonoBehaviour
{
    [Header("B52 Prefab")]
    [SerializeField] private GameObject b52ShadowPrefab; // Prefab có B52Flyover script
    
    [Header("Flight Path Presets")]
    [SerializeField] private Vector2[] startPositions = new Vector2[]
    {
        new Vector2(-20f, 15f),   // Trái sang phải, trên
        new Vector2(20f, 15f),    // Phải sang trái, trên
        new Vector2(-20f, 0f),    // Trái sang phải, giữa
        new Vector2(20f, 0f),     // Phải sang trái, giữa
    };
    
    [SerializeField] private Vector2[] endPositions = new Vector2[]
    {
        new Vector2(20f, -15f),   // Trái sang phải, xuống
        new Vector2(-20f, -15f),  // Phải sang trái, xuống
        new Vector2(20f, 0f),     // Trái sang phải, giữa
        new Vector2(-20f, 0f),    // Phải sang trái, giữa
    };
    
    [Header("Auto Trigger Settings")]
    [SerializeField] private bool autoTrigger = false; // Tự động trigger
    [SerializeField] private float triggerDelay = 5f; // Delay trước lần đầu
    [SerializeField] private float intervalMin = 30f; // Khoảng thời gian tối thiểu giữa các lần
    [SerializeField] private float intervalMax = 60f; // Khoảng thời gian tối đa giữa các lần
    [SerializeField] private bool randomPath = true; // Random đường bay
    
    [Header("Manual Trigger")]
    [SerializeField] private int pathIndex = 0; // Index của path để dùng khi trigger thủ công
    [SerializeField] private float customSpeed = 5f; // Tốc độ bay custom
    
    private float nextTriggerTime = 0f;
    
    void Start()
    {
        if (b52ShadowPrefab == null)
        {
            Debug.LogError("[B52FlyoverTrigger] ❌ B52 Shadow Prefab not assigned!");
            return;
        }
        
        if (autoTrigger)
        {
            nextTriggerTime = Time.time + triggerDelay;
            Debug.Log($"[B52FlyoverTrigger] ✈️ Auto trigger enabled! First flyover in {triggerDelay}s");
        }
    }
    
    void Update()
    {
        if (autoTrigger && Time.time >= nextTriggerTime)
        {
            TriggerRandomFlyover();
            
            // Schedule next trigger
            float nextInterval = Random.Range(intervalMin, intervalMax);
            nextTriggerTime = Time.time + nextInterval;
            Debug.Log($"[B52FlyoverTrigger] Next flyover in {nextInterval:F1}s");
        }
    }
    
    /// <summary>
    /// Trigger flyover với đường bay ngẫu nhiên
    /// </summary>
    public void TriggerRandomFlyover()
    {
        if (startPositions.Length == 0 || endPositions.Length == 0)
        {
            Debug.LogError("[B52FlyoverTrigger] No flight paths defined!");
            return;
        }
        
        int randomIndex = Random.Range(0, Mathf.Min(startPositions.Length, endPositions.Length));
        TriggerFlyover(randomIndex);
    }
    
    /// <summary>
    /// Trigger flyover với path index cụ thể
    /// </summary>
    public void TriggerFlyover(int index)
    {
        if (b52ShadowPrefab == null)
        {
            Debug.LogError("[B52FlyoverTrigger] B52 prefab not assigned!");
            return;
        }
        
        if (index < 0 || index >= startPositions.Length || index >= endPositions.Length)
        {
            Debug.LogError($"[B52FlyoverTrigger] Invalid path index: {index}");
            return;
        }
        
        Vector2 start = startPositions[index];
        Vector2 end = endPositions[index];
        
        // Spawn B52 shadow
        GameObject b52 = Instantiate(b52ShadowPrefab, start, Quaternion.identity);
        b52.name = $"B52_Flyover_{Time.time:F0}";
        
        // Setup flight path
        B52Flyover flyoverScript = b52.GetComponent<B52Flyover>();
        if (flyoverScript != null)
        {
            flyoverScript.SetFlightPath(start, end);
            flyoverScript.SetSpeed(customSpeed);
            flyoverScript.StartFlyover();
            
            Debug.Log($"[B52FlyoverTrigger] ✈️ B52 spawned! Path {index}: {start} → {end}");
        }
        else
        {
            Debug.LogError("[B52FlyoverTrigger] B52 prefab missing B52Flyover script!");
            Destroy(b52);
        }
    }
    
    /// <summary>
    /// Trigger flyover với path custom
    /// </summary>
    public void TriggerFlyover(Vector2 start, Vector2 end, float speed = 5f)
    {
        if (b52ShadowPrefab == null)
        {
            Debug.LogError("[B52FlyoverTrigger] B52 prefab not assigned!");
            return;
        }
        
        // Spawn B52 shadow
        GameObject b52 = Instantiate(b52ShadowPrefab, start, Quaternion.identity);
        b52.name = $"B52_Custom_{Time.time:F0}";
        
        // Setup flight path
        B52Flyover flyoverScript = b52.GetComponent<B52Flyover>();
        if (flyoverScript != null)
        {
            flyoverScript.SetFlightPath(start, end);
            flyoverScript.SetSpeed(speed);
            flyoverScript.StartFlyover();
            
            Debug.Log($"[B52FlyoverTrigger] ✈️ B52 spawned! Custom path: {start} → {end}");
        }
        else
        {
            Debug.LogError("[B52FlyoverTrigger] B52 prefab missing B52Flyover script!");
            Destroy(b52);
        }
    }
    
    // Context menu để test trong Editor
    [ContextMenu("Test Flyover - Path 0")]
    void TestFlyover0() => TriggerFlyover(0);
    
    [ContextMenu("Test Flyover - Random")]
    void TestFlyoverRandom() => TriggerRandomFlyover();
    
    // Debug visualization
    private void OnDrawGizmos()
    {
        if (startPositions == null || endPositions == null) return;
        
        int pathCount = Mathf.Min(startPositions.Length, endPositions.Length);
        
        for (int i = 0; i < pathCount; i++)
        {
            // Draw path với màu khác nhau
            Color pathColor = Color.Lerp(Color.green, Color.red, (float)i / pathCount);
            Gizmos.color = pathColor;
            Gizmos.DrawLine(startPositions[i], endPositions[i]);
            
            // Draw start point
            Gizmos.DrawWireSphere(startPositions[i], 0.5f);
            
            // Draw end point
            Gizmos.DrawSphere(endPositions[i], 0.3f);
        }
    }
}


