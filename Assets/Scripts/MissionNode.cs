using UnityEngine;
using UnityEngine.UI;

public class MissionNode : MonoBehaviour
{
    [Header("Thông tin nhiệm vụ")]
    public string missionName;
    public string displayTitle;
    public string objective;
    public string difficulty;

    [Header("Story")]
    public string storyTitle = "CHIẾN DỊCH";
    [TextArea(3, 6)]
    public string storyDescription =
        "Viết nội dung lịch sử của nhiệm vụ tại đây.\n" +
        "Có thể gõ nhiều dòng để kể câu chuyện trước trận đánh.";

    [Header("Hiển thị / Highlight")]
    public Image icon;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    public enum MissionState { Locked, Available, Cleared }
    public MissionState state = MissionState.Available;

    [Header("Khóa")]
    public GameObject lockIcon;

    private Button button;
    private bool isSelected;

    private void Start()
    {
        button = GetComponent<Button>();
        UpdateVisualState();
    }

    public void SelectNode()
    {
        if (state == MissionState.Locked)
        {
            Debug.Log($"{name}: nhiệm vụ bị khóa, không thể chọn");
            return;
        }

        var ui = FindObjectOfType<MissionSelectUI>();
        if (ui != null)
        {
            ui.SelectMission(this);
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (icon != null)
        {
            icon.color = isSelected ? selectedColor : normalColor;
        }
    }

    public void UpdateVisualState()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        switch (state)
        {
            case MissionState.Locked:
                if (icon != null)
                {
                    icon.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }

                if (button != null)
                {
                    button.interactable = false;
                }

                if (lockIcon != null)
                {
                    lockIcon.SetActive(true);
                }
                break;

            case MissionState.Available:
                if (icon != null)
                {
                    icon.color = normalColor;
                }

                if (button != null)
                {
                    button.interactable = true;
                }

                if (lockIcon != null)
                {
                    lockIcon.SetActive(false);
                }
                break;

            case MissionState.Cleared:
                if (icon != null)
                {
                    icon.color = Color.yellow;
                }

                if (button != null)
                {
                    button.interactable = true;
                }

                if (lockIcon != null)
                {
                    lockIcon.SetActive(false);
                }
                break;
        }
    }
}

