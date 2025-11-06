using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StorySceneController : MonoBehaviour
{
    [Header("UI hiển thị")]
    public TextMeshProUGUI storyTitleText;
    public TextMeshProUGUI storyDescriptionText;
    [SerializeField] private string fallbackScene = "Map1";

    private void Start()
    {
        string title = null;
        string desc = null;

        if (GameManager.Instance != null)
        {
            title = GameManager.Instance.PendingMissionTitle;
            desc = GameManager.Instance.PendingMissionDescription;
        }

        if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(desc))
        {
            title = StoryPayload.Title;
            desc = StoryPayload.Description;
        }

        if (string.IsNullOrEmpty(title))
        {
            title = "CHIẾN DỊCH";
        }

        if (string.IsNullOrEmpty(desc))
        {
            desc = "Không có thông tin nhiệm vụ.";
        }

        if (storyTitleText != null)
        {
            storyTitleText.text = title;
        }

        if (storyDescriptionText != null)
        {
            storyDescriptionText.text = desc;
        }
    }

    public void StartBattle()
    {
        string target = null;

        if (GameManager.Instance != null)
        {
            target = GameManager.Instance.ConsumePendingMissionScene(fallbackScene);
        }

        if (string.IsNullOrEmpty(target))
        {
            target = StoryPayload.ConsumeTargetScene(fallbackScene);
        }

        if (string.IsNullOrEmpty(target))
        {
            target = fallbackScene;
        }

        SceneManager.LoadScene(target);
    }

    public void BackToMap()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ClearPendingMission();
        }
        else
        {
            StoryPayload.Clear();
        }

        SceneManager.LoadScene("ChonMan");
    }
}

