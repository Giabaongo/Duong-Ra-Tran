using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script hiển thị thanh máu của player
/// Hỗ trợ cả PlayerHealth1968 và LinhGiaiPhong1968
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Header("Colors")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color halfHealthColor = Color.yellow;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private bool useGradientColor = true;
    
    [Header("Animation")]
    [SerializeField] private bool smoothTransition = true;
    [SerializeField] private float transitionSpeed = 5f;
    
    private PlayerHealth1968 playerHealth;
    private LinhGiaiPhong1968 linhGiaiPhong;
    
    private float targetHealthPercentage;
    private int currentHealth;
    private int maxHealth;
    
    private void Start()
    {
        InitializeHealthBar();
    }
    
    private void InitializeHealthBar()
    {
        // Tìm player trong scene - ƯU TIÊN LinhGiaiPhong1968 TRƯỚC!
        linhGiaiPhong = FindObjectOfType<LinhGiaiPhong1968>();
        playerHealth = FindObjectOfType<PlayerHealth1968>();
        
        if (linhGiaiPhong == null && playerHealth == null)
        {
            Debug.LogError("[HealthBarUI] Không tìm thấy PlayerHealth1968 hoặc LinhGiaiPhong1968 trong scene!");
            gameObject.SetActive(false);
            return;
        }
        
        // Ưu tiên LinhGiaiPhong1968 nếu có (script chính đang dùng)
        if (linhGiaiPhong != null)
        {
            maxHealth = linhGiaiPhong.GetMaxHealth();
            currentHealth = linhGiaiPhong.GetCurrentHealth();
            
            // Subscribe to health changed event
            linhGiaiPhong.OnHealthChanged.AddListener(OnPlayerHealthChanged);
            Debug.Log("[HealthBarUI] ✅ Subscribed to LinhGiaiPhong1968.OnHealthChanged event");
        }
        else if (playerHealth != null)
        {
            maxHealth = playerHealth.GetMaxHealth();
            currentHealth = playerHealth.GetCurrentHealth();
            
            // Subscribe to health changed event
            playerHealth.OnHealthChanged.AddListener(OnPlayerHealthChanged);
            Debug.Log("[HealthBarUI] ✅ Subscribed to PlayerHealth1968.OnHealthChanged event");
        }
        
        // Setup slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = 1f;
            healthSlider.value = 1f;
        }
        
        // Cập nhật lần đầu
        targetHealthPercentage = 1f;
        UpdateUI(1f);
        
        Debug.Log($"[HealthBarUI] Initialized - Max Health: {maxHealth}, Current Health: {currentHealth}");
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged.RemoveListener(OnPlayerHealthChanged);
        }
        if (linhGiaiPhong != null)
        {
            linhGiaiPhong.OnHealthChanged.RemoveListener(OnPlayerHealthChanged);
        }
    }
    
    /// <summary>
    /// Event callback khi HP của player thay đổi - CẬP NHẬT NGAY LẬP TỨC!
    /// </summary>
    private void OnPlayerHealthChanged(int newHealth, int newMaxHealth)
    {
        currentHealth = newHealth;
        maxHealth = newMaxHealth;
        
        float healthPercentage = (float)currentHealth / maxHealth;
        targetHealthPercentage = healthPercentage;
        
        Debug.Log($"[HealthBarUI] ⚡ Health Changed! {currentHealth}/{maxHealth} ({healthPercentage:P0})");
        
        // Nếu không dùng smooth transition, cập nhật ngay
        if (!smoothTransition)
        {
            UpdateUI(healthPercentage);
        }
        
        // Hiển thị damage effect nếu bị sát thương
        if (newHealth < currentHealth)
        {
            ShowDamageEffect();
        }
    }
    
    private void Update()
    {
        // Chỉ dùng Update cho smooth transition animation
        if (smoothTransition && healthSlider != null)
        {
            float currentSliderValue = healthSlider.value;
            if (Mathf.Abs(currentSliderValue - targetHealthPercentage) > 0.01f)
            {
                float newValue = Mathf.Lerp(currentSliderValue, targetHealthPercentage, Time.deltaTime * transitionSpeed);
                UpdateUI(newValue);
            }
        }
    }
    
    
    private void UpdateUI(float healthPercentage)
    {
        // Cập nhật slider
        if (healthSlider != null)
        {
            healthSlider.value = healthPercentage;
        }
        
        // Cập nhật màu sắc
        if (fillImage != null && useGradientColor)
        {
            fillImage.color = GetHealthColor(healthPercentage);
        }
        
        // Cập nhật text
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
    
    private Color GetHealthColor(float healthPercentage)
    {
        if (healthPercentage > 0.5f)
        {
            // Từ xanh -> vàng (100% -> 50%)
            return Color.Lerp(halfHealthColor, fullHealthColor, (healthPercentage - 0.5f) * 2f);
        }
        else
        {
            // Từ đỏ -> vàng (0% -> 50%)
            return Color.Lerp(lowHealthColor, halfHealthColor, healthPercentage * 2f);
        }
    }
    
    /// <summary>
    /// Gọi method này để cập nhật ngay lập tức (không smooth)
    /// </summary>
    public void ForceUpdate()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        UpdateUI(healthPercentage);
    }
    
    /// <summary>
    /// Hiển thị hiệu ứng khi bị damage (tùy chọn)
    /// </summary>
    public void ShowDamageEffect()
    {
        // Có thể thêm animation shake hoặc flash ở đây
        if (fillImage != null)
        {
            StartCoroutine(FlashEffect());
        }
    }
    
    private System.Collections.IEnumerator FlashEffect()
    {
        Color originalColor = fillImage.color;
        fillImage.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        fillImage.color = originalColor;
    }
    
    // Debug
    private void OnValidate()
    {
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }
        
        if (fillImage == null && healthSlider != null)
        {
            fillImage = healthSlider.fillRect?.GetComponent<Image>();
        }
    }
}

