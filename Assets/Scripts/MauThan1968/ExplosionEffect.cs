using UnityEngine;

/// <summary>
/// Script đơn giản cho hiệu ứng nổ
/// Có thể dùng Particle System hoặc Sprite Animation
/// </summary>
public class ExplosionEffect : MonoBehaviour
{
    [Header("⚙️ SETTINGS")]
    [Tooltip("Tự hủy sau bao nhiêu giây")]
    [SerializeField] private float lifetime = 2f;
    
    [Tooltip("Scale hiệu ứng nổ")]
    [SerializeField] private Vector3 explosionScale = new Vector3(2f, 2f, 1f);
    
    [Tooltip("Fade out trong 0.5s cuối")]
    [SerializeField] private bool fadeOut = true;
    
    [Header("🎨 SPRITE ANIMATION (Optional)")]
    [Tooltip("Các sprite animation frames (nếu dùng sprite)")]
    [SerializeField] private Sprite[] explosionSprites;
    
    [Tooltip("FPS của animation")]
    [SerializeField] private float animationFPS = 30f;
    
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particleSystem;
    private float timer = 0f;
    private int currentFrame = 0;
    
    void Start()
    {
        // Set scale
        transform.localScale = explosionScale;
        
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
        
        // Setup Particle System nếu có
        if (particleSystem != null)
        {
            particleSystem.Play();
            Debug.Log("[Explosion] 🎆 Particle system playing!");
        }
        
        // Setup Sprite Renderer nếu có
        if (spriteRenderer != null && explosionSprites != null && explosionSprites.Length > 0)
        {
            spriteRenderer.sprite = explosionSprites[0];
            spriteRenderer.sortingOrder = 100; // Hiện trên cùng
            Debug.Log($"[Explosion] 🎨 Sprite animation: {explosionSprites.Length} frames");
        }
        
        // Tự hủy sau lifetime
        Destroy(gameObject, lifetime);
        
        Debug.Log($"[Explosion] 💥 Explosion effect created at {transform.position}");
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        // Sprite animation
        if (spriteRenderer != null && explosionSprites != null && explosionSprites.Length > 0)
        {
            int targetFrame = Mathf.FloorToInt(timer * animationFPS);
            
            if (targetFrame != currentFrame && targetFrame < explosionSprites.Length)
            {
                currentFrame = targetFrame;
                spriteRenderer.sprite = explosionSprites[currentFrame];
            }
        }
        
        // Fade out
        if (fadeOut && spriteRenderer != null)
        {
            float fadeStartTime = lifetime - 0.5f;
            if (timer >= fadeStartTime)
            {
                float alpha = 1f - (timer - fadeStartTime) / 0.5f;
                Color c = spriteRenderer.color;
                c.a = alpha;
                spriteRenderer.color = c;
            }
        }
    }
}

