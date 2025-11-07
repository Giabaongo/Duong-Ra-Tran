using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class LinhGiaiPhong1968 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackCooldown = 0.5f;
    
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    
    [Header("Auto-Aim Settings")]
    [SerializeField] private bool enableAutoAim = true; // ★ NEW: Bật/tắt auto-aim
    [SerializeField] private float autoAimRange = 10f; // ★ NEW: Tầm tự động aim (10 units)
    [SerializeField] private LayerMask enemyLayer; // ★ NEW: Layer của enemy để detect

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Input")]
    private Vector2 moveInput;
    private bool facingRight = true;
    private Vector2 lastMoveDirection = Vector2.right; // ★ NEW: Track last move direction for shooting
    
    [Header("State")]
    private bool isAttacking = false;
    private bool isHit = false;
    private float lastAttackTime = 0f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 20; 
    private int currentHealth;
    
    [Header("Events")]
    public UnityEvent<int, int> OnHealthChanged; // (currentHealth, maxHealth)

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
        
        // Trigger initial health update
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // ★ WARN: Check auto-aim setup
        if (enableAutoAim && enemyLayer.value == 0)
        {
            Debug.LogWarning("[Player] ⚠️ Auto-Aim enabled but Enemy Layer not set! Will auto-detect...");
        }

        Debug.Log($"[Player] ✅ LinhGiaiPhong1968 initialized! Auto-Aim: {enableAutoAim}, Range: {autoAimRange}");
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
        
        // ★ NEW: Track last move direction for shooting
        if (moveInput.magnitude > 0.1f)
        {
            lastMoveDirection = moveInput;
        }
    }

    // For new Input System (call this from PlayerInput component or Input Actions)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        
        // ★ NEW: Track last move direction for shooting
        if (moveInput.magnitude > 0.1f)
        {
            lastMoveDirection = moveInput;
        }
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
        
        // ★ NEW: Auto-aim to nearest enemy if enabled and enemy in range
        Vector2 shootDirection;
        Transform targetEnemy = null;
        
        if (enableAutoAim)
        {
            targetEnemy = FindNearestEnemy();
        }
        
        if (targetEnemy != null)
        {
            // Có enemy trong tầm → Bắn vào enemy
            shootDirection = (targetEnemy.position - transform.position).normalized;
            Debug.Log($"[Player] 🎯 AUTO-AIM: Targeting {targetEnemy.name} at {targetEnemy.position}");
        }
        else
        {
            // Không có enemy → Bắn theo hướng di chuyển
            shootDirection = lastMoveDirection.normalized;
            Debug.Log($"[Player] ➡️ Manual aim: Direction {shootDirection}");
        }
        
        // If fire point exists, use it, otherwise use player position with offset
        Vector3 spawnPos;
        if (firePoint != null)
        {
            spawnPos = firePoint.position;
        }
        else
        {
            // Spawn bullet 0.5 units away from player to avoid collision
            spawnPos = transform.position + (Vector3)shootDirection * 0.5f;
        }
        
        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"★ PLAYER BULLET spawned at {spawnPos}, scale: {bullet.transform.localScale}");
        
        // ★ NEW: Calculate rotation angle for bullet sprite
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        // Set bullet direction using PlayerBullet1968 script
        PlayerBullet1968 playerBullet = bullet.GetComponent<PlayerBullet1968>();
        if (playerBullet != null)
        {
            // ★ NEW: Set custom direction instead of using default
            playerBullet.SetDirection(shootDirection);
            Debug.Log($"[LinhGiaiPhong1968] PlayerBullet1968 found, direction set to {shootDirection}");
        }
        else
        {
            Debug.LogWarning("[LinhGiaiPhong1968] PlayerBullet1968 component NOT FOUND on bullet!");
            // Fallback: use Rigidbody2D if PlayerBullet1968 script is not attached
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = shootDirection * bulletSpeed;
            }
        }
        
        Debug.Log($"Player shot bullet in direction: {shootDirection} (angle: {angle}°)");
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
        Debug.Log($"[Player] 🎯 TakeDamage called! Damage: {damage}, isHit: {isHit}, IsAlive: {IsAlive()}");
        
        if (isHit || !IsAlive())
        {
            Debug.LogWarning($"[Player] ⚠️ Cannot take damage - isHit: {isHit}, IsAlive: {IsAlive()}");
            return; // Already taking damage or dead
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"[Player] ⚔️ Player took {damage} damage! Health: {currentHealth}/{maxHealth}");
        
        // Trigger health changed event - CẬP NHẬT NGAY LẬP TỨC
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

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
            Debug.Log($"[Player] 💥 Starting HIT animation!");
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

    /// <summary>
    /// ★ NEW: Tìm enemy gần nhất trong tầm auto-aim
    /// </summary>
    private Transform FindNearestEnemy()
    {
        // ★ FIX: Auto-detect enemy layer nếu chưa setup
        LayerMask effectiveEnemyLayer = enemyLayer;
        if (enemyLayer.value == 0)
        {
            // Fallback: Tìm layer "Enemy" hoặc tìm tất cả colliders và filter theo tag
            int enemyLayerIndex = LayerMask.NameToLayer("Enemy");
            if (enemyLayerIndex != -1)
            {
                effectiveEnemyLayer = 1 << enemyLayerIndex;
                Debug.LogWarning($"[Player] ⚠️ Enemy Layer chưa setup! Auto-detected layer: {enemyLayerIndex}");
            }
            else
            {
                // Last resort: Search all layers
                effectiveEnemyLayer = ~0; // All layers
                Debug.LogWarning("[Player] ⚠️ Enemy layer not found! Searching all layers...");
            }
        }
        
        // Find all enemies in range using OverlapCircle
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, autoAimRange, effectiveEnemyLayer);
        
        Debug.Log($"[Player] 🔍 Scanning for enemies... Found {enemiesInRange.Length} colliders in range {autoAimRange}");
        
        if (enemiesInRange.Length == 0)
        {
            return null; // Không có enemy nào trong tầm
        }
        
        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;
        
        int validEnemies = 0;
        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            // ★ FILTER: Chỉ aim vào object có tag "Enemy"
            if (!enemyCollider.CompareTag("Enemy"))
            {
                Debug.Log($"[Player] ⚪ Skipping {enemyCollider.name} - Not tagged as Enemy (tag: {enemyCollider.tag})");
                continue; // Skip non-enemy objects
            }
            
            // Check xem enemy có còn sống không
            EnemyHealth1968 enemyHealth = enemyCollider.GetComponent<EnemyHealth1968>();
            if (enemyHealth != null && enemyHealth.IsDead())
            {
                Debug.Log($"[Player] 💀 Skipping {enemyCollider.name} - Already dead");
                continue; // Skip enemy đã chết
            }
            
            // Tính khoảng cách
            float distance = Vector2.Distance(transform.position, enemyCollider.transform.position);
            validEnemies++;
            
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemyCollider.transform;
            }
        }
        
        if (nearestEnemy != null)
        {
            Debug.Log($"[Player] 🎯 LOCKED ON TARGET: {nearestEnemy.name} at distance {nearestDistance:F2} (Valid enemies: {validEnemies})");
        }
        else
        {
            Debug.Log($"[Player] ❌ No valid enemy found (Scanned: {enemiesInRange.Length}, Valid: {validEnemies})");
        }
        
        return nearestEnemy;
    }
    
    // Heal method
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"LinhGiaiPhong1968 healed {amount}! Health: {currentHealth}/{maxHealth}");
        
        // Trigger health changed event
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
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
    
    private void OnDrawGizmosSelected()
    {
        // ★ NEW: Draw auto-aim range
        if (enableAutoAim)
        {
            Gizmos.color = new Color(1f, 1f, 0f, 0.3f); // Yellow transparent
            Gizmos.DrawWireSphere(transform.position, autoAimRange);
            
            // Draw line to nearest enemy if in play mode
            if (Application.isPlaying)
            {
                Transform nearestEnemy = FindNearestEnemy();
                if (nearestEnemy != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, nearestEnemy.position);
                }
            }
        }
    }
}
