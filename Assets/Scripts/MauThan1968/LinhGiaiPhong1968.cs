using UnityEngine;
using UnityEngine.InputSystem;

public class LinhGiaiPhong1968 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackCooldown = 0.5f;
    
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Input")]
    private Vector2 moveInput;
    private bool facingRight = true;
    
    [Header("State")]
    private bool isAttacking = false;
    private bool isHit = false;
    private float lastAttackTime = 0f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 5; // Player dies after 5 hits
    private int currentHealth;

    void Start()
    {
        // Get required components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Setup Rigidbody2D for 2D top-down movement
        if (rb != null)
        {
            rb.gravityScale = 0f; // No gravity for 2D top-down
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on LinhGiaiPhong1968!");
        }

        // Check animator
        if (animator == null)
        {
            Debug.LogWarning("Animator not found on LinhGiaiPhong1968!");
        }

        // Initialize health
        currentHealth = maxHealth;

        Debug.Log("LinhGiaiPhong1968 initialized successfully!");
    }

    void Update()
    {
        // Get input from both old and new input system for compatibility
        GetMovementInput();

        // Handle attack input
        HandleAttackInput();

        // Update animator
        UpdateAnimator();

        // Flip sprite based on movement direction
        HandleSpriteFlip();
    }

    void FixedUpdate()
    {
        // Move the player
        MovePlayer();
    }

    private void GetMovementInput()
    {
        // Try to get input from old input system (WASD/Arrow keys)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;
    }

    // For new Input System (call this from PlayerInput component or Input Actions)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void MovePlayer()
    {
        if (rb == null) return;

        // Don't move if attacking or hit
        if (isAttacking || isHit)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Apply movement
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void HandleAttackInput()
    {
        // Check attack cooldown
        if (Time.time - lastAttackTime < attackCooldown) return;
        if (isAttacking || isHit) return;

        // Check attack input (Mouse click, Enter, or Space)
        bool attackPressed = Input.GetMouseButtonDown(0) || 
                            Input.GetKeyDown(KeyCode.Return) || 
                            Input.GetKeyDown(KeyCode.Space);

        if (attackPressed)
        {
            StartAttack();
        }
    }

    // For new Input System
    public void OnAttack(InputValue value)
    {
        if (value.isPressed && Time.time - lastAttackTime >= attackCooldown)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        
        // Shoot bullet
        ShootBullet();
        
        // End attack after animation duration (adjust based on your animation length)
        Invoke(nameof(EndAttack), 0.27f); // PlayerAttack animation is ~0.27 seconds
    }
    
    private void ShootBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Player bullet prefab not assigned!");
            return;
        }
        
        // Calculate shoot direction based on facing direction
        Vector2 shootDirection = facingRight ? Vector2.right : Vector2.left;
        
        // If fire point exists, use it, otherwise use player position
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        
        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"★ PLAYER BULLET spawned at {spawnPos}, scale: {bullet.transform.localScale}");
        
        // Set bullet direction using PlayerBullet script
        PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
        if (playerBullet != null)
        {
            playerBullet.SetDirection(shootDirection);
        }
        else
        {
            // Fallback: use Rigidbody2D if PlayerBullet script is not attached
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = shootDirection * bulletSpeed;
            }
        }
        
        Debug.Log($"Player shot bullet in direction: {shootDirection}");
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        // Update movement animation
        bool isMoving = moveInput.magnitude > 0.1f && !isAttacking && !isHit;
        animator.SetBool("isRunning", isMoving);

        // Update attack animation
        animator.SetBool("isAttacking", isAttacking);

        // Update hit animation
        animator.SetBool("isHitting", isHit);
    }

    private void HandleSpriteFlip()
    {
        // Only flip when moving (not during attack)
        if (moveInput.x > 0.1f && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < -0.1f && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Public method to take damage (called by enemies, bullets, etc.)
    public void TakeDamage(int damage)
    {
        if (isHit || !IsAlive()) return; // Already taking damage or dead

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"LinhGiaiPhong1968 took {damage} damage! Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
            // Trigger Game Over
            GameObject gmObject = GameObject.Find("GameManager");
            if (gmObject != null)
            {
                gmObject.SendMessage("GameOver", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            StartHit();
        }
    }

    private void StartHit()
    {
        isHit = true;
        isAttacking = false; // Cancel attack if hit

        // End hit after animation duration
        Invoke(nameof(EndHit), 0.53f); // PlayerHit animation is ~0.53 seconds
    }

    private void EndHit()
    {
        isHit = false;
    }

    private void Die()
    {
        Debug.Log("LinhGiaiPhong1968 died! GAME OVER!");
        
        // Disable movement
        rb.linearVelocity = Vector2.zero;
        
        // Disable controls
        enabled = false;
        
        // Optional: Play death animation or effect
        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);
            // You could add a death animation here if you have one
        }
    }

    // Heal method
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"LinhGiaiPhong1968 healed {amount}! Health: {currentHealth}/{maxHealth}");
    }

    // Getters
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsAlive() => currentHealth > 0;
    public bool IsFacingRight() => facingRight;

    // Debug info
    private void OnDrawGizmos()
    {
        // Draw velocity direction
        if (rb != null && Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)rb.linearVelocity);
        }
    }
}
