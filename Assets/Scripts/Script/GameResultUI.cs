// 06/11/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameResultUI : MonoBehaviour
{
    [Header("Refs")]
    public CanvasGroup canvasGroup;   // gan CanvasGroup cua ResultUI
    public TMP_Text titleText;        // TitleText (CHIEN THANG!, THAT BAI!)
    public TMP_Text descText;         // DescText (mo ta nhiem vu)
    public Button btnTiepTuc;         // nut "TIEP TUC" / "THU LAI"
    public Button btnThoat;           // nut "THOAT"

    [Header("Win text")]
    [TextArea(2, 4)]
    public string winTitle = "CHIẾN THẮNG!";
    [TextArea(2, 4)]
    public string winDesc =
        "Quân ta đã phá vỡ vòng vây địch.\n" +
        "Tuyệt đối không được mất thăng bằng.\n" +
        "Chiến dịch kết thúc thắng lợi!";

    [Header("Lose text")]
    [TextArea(2, 4)]
    public string loseTitle = "THẤT BẠI!";
    [TextArea(2, 4)]
    public string loseDesc =
        "Bạn đã gục ngã trên chiến trường...\n" +
        "Kẻ địch vẫn còn rất mạnh.\n" +
        "Thất bại nhiệm vụ!";

    [Header("Scene control")]
    public string replaySceneName = "Map1";     // se duoc gan tu dong theo scene
    public string backToSelectScene = "ChonMan"; // quay ve man chon

    public bool IsShowing { get; private set; }

    private bool lastResultWasWin;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
        HandleSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void Awake()
    {
        replaySceneName = SceneManager.GetActiveScene().name; // fallback neu OnEnable chua chay
        HideImmediate();

        if (btnTiepTuc != null)
        {
            btnTiepTuc.onClick.AddListener(OnClickTiepTuc);
        }

        if (btnThoat != null)
        {
            btnThoat.onClick.AddListener(OnClickThoat);
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        replaySceneName = scene.name; // khi thua -> reload dung scene dang choi
    }

    private void HideImmediate()
    {
        Time.timeScale = 1f; // resume gameplay khi UI bi an
        IsShowing = false;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

    // ------------ PUBLIC API ---------------
    public void ShowWin()
    {
        InternalShow(winTitle, winDesc, true);
    }

    public void ShowLose()
    {
        InternalShow(loseTitle, loseDesc, false);
    }

    // ------------ CORE ---------------
    private void InternalShow(string title, string desc, bool isWin)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        IsShowing = true;
        lastResultWasWin = isWin;

        if (titleText != null) titleText.text = title;
        if (descText != null) descText.text = desc;

        if (btnTiepTuc != null)
        {
            var btnTxt = btnTiepTuc.GetComponentInChildren<TMP_Text>();
            if (btnTxt != null)
            {
                btnTxt.text = isWin ? "TIẾP TỤC" : "THỬ LẠI";
            }
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
    }

    // ------------ BUTTON HANDLERS ---------------
    private void OnClickTiepTuc()
    {
        Time.timeScale = 1f;

        if (lastResultWasWin)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.HandleContinueAfterWin();
            }
            else
            {
                SceneManager.LoadScene(backToSelectScene);
            }
        }
        else
        {
            SceneManager.LoadScene(replaySceneName);
        }
    }

    private void OnClickThoat()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
