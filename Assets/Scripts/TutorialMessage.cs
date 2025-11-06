using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialMessageTMP : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public float displayTime = 10f;
    public float fadeDuration = 2f;

    void Start()
    {
        tutorialText.gameObject.SetActive(true);
        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        yield return StartCoroutine(FadeOut());
        tutorialText.gameObject.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        Color color = tutorialText.color;
        float startAlpha = color.a;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalized = t / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, 0, normalized);
            tutorialText.color = color;
            yield return null;
        }

        color.a = 0;
        tutorialText.color = color;
    }
}
