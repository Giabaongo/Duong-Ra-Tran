using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour
{
    private static AudioSource sharedSource;
    private static GameObject sharedSourceOwner;
    private static AudioClip sharedDefaultClip;

    [SerializeField] private AudioClip clickClip;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool dontDestroySharedSource = true;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        EnsureSharedSource();
        RegisterSharedClip();
    }

    private void OnEnable()
    {
        if (button != null)
        {
            button.onClick.AddListener(PlayClick);
        }

        RegisterSharedClip();
    }

    private void OnDisable()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(PlayClick);
        }
    }

    private void PlayClick()
    {
        if (sharedSource == null)
        {
            Debug.LogWarning("UIButtonSound: missing shared AudioSource.");
            return;
        }

        AudioClip clipToPlay = clickClip != null ? clickClip : sharedDefaultClip;

        if (clipToPlay == null)
        {
            Debug.LogWarning("UIButtonSound: no AudioClip assigned.", this);
            return;
        }

        if (sharedDefaultClip == null)
        {
            sharedDefaultClip = clipToPlay;
        }

        sharedSource.PlayOneShot(clipToPlay, volume);
    }

    private void EnsureSharedSource()
    {
        if (sharedSource != null)
        {
            return;
        }

        if (sharedSourceOwner == null)
        {
            sharedSourceOwner = new GameObject("UI_Button_AudioSource");
            sharedSource = sharedSourceOwner.AddComponent<AudioSource>();
            sharedSource.playOnAwake = false;
            sharedSource.loop = false;

            if (dontDestroySharedSource)
            {
                DontDestroyOnLoad(sharedSourceOwner);
            }
        }

        if (sharedDefaultClip != null)
        {
            sharedSource.clip = sharedDefaultClip;
        }
    }

    private void RegisterSharedClip()
    {
        if (clickClip != null)
        {
            sharedDefaultClip = clickClip;

            if (sharedSource != null)
            {
                sharedSource.clip = sharedDefaultClip;
            }
        }
    }

    public static void SetSharedClip(AudioClip clip)
    {
        sharedDefaultClip = clip;

        if (sharedSource != null)
        {
            sharedSource.clip = sharedDefaultClip;
        }
    }
}

