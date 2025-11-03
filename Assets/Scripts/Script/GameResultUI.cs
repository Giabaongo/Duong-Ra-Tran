using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameResultUI : MonoBehaviour
{
    [Header("Refs")]
    public CanvasGroup canvasGroup;   // gán CanvasGroup của ResultUI
    public TMP_Text titleText;        // TitleText (CHIẾN THẮNG!, THẤT BẠI!)
    public TMP_Text descText;         // DescText (mô tả nhiệm vụ)
    public Button btnTiepTuc;         // nút "TIẾP TỤC" / "THỬ LẠI"
    public Button btnThoat;           // nút "THOÁT"

    [Header("Win text")]
    [TextArea(2, 4)]
    public string winTitle = "CHIẾN THẮNG!";
    [TextArea(2, 4)]
    public string winDesc =
        "Quân ta đã phá vòng vây ở biên giới.\n" +
        "Tuyến tiếp tế được mở thông.\n" +
        "Chiến dịch kết thúc thắng lợi!";

    [Header("Lose text")]
    [TextArea(2, 4)]
    public string loseTitle = "THẤT BẠI!";
    [TextArea(2, 4)]
    public string loseDesc =
        "Bạn đã gục ngã trên chiến trường...\n" +
        "Kẻ địch vẫn còn rất mạnh.\n" +
        "Thử lại nhiệm vụ?";

    [Header("Scene control")]
    public string replaySceneName = "Map2";     // scene để chơi lại
    public string backToSelectScene = "ChonMan"; // scene quay về chọn màn

    bool showing = false;

    void Awake()
    {
        HideImmediate();

        // Gán hành vi nút
        if (btnTiepTuc != null)
            btnTiepTuc.onClick.AddListener(OnClickTiepTuc);

        if (btnThoat != null)
            btnThoat.onClick.AddListener(OnClickThoat);
    }

    void HideImmediate()
    {
        showing = false;
        gameObject.SetActive(false);

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
        InternalShow(winTitle, winDesc, isWin: true);
    }

    public void ShowLose()
    {
        InternalShow(loseTitle, loseDesc, isWin: false);
    }

    // ------------ CORE ---------------
    void InternalShow(string title, string desc, bool isWin)
    {
        // stop gameplay time
        Time.timeScale = 0f;

        showing = true;
        gameObject.SetActive(true);

        if (titleText) titleText.text = title;
        if (descText) descText.text = desc;

        // Đổi label nút trái tùy win/thua
        if (btnTiepTuc != null)
        {
            var btnTxt = btnTiepTuc.GetComponentInChildren<TMP_Text>();
            if (btnTxt != null)
            {
                btnTxt.text = isWin ? "TIẾP TỤC" : "THỬ LẠI";
            }
        }

        // Fade in nhanh
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
    }

    // ------------ BUTTON HANDLERS ---------------
    // Nút TIẾP TỤC / THỬ LẠI
    void OnClickTiepTuc()
    {
        // resume time trước khi đổi scene
        Time.timeScale = 1f;

        // Nếu win -> quay về chọn màn (ví dụ)
        // Nếu thua -> chơi lại màn hiện tại
        // Cách đơn giản: kiểm tra text của nút
        string currentBtnText = btnTiepTuc.GetComponentInChildren<TMP_Text>().text;
        if (currentBtnText == "TIẾP TỤC")
        {
            // player thắng => về chọn màn
            SceneManager.LoadScene(backToSelectScene);
        }
        else
        {
            // player thua => reload màn
            SceneManager.LoadScene(replaySceneName);
        }
    }

    // Nút THOÁT
    void OnClickThoat()
    {
        Time.timeScale = 1f;
        // về MainMenu (bạn thay tên scene đúng của bạn)
        SceneManager.LoadScene("MainMenu");
    }
}
