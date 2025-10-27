using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1968 : MonoBehaviour
{
    [Header("Game Over Settings")]
    [SerializeField] private float restartDelay = 3f;
    private bool isGameOver = false;
    
    [Header("UI References (Optional)")]
    [SerializeField] private GameObject gameOverUI; // Assign in Inspector if you have UI
    
    private static GameManager1968 instance;
    
    private void Awake()
    {
        // Singleton pattern (optional)
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        // Hide game over UI at start
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }
    
    public void GameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Debug.Log("=== GAME OVER ===");
        
        // Show game over UI if available
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        
        // Restart game after delay
        Invoke(nameof(RestartGame), restartDelay);
    }
    
    private void RestartGame()
    {
        Debug.Log("Restarting game...");
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    public bool IsGameOver() => isGameOver;
    
    public static GameManager1968 Instance => instance;
}

