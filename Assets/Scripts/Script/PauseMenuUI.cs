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

    void HideInstant()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        gameObject.SetActive(false); // Đảm bảo giao diện bị ẩn hoàn toàn
    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            ShowPause();
        }
        else
        {
            HidePause();
        }
    }

    void ShowPause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (titleText != null) titleText.text = "TẠM DỪNG";
        if (descText != null) descText.text = "Trận đánh đang tạm dừng.";

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; // Hiển thị giao diện
            canvasGroup.interactable = true; // Cho phép tương tác
            canvasGroup.blocksRaycasts = true; // Cho phép nhận sự kiện
        }

        gameObject.SetActive(true); // Hiển thị giao diện
    }

    void HidePause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f; // Ẩn giao diện
            canvasGroup.interactable = false; // Ngừng tương tác
            canvasGroup.blocksRaycasts = false; // Ngừng nhận sự kiện
        }

        gameObject.SetActive(false); // Ẩn giao diện
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