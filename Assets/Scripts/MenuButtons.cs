using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Gọi khi bấm "BẮT ĐẦU CHIẾN DỊCH"
    public void StartNewGame()
    {
        Debug.Log("StartNewGame() called - loading ChonMan");
        SceneManager.LoadScene("ChonMan");
    }

    // Gọi khi bấm "TIẾP TỤC HÀNH TRÌNH"
    public void ContinueGame()
    {
        Debug.Log("ContinueGame() called - loading ChonMan (tiếp tục)");
        SceneManager.LoadScene("ChonMan");
        // sau này bạn có thể đổi thành load map player đang dở
    }

    // Gọi khi bấm "THIẾT LẬP"
    public void OpenSettings()
    {
        Debug.Log("OpenSettings() called - (chưa làm menu cài đặt)");
        // TODO: mở popup Settings nếu bạn có UI
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
