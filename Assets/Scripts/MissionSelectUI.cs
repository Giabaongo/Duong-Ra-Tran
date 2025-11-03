using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MissionSelectUI : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public MissionNode selectedNode;

    public void SelectMission(MissionNode node)
    {
        // bỏ highlight node cũ
        if (selectedNode != null)
            selectedNode.SetSelected(false);

        // ghi lại node mới
        selectedNode = node;
        selectedNode.SetSelected(true);

        UpdateInfoPanel();

        Debug.Log("Đã chọn nhiệm vụ: " + selectedNode.displayTitle);
    }

    void UpdateInfoPanel()
    {
        if (selectedNode == null)
        {
            if (infoText != null)
                infoText.text = "CHỌN MỘT NHIỆM VỤ TRÊN BẢN ĐỒ";
        }
        else
        {
            if (infoText != null)
            {
                infoText.text =
                    selectedNode.displayTitle + "\n" +
                    "MỤC TIÊU: " + selectedNode.objective + "\n" +
                    "ĐỘ KHÓ: " + selectedNode.difficulty;
            }
        }
    }

    public void StartMission()
    {
        if (selectedNode == null)
        {
            Debug.LogWarning("Chưa chọn nhiệm vụ, không thể bắt đầu!");
            return;
        }

        string sceneName = selectedNode.missionName;
        Debug.Log("Bắt đầu nhiệm vụ: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
