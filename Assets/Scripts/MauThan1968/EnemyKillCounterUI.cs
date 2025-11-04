using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Hiển thị số enemy đã tiêu diệt ở góc phải màn hình
/// </summary>
public class EnemyKillCounterUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI counterText; // Nếu dùng TextMeshPro
    [SerializeField] private Text legacyText; // Nếu dùng Unity UI Text thường
    
    [Header("Display Settings")]
    [SerializeField] private string displayFormat = "Enemies Killed: {0}/{1}";
    [SerializeField] private bool showProgress = true; // Hiển thị x/total hay chỉ x
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color completedColor = Color.green;
    
    [Header("Animation")]
    [SerializeField] private bool animateOnKill = true;
    [SerializeField] private float animationDuration = 0.3f;
    
    private GameManager1968 gameManager;
    private int lastKillCount = 0;
    private Vector3 originalScale;
    
    private void Start()
    {
        // Tìm GameManager
        gameManager = GameManager1968.Instance;
        
        if (gameManager == null)
        {
            Debug.LogError("[EnemyKillCounterUI] GameManager1968 not found in scene!");
            enabled = false;
            return;
        }
        
        // Lưu scale ban đầu để animate
        originalScale = transform.localScale;
        
        // Update lần đầu
        UpdateDisplay();
        
        Debug.Log("[EnemyKillCounterUI] Initialized successfully!");
    }
    
    private void Update()
    {
        if (gameManager == null) return;
        
        // Kiểm tra nếu có enemy mới bị giết
        int currentKillCount = gameManager.GetEnemiesKilled();
        
        if (currentKillCount != lastKillCount)
        {
            lastKillCount = currentKillCount;
            UpdateDisplay();
            
            // Animate khi có kill mới
            if (animateOnKill)
            {
                AnimateCounter();
            }
        }
    }
    
    private void UpdateDisplay()
    {
        if (gameManager == null) return;
        
        int killed = gameManager.GetEnemiesKilled();
        int total = gameManager.GetTotalEnemies();
        
        // Tạo text hiển thị
        string displayText;
        if (showProgress && total > 0)
        {
            displayText = string.Format(displayFormat, killed, total);
        }
        else
        {
            displayText = $"Kills: {killed}";
        }
        
        // Update text component
        if (counterText != null)
        {
            counterText.text = displayText;
            
            // Đổi màu nếu hoàn thành
            if (total > 0 && killed >= total)
            {
                counterText.color = completedColor;
            }
            else
            {
                counterText.color = normalColor;
            }
        }
        else if (legacyText != null)
        {
            legacyText.text = displayText;
            
            // Đổi màu nếu hoàn thành
            if (total > 0 && killed >= total)
            {
                legacyText.color = completedColor;
            }
            else
            {
                legacyText.color = normalColor;
            }
        }
        else
        {
            Debug.LogWarning("[EnemyKillCounterUI] No Text component assigned!");
        }
    }
    
    private void AnimateCounter()
    {
        // Cancel animation cũ nếu có
        StopAllCoroutines();
        
        // Start animation mới
        StartCoroutine(AnimateScale());
    }
    
    private System.Collections.IEnumerator AnimateScale()
    {
        float elapsed = 0f;
        Vector3 targetScale = originalScale * 1.3f; // Scale to 130%
        
        // Scale up
        while (elapsed < animationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (animationDuration / 2);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        
        elapsed = 0f;
        
        // Scale down
        while (elapsed < animationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (animationDuration / 2);
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
        
        // Đảm bảo về đúng scale ban đầu
        transform.localScale = originalScale;
    }
    
    // Public method để force update (nếu cần)
    public void ForceUpdate()
    {
        UpdateDisplay();
    }
}

