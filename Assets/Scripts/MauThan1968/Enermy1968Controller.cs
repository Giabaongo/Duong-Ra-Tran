using UnityEngine;

public class Enermy1968Controller : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 2;
    private int currentHealth;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float patrolRange = 5f;
    private Vector2 patrolStartPos;
    private bool movingRight = true;
    
    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private LayerMask playerLayer;
    private Transform player;
    
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float bulletSpeed = 10f;
    private float lastShootTime = 0f;
    
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    [Header("State")]
    private bool isAttacking = false;
    private bool isHit = false;
    private bool isDead = false;
    private bool facingRight = true;
    
    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }
    private EnemyState currentState = EnemyState.Patrol;

    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Setup Rigidbody2D
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        
        // Initialize
        currentHealth = maxHealth;
        patrolStartPos = transform.position;
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure Player has 'Player' tag.");
        }
        
        Debug.Log("Enemy1968 initialized!");
    }

    void Update()
    {
        if (isDead) return;
        
        // Update state based on player distance
        UpdateState();
        
        // Act based on current state
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
            case EnemyState.Attack:
                AttackPlayer();
                break;
        }
        
        // Update animator
        UpdateAnimator();
    }
    
    void FixedUpdate()
    {
        if (isDead || isHit) return;
        
        // Apply movement based on state
        if (currentState == EnemyState.Patrol || currentState == EnemyState.Chase)
        {
            // Movement is applied in respective methods
        }
        else if (currentState == EnemyState.Attack)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    private void UpdateState()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= 6f) // Close range - attack
            {
                currentState = EnemyState.Attack;
            }
            else // Medium range - chase
            {
                currentState = EnemyState.Chase;
            }
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }
    
    private void Patrol()
    {
        // Simple patrol: move left and right from start position
        float distanceFromStart = transform.position.x - patrolStartPos.x;
        
        if (distanceFromStart >= patrolRange)
        {
            movingRight = false;
        }
        else if (distanceFromStart <= -patrolRange)
        {
            movingRight = true;
        }
        
        // Move
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        rb.linearVelocity = direction * moveSpeed;
        
        // Flip sprite
        if (movingRight && !facingRight)
        {
            Flip();
        }
        else if (!movingRight && facingRight)
        {
            Flip();
        }
    }
    
    private void ChasePlayer()
    {
        if (player == null) return;
        
        // Move towards player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed * 1.2f, direction.y * moveSpeed * 1.2f);
        
        // Flip sprite to face player
        if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip();
        }
    }
    
    private void AttackPlayer()
    {
        if (player == null) return;
        
        // Stop moving
        rb.linearVelocity = Vector2.zero;
        
        // Face player
        Vector2 direction = (player.position - transform.position).normalized;
        if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip();
        }
        
        // Shoot
        if (Time.time - lastShootTime >= shootCooldown && !isAttacking)
        {
            Shoot();
        }
    }
    
    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }
        
        isAttacking = true;
        lastShootTime = Time.time;
        
        // Spawn bullet
        Vector2 shootDirection = (player.position - transform.position).normalized;
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = shootDirection * bulletSpeed;
        }
        
        // End attack animation after delay
        Invoke(nameof(EndAttack), 0.52f); // EnermyAttack animation is ~0.52 seconds
    }
    
    private void EndAttack()
    {
        isAttacking = false;
    }
    
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    private void UpdateAnimator()
    {
        if (animator == null) return;
        
        // Update movement animation
        bool isMoving = rb.linearVelocity.magnitude > 0.1f && !isAttacking && !isHit;
        animator.SetBool("isRunning", isMoving);
        
        // Update attack animation
        animator.SetBool("isAttacking", isAttacking);
        
        // Update hit animation
        animator.SetBool("isHitting", isHit);
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead || isHit) return;
        
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage! Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartHit();
        }
    }
    
    private void StartHit()
    {
        isHit = true;
        rb.linearVelocity = Vector2.zero;
        
        // End hit after animation duration
        Invoke(nameof(EndHit), 0.35f); // EnemyHit animation is ~0.35 seconds
    }
    
    private void EndHit()
    {
        isHit = false;
    }
    
    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        
        Debug.Log("Enemy died!");
        
        // Disable components
        if (rb != null) rb.simulated = false;
        enabled = false;
        
        // Destroy after delay
        Destroy(gameObject, 1f);
    }
    
    // Handle collision with player (direct contact damage)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            LinhGiaiPhong1968 player = collision.gameObject.GetComponent<LinhGiaiPhong1968>();
            if (player != null)
            {
                player.TakeDamage(1);
                Debug.Log("Enemy touched player - dealing damage!");
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        
        // If enemy has trigger collider, also handle collision here
        if (collision.CompareTag("Player"))
        {
            LinhGiaiPhong1968 player = collision.GetComponent<LinhGiaiPhong1968>();
            if (player != null)
            {
                player.TakeDamage(1);
                Debug.Log("Enemy trigger hit player - dealing damage!");
            }
        }
    }
    
    // Debug visualization
    private void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 6f);
        
        // Draw patrol range
        Gizmos.color = Color.blue;
        Vector2 startPos = Application.isPlaying ? patrolStartPos : (Vector2)transform.position;
        Gizmos.DrawLine(startPos + Vector2.left * patrolRange, startPos + Vector2.right * patrolRange);
    }
}
