# 🎮 VÍ DỤ TÍCH HỢP B52 VỚI GAME

## 📋 CÁC TÌNH HUỐNG SỬ DỤNG

### 1. Bay qua khi THẮNG trận (Victory)

```csharp
// Trong GameManager1968.cs
public void Victory()
{
    if (hasWon || isGameOver) return;
    
    hasWon = true;
    Debug.Log("🎉🎉🎉 === VICTORY! === 🎉🎉🎉");
    
    // ✈️ THÊM: Trigger B52 flyover khi thắng!
    B52FlyoverTrigger b52Trigger = FindObjectOfType<B52FlyoverTrigger>();
    if (b52Trigger != null)
    {
        // Random path để đa dạng
        b52Trigger.TriggerRandomFlyover();
        Debug.Log("[GameManager] ✈️ Victory B52 flyover triggered!");
    }
    
    // Show victory UI
    if (victoryUI != null)
    {
        victoryUI.SetActive(true);
        // ...
    }
}
```

---

### 2. Bay qua khi tiêu diệt số lượng enemy nhất định

```csharp
// Trong GameManager1968.cs
public void EnemyKilled()
{
    if (hasWon || isGameOver) return;
    
    enemiesKilled++;
    Debug.Log($"[GameManager1968] ⚔️ Enemy killed! Progress: {enemiesKilled}/{totalEnemies}");
    
    // ✈️ THÊM: Bay qua khi giết được 50% enemies
    if (enemiesKilled == Mathf.FloorToInt(totalEnemies * 0.5f))
    {
        B52FlyoverTrigger b52Trigger = FindObjectOfType<B52FlyoverTrigger>();
        if (b52Trigger != null)
        {
            b52Trigger.TriggerFlyover(0); // Path cố định từ trái sang phải
            Debug.Log("[GameManager] ✈️ Halfway milestone - B52 flyover!");
        }
    }
    
    // Kiểm tra điều kiện thắng
    if (enemiesKilled >= totalEnemies)
    {
        Victory();
    }
}
```

---

### 3. Bay qua định kỳ sau mỗi X phút

```csharp
// Tạo script mới: B52TimedTrigger.cs
using UnityEngine;

public class B52TimedTrigger : MonoBehaviour
{
    [SerializeField] private B52FlyoverTrigger b52Trigger;
    [SerializeField] private float intervalSeconds = 120f; // 2 phút
    
    private float timer = 0f;
    
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= intervalSeconds)
        {
            if (b52Trigger != null)
            {
                b52Trigger.TriggerRandomFlyover();
                Debug.Log($"[B52Timed] ✈️ Timed flyover after {intervalSeconds}s");
            }
            
            timer = 0f; // Reset timer
        }
    }
}
```

---

### 4. Bay qua khi Player vào khu vực nhất định

```csharp
// Tạo script mới: B52ZoneTrigger.cs
using UnityEngine;

public class B52ZoneTrigger : MonoBehaviour
{
    [SerializeField] private B52FlyoverTrigger b52Trigger;
    [SerializeField] private int pathIndex = 0; // Path nào sẽ bay
    
    private bool hasTriggered = false;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;
        
        if (collision.CompareTag("Player"))
        {
            if (b52Trigger != null)
            {
                b52Trigger.TriggerFlyover(pathIndex);
                Debug.Log($"[B52Zone] ✈️ Player entered zone - B52 flyover!");
                
                hasTriggered = true; // Chỉ trigger 1 lần
            }
        }
    }
}

// SETUP:
// 1. Tạo GameObject với BoxCollider2D (Is Trigger = true)
// 2. Attach script này
// 3. Assign B52FlyoverTrigger reference
// 4. Đặt ở vị trí muốn trigger
```

---

### 5. Bay qua random trong gameplay (background ambience)

```csharp
// CÁCH 1: Dùng B52FlyoverTrigger với Auto Trigger
// Đã có sẵn! Chỉ cần:
// - Auto Trigger = true
// - Interval Min/Max = 60-180 (1-3 phút)
// - Random Path = true

// CÁCH 2: Custom script với nhiều điều kiện hơn
using UnityEngine;

public class B52AmbientController : MonoBehaviour
{
    [SerializeField] private B52FlyoverTrigger b52Trigger;
    [SerializeField] private float minInterval = 60f;
    [SerializeField] private float maxInterval = 180f;
    [SerializeField] private float chanceToTrigger = 0.7f; // 70% chance
    
    private float nextCheckTime = 0f;
    
    void Start()
    {
        ScheduleNextCheck();
    }
    
    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            // Random check có bay không
            if (Random.value <= chanceToTrigger)
            {
                if (b52Trigger != null)
                {
                    b52Trigger.TriggerRandomFlyover();
                    Debug.Log("[B52Ambient] ✈️ Random ambient flyover");
                }
            }
            
            ScheduleNextCheck();
        }
    }
    
    void ScheduleNextCheck()
    {
        float interval = Random.Range(minInterval, maxInterval);
        nextCheckTime = Time.time + interval;
        Debug.Log($"[B52Ambient] Next check in {interval:F0}s");
    }
}
```

---

### 6. Bay qua khi Boss xuất hiện

```csharp
// Trong BossController hoặc BossSpawner script
void SpawnBoss()
{
    // Spawn boss logic
    GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    
    // ✈️ Trigger B52 flyover
    B52FlyoverTrigger b52Trigger = FindObjectOfType<B52FlyoverTrigger>();
    if (b52Trigger != null)
    {
        // Bay chậm, uy nghiêm
        Vector2 start = new Vector2(-25f, 20f);
        Vector2 end = new Vector2(25f, -20f);
        float speed = 3f; // Chậm
        
        b52Trigger.TriggerFlyover(start, end, speed);
        Debug.Log("[Boss] ✈️ Boss arrived - B52 flyover!");
    }
}
```

---

### 7. Kết hợp với Camera Shake

```csharp
// Trong B52Flyover.cs, thêm vào Update():
void Update()
{
    if (!isFlying) return;
    
    // ... existing movement code ...
    
    // ✈️ THÊM: Camera shake khi máy bay gần camera
    if (mainCamera != null)
    {
        float distanceToCamera = Vector2.Distance(transform.position, mainCamera.transform.position);
        
        if (distanceToCamera < 5f) // Trong bán kính 5 units
        {
            // Trigger camera shake (nếu có CameraShake script)
            CameraShake shake = mainCamera.GetComponent<CameraShake>();
            if (shake != null)
            {
                float intensity = Mathf.Lerp(0.2f, 0f, distanceToCamera / 5f);
                shake.Shake(0.1f, intensity); // Duration 0.1s, intensity dựa vào khoảng cách
            }
        }
    }
}
```

---

### 8. Bombing Run (thả bom dọc đường bay)

```csharp
// Thêm vào B52Flyover.cs:
[Header("Bombing Settings")]
[SerializeField] private bool enableBombing = false;
[SerializeField] private GameObject bombPrefab;
[SerializeField] private float bombInterval = 0.5f; // Thả bom mỗi 0.5s
[SerializeField] private int maxBombs = 10;

private float lastBombTime = 0f;
private int bombsDropped = 0;

void Update()
{
    if (!isFlying) return;
    
    // ... existing code ...
    
    // ✈️ THÊM: Drop bombs
    if (enableBombing && bombPrefab != null)
    {
        if (Time.time - lastBombTime >= bombInterval && bombsDropped < maxBombs)
        {
            DropBomb();
            lastBombTime = Time.time;
            bombsDropped++;
        }
    }
}

void DropBomb()
{
    // Spawn bomb ở vị trí hiện tại
    GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
    
    // Bomb sẽ có script riêng để rơi xuống và nổ
    Debug.Log($"[B52Flyover] 💣 Bomb dropped! ({bombsDropped}/{maxBombs})");
}
```

---

### 9. UI Warning trước khi máy bay bay qua

```csharp
// Tạo script: B52WarningUI.cs
using UnityEngine;
using UnityEngine.UI;

public class B52WarningUI : MonoBehaviour
{
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private Text warningText;
    [SerializeField] private float warningDuration = 3f;
    
    private B52FlyoverTrigger b52Trigger;
    
    void Start()
    {
        b52Trigger = FindObjectOfType<B52FlyoverTrigger>();
        
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
    }
    
    // Gọi method này TRƯỚC KHI trigger B52
    public void ShowWarningAndTrigger()
    {
        StartCoroutine(WarningSequence());
    }
    
    System.Collections.IEnumerator WarningSequence()
    {
        // Hiện warning
        if (warningPanel != null)
        {
            warningPanel.SetActive(true);
        }
        
        if (warningText != null)
        {
            warningText.text = "⚠️ AIR STRIKE INCOMING! ⚠️";
        }
        
        Debug.Log("[Warning] ⚠️ Air strike warning shown!");
        
        // Đợi 3 giây
        yield return new WaitForSeconds(warningDuration);
        
        // Ẩn warning
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
        
        // Trigger B52
        if (b52Trigger != null)
        {
            b52Trigger.TriggerRandomFlyover();
            Debug.Log("[Warning] ✈️ B52 triggered after warning!");
        }
    }
}

// SỬ DỤNG:
// B52WarningUI warningUI = FindObjectOfType<B52WarningUI>();
// warningUI.ShowWarningAndTrigger();
```

---

### 10. Fleet of Planes (Đội hình máy bay)

```csharp
// Tạo script: B52FleetController.cs
using UnityEngine;

public class B52FleetController : MonoBehaviour
{
    [SerializeField] private GameObject b52Prefab;
    [SerializeField] private int planeCount = 3;
    [SerializeField] private float spacing = 3f; // Khoảng cách giữa các máy bay
    [SerializeField] private Vector2 baseStart = new Vector2(-20f, 10f);
    [SerializeField] private Vector2 baseEnd = new Vector2(20f, -10f);
    [SerializeField] private float speed = 5f;
    [SerializeField] private float delayBetweenPlanes = 0.5f; // Delay giữa các máy bay
    
    public void TriggerFleet()
    {
        StartCoroutine(SpawnFleet());
    }
    
    System.Collections.IEnumerator SpawnFleet()
    {
        for (int i = 0; i < planeCount; i++)
        {
            // Offset position cho mỗi máy bay
            Vector2 offset = new Vector2(-i * spacing, -i * spacing * 0.5f);
            Vector2 start = baseStart + offset;
            Vector2 end = baseEnd + offset;
            
            // Spawn plane
            GameObject plane = Instantiate(b52Prefab, start, Quaternion.identity);
            plane.name = $"B52_Fleet_{i}";
            
            // Setup
            B52Flyover flyover = plane.GetComponent<B52Flyover>();
            if (flyover != null)
            {
                flyover.SetFlightPath(start, end);
                flyover.SetSpeed(speed);
                flyover.StartFlyover();
            }
            
            Debug.Log($"[Fleet] ✈️ Plane {i+1}/{planeCount} launched");
            
            // Delay trước khi spawn máy bay tiếp theo
            if (i < planeCount - 1)
            {
                yield return new WaitForSeconds(delayBetweenPlanes);
            }
        }
        
        Debug.Log("[Fleet] ✈️✈️✈️ Full fleet deployed!");
    }
}
```

---

## 🎯 CHECKLIST TRIỂN KHAI

- [ ] Đã tạo sprite bóng máy bay B52
- [ ] Đã import vào Unity với settings đúng
- [ ] Đã tạo B52_Shadow prefab với B52Flyover script
- [ ] Đã setup B52_FlyoverManager với B52FlyoverTrigger
- [ ] Đã test trigger thủ công trong Inspector
- [ ] Đã tích hợp vào GameManager hoặc event system
- [ ] Đã test trong gameplay

---

**Happy coding! ✈️**


