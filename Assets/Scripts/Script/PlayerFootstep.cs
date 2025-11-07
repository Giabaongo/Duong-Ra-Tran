using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float minSpeedToPlay = 0.1f;
    [SerializeField] private float stepInterval = 0.35f;
    [SerializeField] private float volume = 0.65f;
    [SerializeField] private bool stopImmediatelyOnIdle = true;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private float stepTimer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        if (audioSource == null)
        {
            Debug.LogError("PlayerFootstep: missing AudioSource on " + name);
        }

        if (rb == null)
        {
            Debug.LogError("PlayerFootstep: missing Rigidbody2D on " + name);
        }
    }

    private void Update()
    {
        if (audioSource == null || rb == null)
        {
            return;
        }

        float speed = rb.linearVelocity.magnitude;

        if (speed > minSpeedToPlay)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = stepInterval;

            if (stopImmediatelyOnIdle && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void PlayFootstep()
    {
        if (footstepClip == null && audioSource.clip == null)
        {
            Debug.LogWarning("PlayerFootstep: no footstep clip assigned.", this);
            return;
        }

        AudioClip clipToPlay = footstepClip != null ? footstepClip : audioSource.clip;
        audioSource.Stop();
        audioSource.clip = clipToPlay;
        audioSource.volume = volume;
        audioSource.time = 0f;
        audioSource.Play();
    }
}
