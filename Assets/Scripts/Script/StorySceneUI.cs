using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StorySceneUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField, TextArea(3, 6)] private string fallbackBody;
    [SerializeField] private string fallbackTitle = "CHIẾN DỊCH";
    [SerializeField] private string fallbackMissionScene = "Map1";

    [Header("Navigation")]
    [SerializeField] private string backToSelectScene = "ChonMan";

    private void Start()
    {
        PopulateStory();
    }

    private void PopulateStory()
    {
        string title = fallbackTitle;
        string body = fallbackBody;

        bool populated = false;

        if (GameManager.Instance != null)
        {
            if (!string.IsNullOrEmpty(GameManager.Instance.PendingMissionTitle))
            {
                title = GameManager.Instance.PendingMissionTitle;
                populated = true;
            }

            if (!string.IsNullOrEmpty(GameManager.Instance.PendingMissionDescription))
            {
                body = GameManager.Instance.PendingMissionDescription;
                populated = true;
            }
        }

        if (!populated)
        {
            if (!string.IsNullOrEmpty(StoryPayload.Title))
            {
                title = StoryPayload.Title;
            }

            if (!string.IsNullOrEmpty(StoryPayload.Description))
            {
                body = StoryPayload.Description;
            }
        }

        if (titleText != null)
        {
            titleText.text = title;
        }

        if (bodyText != null)
        {
            bodyText.text = body;
        }
    }

    public void OnClickStartMission()
    {
        string targetScene = fallbackMissionScene;
        bool consumedFromPayload = false;

        if (GameManager.Instance != null)
        {
            targetScene = GameManager.Instance.ConsumePendingMissionScene(fallbackMissionScene);
        }

        if (string.IsNullOrEmpty(targetScene))
        {
            targetScene = StoryPayload.ConsumeTargetScene(fallbackMissionScene);
            consumedFromPayload = true;
        }

        if (string.IsNullOrEmpty(targetScene))
        {
            Debug.LogError("StorySceneUI: target scene is empty. Please check MissionNode.missionName and StorySceneUI fallbackMissionScene.");
            return;
        }

        if (!consumedFromPayload)
        {
            StoryPayload.Clear();
        }

        SceneManager.LoadScene(targetScene);
    }

    public void OnClickBack()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ClearPendingMission();
        }
        else
        {
            StoryPayload.Clear();
        }

        SceneManager.LoadScene(backToSelectScene);
    }
}
