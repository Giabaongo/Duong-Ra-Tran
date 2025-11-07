using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("force")] private float bulletSpeed = 6f;
    [SerializeField] private float damage = 3f;
    [SerializeField] private LayerMask obstacleLayers;

    private GameObject player;
    private Rigidbody2D rb;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        EnsureRigidbodySettings();
        EnsureColliderSettings();
        AcquirePlayerReference();
        LaunchTowardPlayer();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerCollider(other))
        {
            ApplyDamageToPlayer(other);
            Destroy(gameObject);
            return;
        }

        if (HitsObstacle(other))
        {
            Destroy(gameObject);
        }
    }

    public float DamageAmount => damage;

    private void EnsureRigidbodySettings()
    {
        if (rb == null)
        {
            Debug.LogError("EnemyBulletScript: missing Rigidbody2D.");
            return;
        }

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 0f;
    }

    private void EnsureColliderSettings()
    {
        Collider2D bulletCollider = GetComponent<Collider2D>();
        if (bulletCollider == null)
        {
            Debug.LogError("EnemyBulletScript: bullet needs a Collider2D.");
            return;
        }

        bulletCollider.isTrigger = true;
    }

    private void AcquirePlayerReference()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("EnemyBulletScript: Player (tag 'Player') not found.");
        }
    }

    private void LaunchTowardPlayer()
    {
        if (player == null || rb == null)
        {
            return;
        }

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    private bool IsPlayerCollider(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            return true;
        }

        if (other.GetComponent<PlayerHealth>() != null)
        {
            return true;
        }

        if (other.GetComponentInParent<PlayerHealth>() != null)
        {
            return true;
        }

        return false;
    }

    private void ApplyDamageToPlayer(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = other.GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private bool HitsObstacle(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & obstacleLayers) != 0)
        {
            return true;
        }

        if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
        {
            return true;
        }

        if (!other.isTrigger && !other.CompareTag("Enemy") && !other.CompareTag("EnemyBullet"))
        {
            return true;
        }

        return false;
    }
}
