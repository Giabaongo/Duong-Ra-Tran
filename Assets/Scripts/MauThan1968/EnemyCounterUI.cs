using UnityEngine;
using TMPro;

/// <summary>
/// Script hiển thị số enemy đã tiêu diệt trên UI
/// Attach vào Text object trong Canvas
/// </summary>
public class EnemyCounterUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private string textFormat = "Enemies: {0}/{1}"; // Format: "Enemies: 3/6"
    
    private GameManager1968 gameManager;
    
    private void Start()
    {
        // Tìm references
        if (counterText == null)
        {
            counterText = GetComponent<TextMeshProUGUI>();
        }
        
        gameManager = GameManager1968.Instance;
        
        if (gameManager == null)
        {
            Debug.LogError("[EnemyCounterUI] GameManager1968 not found!");
        }
    }
    
    private void Update()
    {
        // Cập nhật text mỗi frame
        if (gameManager != null && counterText != null)
        {
            int killed = gameManager.GetEnemiesKilled();
            int total = gameManager.GetTotalEnemies();
            counterText.text = string.Format(textFormat, killed, total);
        }
    }
}

