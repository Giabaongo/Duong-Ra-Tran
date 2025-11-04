using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Gọi khi bấm "BẮT ĐẦU CHIẾN DỊCH"
    public GameObject settingsUI;
    public void StartNewGame()
    {
        Debug.Log("StartNewGame() called - loading ChonMan");
        SceneManager.LoadScene("ChonMan");
    }

    // Gọi khi bấm "TIẾP TỤC HÀNH TRÌNH"
    public void ContinueGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadGame(); // Gọi phương thức LoadGame từ GameManager
            Debug.Log("Continuing game...");
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }


    public void OpenSettings()
    {
        if (settingsUI != null)
        {
            settingsUI.SetActive(true); // Hiển thị giao diện thiết lập
            Debug.Log("Settings menu opened");
        }
        else
        {
            Debug.LogError("settingsUI is not assigned in the Inspector!");
        }
    }

    public void CloseSettings()
    {
        if (settingsUI != null)
        {
            settingsUI.SetActive(false); // Ẩn giao diện thiết lập
            Debug.Log("Settings menu closed");
        }
        else
        {
            Debug.LogError("settingsUI is not assigned in the Inspector!");
        }
    }

    public void SetVolume(float volume)
    {
        Debug.Log($"Volume set to: {volume}");
        // Thêm logic để điều chỉnh âm lượng
        AudioListener.volume = volume;
    }

    public void ToggleMute(bool isMuted)
    {
        Debug.Log($"Mute toggled: {isMuted}");
        // Thêm logic để bật/tắt âm thanh
        AudioListener.pause = isMuted;
    }
    // Gọi khi bấm "RÚT LUI"
    public void QuitGame()
    {
        Debug.Log("QuitGame() called - quitting game/app");
        Application.Quit();

        // Trong Editor thì Quit() không thoát được, nên chỉ in log
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
