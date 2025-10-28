using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    void Update()
    {
        Debug.Log("MenuButtons is alive");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("ChonMan");
    }

    public void ContinueGame()
    {
        Debug.Log("ContinueGame() called");
    }

    public void OpenSettings()
    {
        Debug.Log("OpenSettings() called");
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame() called - exiting game...");
        Application.Quit();
    }
}
