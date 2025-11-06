using UnityEngine;

/// <summary>
/// Gắn vào collider nhận damage của người chơi, chuyển sát thương tới PlayerHealth.
/// </summary>
public class PlayerDamageReceiver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private float hitVolume = 1f;

    private void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerDamageReceiver: missing PlayerHealth reference!");
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponentInParent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBulletScript bullet = other.GetComponent<EnemyBulletScript>();
        if (other.CompareTag("EnemyBullet") || bullet != null)
        {
            if (playerHealth != null)
            {
                float damage = bullet != null ? bullet.DamageAmount : 10f;
                playerHealth.TakeDamage(damage);
                PlayHitSound();
            }
            else
            {
                Debug.LogError("PlayerDamageReceiver: playerHealth is NULL!");
            }
        }

        DamageSource damageSource = other.GetComponent<DamageSource>();
        if (damageSource != null && damageSource.CompareTag("EnemyWeapon"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageSource.DamageAmount);
                PlayHitSound();
            }
        }
    }

    private void PlayHitSound()
    {
        if (hitClip == null)
        {
            return;
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(hitClip, hitVolume);
            return;
        }

        GameObject temp = new GameObject("PlayerHitTempAudio");
        temp.transform.position = transform.position;
        AudioSource tempSource = temp.AddComponent<AudioSource>();
        tempSource.clip = hitClip;
        tempSource.volume = hitVolume;
        tempSource.spatialBlend = 0f;
        tempSource.Play();
        Destroy(temp, hitClip.length);
    }
}
