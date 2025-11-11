using UnityEngine;

/// <summary>
/// Script điều khiển hiệu ứng máy bay B52 bay qua map
/// Attach vào GameObject có SpriteRenderer (bóng máy bay)
/// </summary>
public class B52Flyover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector2 startPosition = new Vector2(-20f, 10f); // Vị trí bắt đầu (ngoài màn hình)
    [SerializeField] private Vector2 endPosition = new Vector2(20f, -10f); // Vị trí kết thúc (ngoài màn hình)
    [SerializeField] private float flySpeed = 5f; // Tốc độ bay (units/second)
    [SerializeField] private float flightDuration = 0f; // Tự động tính hoặc set thủ công (0 = auto)
    
    [Header("Visual Settings")]
    [SerializeField] private bool fadeInOut = true; // Fade khi vào/ra
    [SerializeField] private float fadeDistance = 3f; // Khoảng cách fade
    [SerializeField] private float shadowOpacity = 0.5f; // Độ mờ bóng (0-1)
    [SerializeField] private Vector3 shadowScale = new Vector3(1.2f, 1.2f, 1f); // Scale bóng so với máy bay
    
    [Header("Audio Settings")]
    [SerializeField] private AudioClip engineSound; // Âm thanh động cơ B52
    [SerializeField] private bool loopSound = true; // Loop âm thanh trong khi bay
    [SerializeField] private float maxVolume = 0.8f; // Volume tối đa
    [SerializeField] private float audioFadeDistance = 10f; // Khoảng cách fade âm thanh
    
    [Header("Automation")]
    [SerializeField] private bool autoStart = false; // Tự động bay khi game start
    [SerializeField] private float autoStartDelay = 2f; // Delay trước khi bay
    [SerializeField] private bool destroyOnComplete = true; // Tự hủy khi bay xong
    
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool isFlying = false;
    private float journeyProgress = 0f;
    private float totalDistance = 0f;
    private Camera mainCamera;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("[B52Flyover] ❌ SpriteRenderer not found! This script requires a SpriteRenderer!");
        }
        
        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.clip = engineSound;
        audioSource.loop = loopSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound (không spatial)
        
        mainCamera = Camera.main;
        
        // Tính tổng khoảng cách
        totalDistance = Vector2.Distance(startPosition, endPosition);
    }
    
    void Start()
    {
        // Set vị trí ban đầu
        transform.position = startPosition;
        transform.localScale = shadowScale;
        
        // Set alpha ban đầu
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0f; // Bắt đầu trong suốt
            spriteRenderer.color = c;
        }
        
        // Auto start nếu bật
        if (autoStart)
        {
            Invoke(nameof(StartFlyover), autoStartDelay);
        }
        
        Debug.Log($"[B52Flyover] ✈️ B52 ready! Distance: {totalDistance:F1}m, Speed: {flySpeed}m/s");
    }
    
    void Update()
    {
        if (!isFlying) return;
        
        // Tính movement
        float step = flySpeed * Time.deltaTime / totalDistance;
        journeyProgress += step;
        
        // Di chuyển theo đường thẳng
        transform.position = Vector2.Lerp(startPosition, endPosition, journeyProgress);
        
        // Cập nhật opacity (fade in/out)
        if (fadeInOut && spriteRenderer != null)
        {
            float alpha = CalculateFadeAlpha();
            Color c = spriteRenderer.color;
            c.a = alpha * shadowOpacity;
            spriteRenderer.color = c;
        }
        
        // Cập nhật volume âm thanh (fade dựa trên khoảng cách đến camera)
        if (audioSource != null && engineSound != null && mainCamera != null)
        {
            float distanceToCamera = Vector2.Distance(transform.position, mainCamera.transform.position);
            float volumeFactor = 1f - Mathf.Clamp01(distanceToCamera / audioFadeDistance);
            audioSource.volume = volumeFactor * maxVolume;
        }
        
        // Kiểm tra hoàn thành
        if (journeyProgress >= 1f)
        {
            CompleteFlyover();
        }
    }
    
    /// <summary>
    /// Bắt đầu hiệu ứng bay qua
    /// </summary>
    public void StartFlyover()
    {
        if (isFlying)
        {
            Debug.LogWarning("[B52Flyover] Already flying!");
            return;
        }
        
        isFlying = true;
        journeyProgress = 0f;
        
        // Phát âm thanh
        if (audioSource != null && engineSound != null)
        {
            audioSource.Play();
            Debug.Log("[B52Flyover] 🔊 Playing B52 engine sound");
        }
        
        Debug.Log("[B52Flyover] ✈️ B52 flyover started!");
    }
    
    /// <summary>
    /// Hoàn thành bay qua
    /// </summary>
    private void CompleteFlyover()
    {
        isFlying = false;
        
        // Dừng âm thanh
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        
        Debug.Log("[B52Flyover] ✅ B52 flyover completed!");
        
        // Tự hủy nếu bật
        if (destroyOnComplete)
        {
            Destroy(gameObject, 0.5f);
        }
    }
    
    /// <summary>
    /// Tính alpha cho fade in/out effect
    /// </summary>
    private float CalculateFadeAlpha()
    {
        float distanceFromStart = journeyProgress * totalDistance;
        float distanceFromEnd = (1f - journeyProgress) * totalDistance;
        
        float fadeIn = Mathf.Clamp01(distanceFromStart / fadeDistance);
        float fadeOut = Mathf.Clamp01(distanceFromEnd / fadeDistance);
        
        return Mathf.Min(fadeIn, fadeOut);
    }
    
    /// <summary>
    /// Set custom start/end positions (call before StartFlyover)
    /// </summary>
    public void SetFlightPath(Vector2 start, Vector2 end)
    {
        startPosition = start;
        endPosition = end;
        totalDistance = Vector2.Distance(start, end);
        transform.position = start;
        
        Debug.Log($"[B52Flyover] Flight path updated: {start} → {end} (Distance: {totalDistance:F1}m)");
    }
    
    /// <summary>
    /// Set tốc độ bay
    /// </summary>
    public void SetSpeed(float speed)
    {
        flySpeed = speed;
    }
    
    // Debug visualization
    private void OnDrawGizmos()
    {
        // Draw flight path
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosition, endPosition);
        
        // Draw start point
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, 0.5f);
        
        // Draw end point
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPosition, 0.5f);
        
        // Draw current position if flying
        if (Application.isPlaying && isFlying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}


