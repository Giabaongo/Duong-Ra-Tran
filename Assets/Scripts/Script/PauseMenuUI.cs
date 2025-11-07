// 31/10/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Refs")]
    public CanvasGroup canvasGroup;
    public TMP_Text titleText;
    public TMP_Text descText;
    public UnityEngine.UI.Button btnResume;
    public UnityEngine.UI.Button btnReplay;
    public UnityEngine.UI.Button btnRetreat;

    [Header("Scene Names")]
    public string replaySceneName = "Map2"; // se duoc cap nhat theo scene
    public string retreatSceneName = "ChonMan";

    private bool isPaused = false;
    private string currentSceneName;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
        HandleSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    void Start()
    {
        HideInstant();
    }

    public void HideInstant()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(true);
    }

    public void TogglePause()
    {
        Debug.Log("TogglePause method called");
        if (canvasGroup.alpha == 0)
        {
            ShowPause();
        }
        else
        {
            HidePause();
        }
    }

    public void ShowPause()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = true;
        }
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void HidePause()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Pause menu is now hidden");
    }

    public void OnResumePressed()
    {
        HidePause();
    }

    public void OnReplayPressed()
    {
        Time.timeScale = 1f;
        string targetScene = !string.IsNullOrEmpty(replaySceneName) ? replaySceneName : currentSceneName;
        if (string.IsNullOrEmpty(targetScene))
        {
            targetScene = SceneManager.GetActiveScene().name;
        }
        SceneManager.LoadScene(targetScene);
    }

    public void OnRetreatPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(retreatSceneName);
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        replaySceneName = currentSceneName;
    }
}

