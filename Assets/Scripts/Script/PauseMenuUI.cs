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
    public string replaySceneName = "Map2";
    public string retreatSceneName = "ChonMan";

    private bool isPaused = false;

    void Start()
    {
        // Ban đầu ẩn menu
        HideInstant();
    }

    public void HideInstant()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(true); // Đảm bảo rằng PauseUI vẫn được kích hoạt
    }

    public void TogglePause()
    {
        Debug.Log("TogglePause method called"); // Thêm dòng này để kiểm tra
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
            canvas.enabled = true; // Kích hoạt Canvas
        }
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HidePause()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        Debug.Log("Pause menu is now hidden"); // Thêm dòng này để kiểm tra
    }

    public void OnResumePressed()
    {
        HidePause();
    }

    public void OnReplayPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(replaySceneName);
    }

    public void OnRetreatPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(retreatSceneName);
    }
}