using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script để load scenes
/// Attach vào Button hoặc GameObject bất kỳ
/// </summary>
public class SceneLoader1968 : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "MauthanScene";
    [SerializeField] private string startSceneName = "Start";
    
    [Header("Load Settings")]
    [SerializeField] private float loadDelay = 0f; // Delay trước khi load (giây)
    
    /// <summary>
    /// Load scene game chính (MauthanScene)
    /// </summary>
    public void LoadGameScene()
    {
        Debug.Log($"[SceneLoader] Loading game scene: {gameSceneName}");
        
        if (loadDelay > 0)
        {
            Invoke(nameof(LoadGame), loadDelay);
        }
        else
        {
            LoadGame();
        }
    }
    
    private void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
    /// <summary>
    /// Load scene Start (menu chính)
    /// </summary>
    public void LoadStartScene()
    {
        Debug.Log($"[SceneLoader] Loading start scene: {startSceneName}");
        SceneManager.LoadScene(startSceneName);
    }
    
    /// <summary>
    /// Restart scene hiện tại
    /// </summary>
    public void RestartCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"[SceneLoader] Restarting scene: {currentScene}");
        SceneManager.LoadScene(currentScene);
    }
    
    /// <summary>
    /// Quit game
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("[SceneLoader] Quitting game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    /// <summary>
    /// Load scene theo tên (flexible)
    /// </summary>
    public void LoadSceneByName(string sceneName)
    {
        Debug.Log($"[SceneLoader] Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// Load scene theo index
    /// </summary>
    public void LoadSceneByIndex(int sceneIndex)
    {
        Debug.Log($"[SceneLoader] Loading scene index: {sceneIndex}");
        SceneManager.LoadScene(sceneIndex);
    }
}

