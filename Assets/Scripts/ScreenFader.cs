using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [Header("Assign the black Image (FadePanel) here")]
    public Image fadeImage;

    [Header("Fade settings")]
    public float fadeDuration = 1.0f; // thời gian tối dần (giây)
    public string sceneToLoad = "Map1"; // đổi nếu map bạn tên khác

    bool isFading = false;

    void Awake()
    {
        // đảm bảo panel lúc đầu trong suốt
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    // Hàm này sẽ được gọi khi bấm nút "BẮT ĐẦU CHIẾN DỊCH"
    public void StartFadeAndLoad()
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndLoadRoutine());
        }
    }

    IEnumerator FadeAndLoadRoutine()
    {
        isFading = true;

        float t = 0f;
        Color c = fadeImage.color;

        // Tăng alpha từ 0 -> 1 trong fadeDuration giây
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / fadeDuration);
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        // Khi đã đen hoàn toàn => load scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
