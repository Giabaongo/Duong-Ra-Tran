using UnityEngine;
using UnityEngine.SceneManagement;

public class MapIntroController : MonoBehaviour
{
    [SerializeField] private string nextMapScene;

    public void GoToBattle()
    {
        SceneManager.LoadScene(nextMapScene);
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("ChonMan");
    }
}
