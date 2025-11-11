using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Script đơn giản cho bóng máy bay B52 bay qua tự động
/// Tự động loop liên tục khi scene play
/// BAY THEO KHUNG HÌNH CAMERA (tracking player)
/// </summary>
public class B52ShadowSimple : MonoBehaviour
{
    [Header("⚙️ CÀI ĐẶT CƠ BẢN")]
    [Tooltip("Tốc độ bay (units/giây)")]
    [SerializeField] private float flySpeed = 5f;
    
    [Tooltip("Thời gian chờ giữa các lần bay (giây)")]
    [SerializeField] private float delayBetweenFlights = 10f;
    
    [Tooltip("Tự động bắt đầu khi scene load")]
    [SerializeField] private bool autoStart = true;
    
    [Tooltip("Delay trước lần bay đầu tiên (giây)")]
    [SerializeField] private float initialDelay = 5f;
    
    [Header("🎨 HIỆU ỨNG HÌNH ẢNH")]
    [Tooltip("Độ mờ của bóng (0-1)")]
    [SerializeField] private float shadowOpacity = 0.6f;
    
    [Tooltip("Fade mượt khi vào/ra")]
    [SerializeField] private bool enableFade = true;
    
    [Tooltip("Khoảng cách fade (units)")]
    [SerializeField] private float fadeDistance = 5f;
    
    [Tooltip("Kích thước sprite (scale)")]
    [SerializeField] private Vector3 spriteScale = new Vector3(0.8f, 0.8f, 1f);
    
    [Tooltip("Điều chỉnh góc xoay (nếu sprite không hướng đúng)")]
    [Range(-180f, 180f)]
    [SerializeField] private float rotationOffset = -90f; // -90° nếu sprite hướng lên trên
    
    [Header("🔊 ÂM THANH")]
    [Tooltip("Kéo file b52-sound.mp3 vào đây")]
    [SerializeField] private AudioClip b52Sound;
    
    [Tooltip("Âm lượng âm thanh (có thể > 1 để to hơn!)")]
    [SerializeField] private float volume = 2.5f; // TĂNG LÊN 2.5x!
    
    [Tooltip("Fade âm thanh theo khoảng cách")]
    [SerializeField] private bool fadeAudio = false; // TẮT FADE để luôn to!
    
    [Tooltip("Khoảng cách tối đa nghe được (units)")]
    [SerializeField] private float audioMaxDistance = 30f; // TĂNG khoảng cách nghe
    
    [Header("🛫 ĐƯỜNG BAY")]
    [Tooltip("Bay theo khung hình camera (tracking player) thay vì random toàn map")]
    [SerializeField] private bool followCamera = true;
    
    [Tooltip("Khoảng cách từ camera để bắt đầu bay (units ngoài màn hình)")]
    [SerializeField] private float cameraOffset = 8f; // Tăng lên để bay cao hơn!
    
    [Tooltip("Random đường bay mỗi lần (nếu không follow camera)")]
    [SerializeField] private bool randomFlightPath = true;
    
    [Header("💣 BOMBING (Thả bom)")]
    [Tooltip("Bật tính năng thả bom khi bay qua")]
    [SerializeField] private bool enableBombing = false;
    
    [Tooltip("Prefab bom (kéo vào đây)")]
    [SerializeField] private GameObject bombPrefab;
    
    [Tooltip("Thả tất cả bom cùng lúc (salvo) thay vì từng quả")]
    [SerializeField] private bool dropAllAtOnce = true;
    
    [Tooltip("Khoảng cách giữa các bom khi thả cùng lúc (units)")]
    [SerializeField] private float bombSpacing = 1f;
    
    [Tooltip("Khoảng thời gian giữa các quả bom (giây) - Nếu không thả cùng lúc")]
    [SerializeField] private float bombInterval = 0.5f;
    
    [Tooltip("Số lượng bom mỗi chuyến bay")]
    [SerializeField] private int bombCount = 4;
    
    [Tooltip("Âm thanh thả bom (whistle - tiếng rít)")]
    [SerializeField] private AudioClip bombDropSound;
    
    private float lastBombTime = -999f;
    private int bombsDropped = 0;
    private bool hasBombed = false; // Đã thả bom chưa (cho salvo mode)
    
    // Private variables
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Camera mainCamera;
    
    private bool isFlying = false;
    private Vector2 currentStart;
    private Vector2 currentEnd;
    private float journeyProgress = 0f;
    private float totalDistance = 0f;
    private int flightCount = 0;
    
    void Start()
    {
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("[B52Shadow] ❌ Cần có SpriteRenderer component!");
            enabled = false;
            return;
        }
        
        // ⚠️ KIỂM TRA SPRITE
        if (spriteRenderer.sprite == null)
        {
            Debug.LogError("[B52Shadow] ❌❌❌ CHƯA CÓ SPRITE! Kéo hình B52_Shadow.png vào field 'Sprite' trong SpriteRenderer!");
            Debug.LogError("[B52Shadow] → Chọn GameObject này → Component SpriteRenderer → Kéo sprite vào");
        }
        else
        {
            Debug.Log($"[B52Shadow] ✅ Sprite loaded: {spriteRenderer.sprite.name}");
        }
        
        // Set scale
        transform.localScale = spriteScale;
        Debug.Log($"[B52Shadow] ✅ Scale set to: {spriteScale}");
        
        // ⚠️ KIỂM TRA SORTING LAYER
        Debug.Log($"[B52Shadow] Sorting Layer: '{spriteRenderer.sortingLayerName}', Order: {spriteRenderer.sortingOrder}");
        if (spriteRenderer.sortingOrder < 50)
        {
            Debug.LogWarning("[B52Shadow] ⚠️ Order in Layer < 50 - Có thể bị che! Đề xuất: 100+");
        }
        
        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.clip = b52Sound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
        audioSource.volume = 0f; // Bắt đầu từ 0, sẽ fade in
        
        if (b52Sound == null)
        {
            Debug.LogWarning("[B52Shadow] ⚠️ Chưa có âm thanh B52! Kéo file b52-sound.mp3 vào field 'B52 Sound'");
        }
        
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("[B52Shadow] ❌ Không tìm thấy Main Camera!");
        }
        else
        {
            Debug.Log($"[B52Shadow] ✅ Camera found: {mainCamera.name}");
        }
        
        // Set opacity ban đầu
        Color c = spriteRenderer.color;
        c.a = 0f; // Trong suốt
        spriteRenderer.color = c;
        
        // Ẩn sprite ban đầu
        spriteRenderer.enabled = false;
        
        // Auto start nếu bật
        if (autoStart)
        {
            Invoke(nameof(StartFlight), initialDelay);
            Debug.Log($"[B52Shadow] ✈️ Sẽ bay lần đầu sau {initialDelay}s, sau đó lặp mỗi {delayBetweenFlights}s");
            Debug.Log($"[B52Shadow] Follow Camera: {followCamera}, Camera Offset: {cameraOffset}");
        }
    }
    
    void Update()
    {
        if (!isFlying) return;
        
        // Di chuyển
        float step = flySpeed * Time.deltaTime / totalDistance;
        journeyProgress += step;
        
        // Cập nhật vị trí
        transform.position = Vector2.Lerp(currentStart, currentEnd, journeyProgress);
        
        // ✈️ Xoay máy bay theo hướng bay
        Vector2 direction = (currentEnd - currentStart).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
        
        // Cập nhật opacity (fade)
        if (enableFade)
        {
            float alpha = CalculateFadeAlpha();
            Color c = spriteRenderer.color;
            c.a = alpha * shadowOpacity;
            spriteRenderer.color = c;
        }
        else
        {
            Color c = spriteRenderer.color;
            c.a = shadowOpacity;
            spriteRenderer.color = c;
        }
        
        // Cập nhật âm thanh
        if (audioSource != null)
        {
            if (fadeAudio && mainCamera != null)
            {
                // Fade theo khoảng cách
                float distanceToCamera = Vector2.Distance(transform.position, mainCamera.transform.position);
                float volumeFactor = 1f - Mathf.Clamp01(distanceToCamera / audioMaxDistance);
                audioSource.volume = volumeFactor * volume;
            }
            else
            {
                // Âm lượng cố định (to nhất!)
                audioSource.volume = volume;
            }
        }
        
        // 💣 Thả bom (nếu bật)
        if (enableBombing && bombPrefab != null)
        {
            if (dropAllAtOnce)
            {
                // Thả tất cả bom cùng lúc khi đến giữa hành trình
                if (!hasBombed && journeyProgress >= 0.4f && journeyProgress <= 0.6f)
                {
                    DropBombSalvo();
                    hasBombed = true;
                }
            }
            else
            {
                // Thả từng quả theo interval
                if (Time.time - lastBombTime >= bombInterval && bombsDropped < bombCount)
                {
                    DropBomb();
                    lastBombTime = Time.time;
                    bombsDropped++;
                }
            }
        }
        
        // Kiểm tra hoàn thành
        if (journeyProgress >= 1f)
        {
            EndFlight();
        }
    }
    
    /// <summary>
    /// Bắt đầu một chuyến bay
    /// </summary>
    void StartFlight()
    {
        if (isFlying) return;
        
        flightCount++;
        
        // Tính toán đường bay
        if (followCamera && mainCamera != null)
        {
            // BAY THEO CAMERA (tracking player)
            CalculateCameraRelativePath();
        }
        else
        {
            // BAY RANDOM TOÀN MAP (legacy mode)
            CalculateFixedPath();
        }
        
        totalDistance = Vector2.Distance(currentStart, currentEnd);
        
        // Reset progress
        journeyProgress = 0f;
        isFlying = true;
        
        // Hiện sprite
        spriteRenderer.enabled = true;
        transform.position = currentStart;
        
        // ✈️ Xoay máy bay về hướng bay ngay từ đầu
        Vector2 direction = (currentEnd - currentStart).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float finalAngle = angle + rotationOffset;
        transform.rotation = Quaternion.Euler(0, 0, finalAngle);
        
        // ⚠️ DEBUG: Kiểm tra sprite có hiển thị không
        Debug.Log($"[B52Shadow] 🎨 Sprite enabled: {spriteRenderer.enabled}, Has sprite: {spriteRenderer.sprite != null}");
        Debug.Log($"[B52Shadow] 📍 Position: {transform.position}, Scale: {transform.localScale}, Rotation: {finalAngle:F1}° (Base: {angle:F1}° + Offset: {rotationOffset:F1}°)");
        Debug.Log($"[B52Shadow] 🎨 Color: {spriteRenderer.color}, Opacity: {spriteRenderer.color.a}");
        
        // Phát âm thanh
        if (audioSource != null && b52Sound != null)
        {
            audioSource.Play();
        }
        
        Debug.Log($"[B52Shadow] ✈️ Chuyến bay #{flightCount} bắt đầu! {currentStart} → {currentEnd} (Distance: {totalDistance:F1})");
    }
    
    /// <summary>
    /// Tính đường bay theo khung hình camera (player)
    /// </summary>
    void CalculateCameraRelativePath()
    {
        // Lấy vị trí camera (player)
        Vector3 cameraPos = mainCamera.transform.position;
        
        // Tính kích thước viewport trong world space
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        
        // Random hướng bay (4 hướng chính)
        int direction = Random.Range(0, 4);
        
        switch (direction)
        {
            case 0: // Trái → Phải
                currentStart = new Vector2(
                    cameraPos.x - cameraWidth/2f - cameraOffset,
                    cameraPos.y + Random.Range(-cameraHeight/3f, cameraHeight/3f)
                );
                currentEnd = new Vector2(
                    cameraPos.x + cameraWidth/2f + cameraOffset,
                    cameraPos.y + Random.Range(-cameraHeight/3f, cameraHeight/3f)
                );
                break;
                
            case 1: // Phải → Trái
                currentStart = new Vector2(
                    cameraPos.x + cameraWidth/2f + cameraOffset,
                    cameraPos.y + Random.Range(-cameraHeight/3f, cameraHeight/3f)
                );
                currentEnd = new Vector2(
                    cameraPos.x - cameraWidth/2f - cameraOffset,
                    cameraPos.y + Random.Range(-cameraHeight/3f, cameraHeight/3f)
                );
                break;
                
            case 2: // Trên → Dưới
                currentStart = new Vector2(
                    cameraPos.x + Random.Range(-cameraWidth/3f, cameraWidth/3f),
                    cameraPos.y + cameraHeight/2f + cameraOffset
                );
                currentEnd = new Vector2(
                    cameraPos.x + Random.Range(-cameraWidth/3f, cameraWidth/3f),
                    cameraPos.y - cameraHeight/2f - cameraOffset
                );
                break;
                
            case 3: // Dưới → Trên
                currentStart = new Vector2(
                    cameraPos.x + Random.Range(-cameraWidth/3f, cameraWidth/3f),
                    cameraPos.y - cameraHeight/2f - cameraOffset
                );
                currentEnd = new Vector2(
                    cameraPos.x + Random.Range(-cameraWidth/3f, cameraWidth/3f),
                    cameraPos.y + cameraHeight/2f + cameraOffset
                );
                break;
        }
        
        Debug.Log($"[B52Shadow] 📷 Camera-relative path: Direction {direction}, Camera at {cameraPos}");
    }
    
    /// <summary>
    /// Tính đường bay cố định (legacy mode - không theo camera)
    /// </summary>
    void CalculateFixedPath()
    {
        // Các đường bay cố định
        Vector2[] startPositions = new Vector2[]
        {
            new Vector2(-25f, 15f),   // 0: Trái → Phải (trên)
            new Vector2(25f, 15f),    // 1: Phải → Trái (trên)
            new Vector2(-25f, 0f),    // 2: Trái → Phải (giữa)
            new Vector2(25f, 0f),     // 3: Phải → Trái (giữa)
            new Vector2(-25f, -15f),  // 4: Trái → Phải (dưới)
            new Vector2(25f, -15f),   // 5: Phải → Trái (dưới)
        };
        
        Vector2[] endPositions = new Vector2[]
        {
            new Vector2(25f, -15f),   // 0
            new Vector2(-25f, -15f),  // 1
            new Vector2(25f, 0f),     // 2
            new Vector2(-25f, 0f),    // 3
            new Vector2(25f, 15f),    // 4
            new Vector2(-25f, 15f),   // 5
        };
        
        int pathIndex;
        if (randomFlightPath)
        {
            pathIndex = Random.Range(0, startPositions.Length);
        }
        else
        {
            pathIndex = (flightCount - 1) % startPositions.Length;
        }
        
        currentStart = startPositions[pathIndex];
        currentEnd = endPositions[pathIndex];
        
        Debug.Log($"[B52Shadow] 🗺️ Fixed path #{pathIndex}");
    }
    
    /// <summary>
    /// Kết thúc chuyến bay
    /// </summary>
    void EndFlight()
    {
        isFlying = false;
        
        // Ẩn sprite
        spriteRenderer.enabled = false;
        
        // Dừng âm thanh
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        
        // Reset bomb counter
        int totalBombs = hasBombed ? bombCount : bombsDropped;
        bombsDropped = 0;
        hasBombed = false;
        
        Debug.Log($"[B52Shadow] ✅ Chuyến bay #{flightCount} hoàn thành! Đã thả {totalBombs} bom.");
        
        // Lên lịch chuyến bay tiếp theo
        Invoke(nameof(StartFlight), delayBetweenFlights);
    }
    
    /// <summary>
    /// 💣 Thả bom
    /// </summary>
    void DropBomb()
    {
        if (bombPrefab == null)
        {
            Debug.LogWarning("[B52Shadow] ⚠️ Bomb prefab not assigned!");
            return;
        }
        
        // ⬆️ Spawn bom CAO HƠN vị trí máy bay
        float dropHeight = 5f; // Spawn cao hơn 5 units để có thời gian rơi!
        Vector3 bombPosition = transform.position + Vector3.up * dropHeight;
        
        GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);
        bomb.name = $"Bomb_{bombsDropped + 1}";
        
        // Phát âm thanh thả bom (whistle)
        if (bombDropSound != null)
        {
            AudioSource.PlayClipAtPoint(bombDropSound, transform.position, 2.5f); // TĂNG MẠNH!
        }
        
        Debug.Log($"[B52Shadow] 💣 Bomb dropped! ({bombsDropped + 1}/{bombCount}) at {bombPosition} (Height offset: {dropHeight})");
    }
    
    /// <summary>
    /// 💣💣💣 Thả nhiều bom cùng lúc (Salvo/Carpet bombing)
    /// </summary>
    void DropBombSalvo()
    {
        if (bombPrefab == null)
        {
            Debug.LogWarning("[B52Shadow] ⚠️ Bomb prefab not assigned!");
            return;
        }
        
        // Tính hướng bay
        Vector2 direction = (currentEnd - currentStart).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x); // Vuông góc với hướng bay
        
        // Tính vị trí trung tâm của salvo
        float totalWidth = (bombCount - 1) * bombSpacing;
        Vector2 startOffset = perpendicular * (-totalWidth / 2f);
        
        // ⬆️ Offset để bom spawn CAO HƠN (không chạm đất ngay)
        float dropHeight = 5f; // Spawn cao hơn 5 units so với B52 để có thời gian rơi!
        
        // Thả tất cả bom
        for (int i = 0; i < bombCount; i++)
        {
            Vector2 offset = startOffset + (perpendicular * (i * bombSpacing));
            Vector2 bombPosition = (Vector2)transform.position + offset + Vector2.up * dropHeight;
            
            GameObject bomb = Instantiate(bombPrefab, bombPosition, Quaternion.identity);
            bomb.name = $"Bomb_{i + 1}";
            
            Debug.Log($"[B52Shadow] 💣 Bomb {i + 1}/{bombCount} dropped at {bombPosition} (Height offset: {dropHeight})");
        }
        
        // Phát âm thanh thả bom (chỉ 1 lần cho cả salvo)
        if (bombDropSound != null)
        {
            AudioSource.PlayClipAtPoint(bombDropSound, transform.position, 2.5f); // TĂNG MẠNH!
        }
        
        Debug.Log($"[B52Shadow] 💥 SALVO! Dropped {bombCount} bombs at once at {transform.position}!");
    }
    
    /// <summary>
    /// Tính alpha cho fade effect
    /// </summary>
    float CalculateFadeAlpha()
    {
        float distanceFromStart = journeyProgress * totalDistance;
        float distanceFromEnd = (1f - journeyProgress) * totalDistance;
        
        float fadeIn = Mathf.Clamp01(distanceFromStart / fadeDistance);
        float fadeOut = Mathf.Clamp01(distanceFromEnd / fadeDistance);
        
        return Mathf.Min(fadeIn, fadeOut);
    }
    
    /// <summary>
    /// Bắt đầu bay ngay (dùng trong code hoặc Inspector)
    /// </summary>
    [ContextMenu("Bay Ngay")]
    public void TriggerImmediateFlight()
    {
        CancelInvoke(); // Hủy các invoke đang chờ
        StartFlight();
    }
    
    /// <summary>
    /// Dừng tất cả các chuyến bay
    /// </summary>
    [ContextMenu("Dừng Bay")]
    public void StopFlying()
    {
        CancelInvoke();
        if (isFlying)
        {
            EndFlight();
        }
        Debug.Log("[B52Shadow] ⏸️ Đã dừng bay!");
    }
    
    void OnDestroy()
    {
        // Cleanup
        CancelInvoke();
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
    
    // Debug visualization
    void OnDrawGizmos()
    {
        // Vẽ đường bay hiện tại
        if (Application.isPlaying && isFlying)
        {
            // Vẽ đường bay
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(currentStart, currentEnd);
            
            // Vẽ vị trí hiện tại
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 1.5f);
            Gizmos.DrawLine(transform.position, currentEnd);
            
            // Vẽ điểm đích
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentEnd, 0.8f);
        }
        
        // Vẽ khung hình camera (nếu follow camera)
        if (followCamera && mainCamera != null)
        {
            Vector3 cameraPos = Application.isPlaying ? mainCamera.transform.position : Camera.main.transform.position;
            Camera cam = Application.isPlaying ? mainCamera : Camera.main;
            
            if (cam != null)
            {
                float height = cam.orthographicSize * 2f;
                float width = height * cam.aspect;
                
                // Vẽ viewport camera
                Gizmos.color = new Color(0f, 1f, 1f, 0.3f); // Cyan trong suốt
                Vector3 topLeft = new Vector3(cameraPos.x - width/2f, cameraPos.y + height/2f, 0);
                Vector3 topRight = new Vector3(cameraPos.x + width/2f, cameraPos.y + height/2f, 0);
                Vector3 bottomLeft = new Vector3(cameraPos.x - width/2f, cameraPos.y - height/2f, 0);
                Vector3 bottomRight = new Vector3(cameraPos.x + width/2f, cameraPos.y - height/2f, 0);
                
                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft);
                Gizmos.DrawLine(bottomLeft, topLeft);
                
                #if UNITY_EDITOR
                // Vẽ text "Camera View"
                Handles.Label(cameraPos + Vector3.up * (height/2f + 1f), "📷 Camera Viewport");
                #endif
            }
        }
    }
}


