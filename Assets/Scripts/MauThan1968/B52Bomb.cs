using UnityEngine;
using Unity.Cinemachine; // Unity 6 / Cinemachine 3.x

/// <summary>
/// Script cho bom rơi từ B52
/// Rơi xuống → Nổ → Tạo hiệu ứng và gây damage
/// </summary>
public class B52Bomb : MonoBehaviour
{
    [Header("💣 BOM SETTINGS")]
    [Tooltip("Tốc độ rơi (gravity)")]
    [SerializeField] private float fallSpeed = 5f;

    [Tooltip("Bán kính nổ (units)")]
    [SerializeField] private float explosionRadius = 3f;
    
    [Tooltip("Damage gây ra (B52 của Mỹ - bombing lính giải phóng!)")]
    [SerializeField] private int damageToPlayer = 5;
    
    [Tooltip("Damage cho cả enemies (friendly fire nếu trong bán kính)")]
    [SerializeField] private int damageToEnemies = 1;
    
    [Tooltip("Có gây damage cho cả hai bên không (realistic)")]
    [SerializeField] private bool damageAll = true;
    
    [Header("🔊 ÂM THANH")]
    [Tooltip("Âm thanh khi bom rơi (whistle/falling)")]
    [SerializeField] private AudioClip fallingSound;
    
    [Tooltip("Volume âm thanh rơi")]
    [SerializeField] private float fallingVolume = 2.0f; // TĂNG thêm!
    
    [Tooltip("Âm thanh khi nổ (explosion)")]
    [SerializeField] private AudioClip explosionSound;
    
    [Tooltip("Volume âm thanh nổ (có thể rất cao!)")]
    [SerializeField] private float explosionVolume = 80f; // CỰC TO! Rung cả tai!
    
    [Header("🎨 HIỆU ỨNG")]
    [Tooltip("Prefab hiệu ứng nổ (particle system)")]
    [SerializeField] private GameObject explosionEffectPrefab;
    
    [Tooltip("Thời gian sống của hiệu ứng nổ (giây)")]
    [SerializeField] private float explosionEffectDuration = 2f;
    
    [Tooltip("Sprite bom (nếu dùng sprite)")]
    [SerializeField] private Sprite bombSprite;
    
    [Header("⚙️ SPAWN SETTINGS")]
    [Tooltip("Delay trước khi bật collision (tránh nổ ngay khi spawn)")]
    [SerializeField] private float collisionDelay = 0.5f;
    
    [Header("📳 CAMERA SHAKE")]
    [Tooltip("Cường độ rung camera khi nổ (0-2)")]
    [SerializeField] private float shakeIntensity = 10.0f;
    
    [Tooltip("Khoảng cách tối đa camera rung (units)")]
    [SerializeField] private float shakeMaxDistance = 25f;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Collider2D bombCollider;
    private bool hasExploded = false;
    private bool collisionEnabled = false;
    
    void Awake()
    {
        // Get/Add components
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // 2D sound
        audioSource.volume = fallingVolume; // Set volume!
        
        // Setup Rigidbody2D
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f; // Tắt gravity, dùng custom fall speed
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        // Setup sprite nếu có
        if (bombSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = bombSprite;
        }
        
        // ⚠️ KHÔNG tự động set scale - dùng scale từ prefab!
        // transform.localScale giữ nguyên từ prefab
        
        // GIỮ NGUYÊN sorting layer từ prefab (không override!)
        // Chỉ tăng Order nếu cần
        if (spriteRenderer != null)
        {
            // KHÔNG set sortingLayerName - để prefab quyết định!
            // spriteRenderer.sortingLayerName = "Default"; // ❌ BỎ DÒNG NÀY!
            
            // Chỉ set Order nếu prefab chưa set
            if (spriteRenderer.sortingOrder < 50)
            {
                spriteRenderer.sortingOrder = 50;
            }
            
            Debug.Log($"[Bomb] 🎨 Sprite settings: Layer={spriteRenderer.sortingLayerName}, Order={spriteRenderer.sortingOrder}, Scale={transform.localScale}");
        }
        
        // Get collider và TẠM THỜI DISABLE để tránh nổ ngay khi spawn
        bombCollider = GetComponent<Collider2D>();
        if (bombCollider != null)
        {
            bombCollider.enabled = false;
            Debug.Log($"[Bomb] ⏳ Collider disabled for {collisionDelay}s to prevent instant explosion");
        }
        
        // 🔊 Phát âm thanh rơi ngay khi spawn
        if (fallingSound != null && audioSource != null)
        {
            audioSource.clip = fallingSound;
            audioSource.Play();
            Debug.Log($"[Bomb] 🔊 Playing falling sound!");
        }
        
        Debug.Log($"[Bomb] 💣 Bomb initialized at {transform.position}");
    }
    
    void Start()
    {
        // Enable collision sau delay
        Invoke(nameof(EnableCollision), collisionDelay);
    }
    
    void EnableCollision()
    {
        if (bombCollider != null)
        {
            bombCollider.enabled = true;
            collisionEnabled = true;
            Debug.Log($"[Bomb] ✅ Collision ENABLED at {transform.position}");
        }
    }
    
    void Update()
    {
        if (hasExploded) return;
        
        // Rơi xuống với tốc độ cố định
        rb.linearVelocity = Vector2.down * fallSpeed;
        
        // Xoay khi rơi (cho đẹp)
        transform.Rotate(0, 0, 360f * Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;
        
        // ⏳ Chờ collision được enable
        if (!collisionEnabled) return;
        
        // Ignore bom khác
        if (collision.CompareTag("Bomb")) return;
        
        // Nổ khi chạm bất kỳ collider nào (trừ trigger)
        if (!collision.isTrigger)
        {
            Debug.Log($"[Bomb] 💥 Hit: {collision.gameObject.name} (Tag: {collision.tag}, Layer: {LayerMask.LayerToName(collision.gameObject.layer)})");
            Explode();
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasExploded) return;
        
        // ⏳ Chờ collision được enable
        if (!collisionEnabled) return;
        
        // Ignore bom khác
        if (collision.gameObject.CompareTag("Bomb")) return;
        
        // Nổ khi chạm bất kỳ thứ gì
        Debug.Log($"[Bomb] 💥 Collision with: {collision.gameObject.name}");
        Explode();
    }
    
    /// <summary>
    /// 💥 Bom nổ!
    /// </summary>
    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        
        Debug.Log($"[Bomb] 💥 EXPLOSION at {transform.position}!");
        
        // Dừng âm thanh rơi
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        
        // 1. Phát âm thanh nổ
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume);
        }
        
        // 2. Tạo hiệu ứng nổ
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionEffectDuration);
        }
        
        // 3. Gây damage (B52 của Mỹ - target là Player/lính giải phóng!)
        DamageTargetsInRadius();
        
        // 4. Camera shake (nếu có)
        ShakeCamera();
        
        // 5. Hủy bom
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Gây damage trong bán kính nổ
    /// B52 của Mỹ - Target chính là PLAYER (lính giải phóng Việt Nam!)
    /// </summary>
    void DamageTargetsInRadius()
    {
        // Tìm tất cả colliders trong bán kính
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        
        int playersHit = 0;
        int enemiesHit = 0;
        
        foreach (Collider2D hit in hits)
        {
            // 🎯 TARGET CHÍNH: PLAYER (Lính Giải Phóng)
            if (hit.CompareTag("Player"))
            {
                // Gây damage cho player
                LinhGiaiPhong1968 player = hit.GetComponent<LinhGiaiPhong1968>();
                if (player != null)
                {
                    player.TakeDamage(damageToPlayer);
                    playersHit++;
                    Debug.Log($"[B52Bomb] 💥 B52 hit Player! Dealt {damageToPlayer} damage! (Historical: American bombing)");
                }
                else
                {
                    // Fallback: PlayerHealth1968
                    PlayerHealth1968 playerHealth = hit.GetComponent<PlayerHealth1968>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damageToPlayer);
                        playersHit++;
                        Debug.Log($"[B52Bomb] 💥 B52 hit Player! Dealt {damageToPlayer} damage!");
                    }
                }
            }
            
            // ⚠️ FRIENDLY FIRE: Enemies (nếu bật damageAll - realistic)
            if (damageAll && hit.CompareTag("Enemy"))
            {
                // Friendly fire - cả địch cũng bị nổ
                EnemyHealth1968 enemyHealth = hit.GetComponent<EnemyHealth1968>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageToEnemies);
                    enemiesHit++;
                    Debug.Log($"[B52Bomb] 💥 Friendly fire! Enemy hit: {hit.name}, dealt {damageToEnemies} damage!");
                }
                else
                {
                    Enermy1968Controller enemyController = hit.GetComponent<Enermy1968Controller>();
                    if (enemyController != null)
                    {
                        enemyController.TakeDamage(damageToEnemies);
                        enemiesHit++;
                        Debug.Log($"[B52Bomb] 💥 Friendly fire! Enemy hit: {hit.name}, dealt {damageToEnemies} damage!");
                    }
                }
            }
        }
        
        if (playersHit > 0 || enemiesHit > 0)
        {
            Debug.Log($"[B52Bomb] 💥💥 B52 Explosion Results: Players hit: {playersHit}, Enemies hit: {enemiesHit} (friendly fire)");
        }
        else
        {
            Debug.Log($"[B52Bomb] 💨 Explosion missed all targets!");
        }
    }
    
    /// <summary>main
    /// Rung camera khi nổ
    /// </summary>
    void ShakeCamera()
    {
        // Tìm Main Camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("[B52Bomb] ⚠️ Main Camera not found for shake effect!");
            return;
        }
        
        Debug.Log($"[B52Bomb] 🔍 Main Camera found: {mainCamera.name}");
        
        // Kiểm tra CameraShake component trên Main Camera
        CameraShake cameraShake = mainCamera.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            Debug.Log($"[B52Bomb] ✅ Found CameraShake on Main Camera!");
        }
        else
        {
            Debug.Log($"[B52Bomb] ❌ No CameraShake on Main Camera, searching Cinemachine...");
        }
        
        // Nếu không có trên Main Camera, thử tìm trên Cinemachine Camera
        if (cameraShake == null)
        {
            Debug.Log($"[B52Bomb] 🔍 Searching for Cinemachine Camera...");
            
            // Unity 6 / Cinemachine 3.x - Tìm trực tiếp CinemachineCamera
            var cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
            if (cinemachineCamera != null)
            {
                Debug.Log($"[B52Bomb] ✅ Found CinemachineCamera: {cinemachineCamera.name}");
                cameraShake = cinemachineCamera.GetComponent<CameraShake>();
                if (cameraShake != null)
                {
                    Debug.Log($"[B52Bomb] ✅ Found CameraShake on {cinemachineCamera.name}!");
                }
                else
                {
                    Debug.LogError($"[B52Bomb] ❌❌❌ CinemachineCamera '{cinemachineCamera.name}' DOES NOT HAVE CameraShake component! Please add it!");
                }
            }
            else
            {
                Debug.LogError($"[B52Bomb] ❌ No CinemachineCamera found in scene!");
            }
        }
        
        if (cameraShake != null)
        {
            // Rung camera theo khoảng cách
            cameraShake.ShakeFromExplosion(transform.position, shakeMaxDistance, shakeIntensity);
            Debug.Log($"[B52Bomb] 📳 Camera shake triggered! Intensity: {shakeIntensity}");
        }
        else
        {
            Debug.LogWarning("[B52Bomb] ⚠️ CameraShake component not found! Add CameraShake script to Main Camera or Cinemachine Virtual Camera!");
        }
    }
    
    // Debug visualization
    void OnDrawGizmos()
    {
        // Vẽ bán kính nổ (màu đỏ - danger zone)
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f); // Đỏ trong suốt
        Gizmos.DrawSphere(transform.position, explosionRadius);
        
        Gizmos.color = new Color(1f, 0f, 0f, 0.8f); // Đỏ đậm
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        
        // Vẽ vị trí bom
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
    
    void OnDestroy()
    {
        // Cleanup
        if (!hasExploded)
        {
            Debug.Log("[Bomb] 🗑️ Bomb destroyed without exploding (out of bounds?)");
        }
    }
}

