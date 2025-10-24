using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float health = 100f;
    public float speed = 2f;
    public float attackRange = 4f;
    public float chaseRange = 8f;

    private GameObject player;
    private Rigidbody2D rb;
    private Animator anim;
    private float timer;
    private EnemyShooting shooter;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shooter = GetComponent<EnemyShooting>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= chaseRange)
        {
            Chase();
        }
        else
        {
            Idle();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Chase()
    {
        anim.SetBool("IsRunning", true);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        if ((direction.x > 0 && transform.localScale.x < 0) ||
            (direction.x < 0 && transform.localScale.x > 0))
        {
            Flip();
        }
    }

    private void Attack()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsRunning", false);

        timer += Time.deltaTime;
        if (timer >= 2f)
        {
            timer = 0;
            shooter?.Shoot(); // ✅ gọi script EnemyShooting để bắn
        }
    }

    private void Idle()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsRunning", false);
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"Enemy HP: {health}");
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(50);
        }
    }
}
