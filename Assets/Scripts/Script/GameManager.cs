// 31/10/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Đảm bảo GameManager không bị hủy khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGame()
    {
        Debug.Log("Loading previous game state...");
        // Thêm logic để load trạng thái game trước đó
    }
}