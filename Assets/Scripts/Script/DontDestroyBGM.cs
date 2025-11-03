using UnityEngine;

public class DontDestroyBGM : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
