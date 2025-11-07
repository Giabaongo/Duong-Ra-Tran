using UnityEngine;

public class Enermy1968Controller : MonoBehaviour
{
    // ★ STATIC: Track initialized enemies across domain reloads
    private static System.Collections.Generic.HashSet<int> initializedEnemies = new System.Collections.Generic.HashSet<int>();
    
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
    [SerializeField] private float bulletSpeed = 6f; // ★ REDUCED: 6 (was 10) - Easier to dodge, -40% speed
    [SerializeField] private float initialAttackDelay = 1f; // ★ NEW: 1s delay before first shot after detecting player
    [SerializeField] private float maxBlockedTime = 2f; // ★ REDUCED: 2s (was 3s) - Give up faster
    [SerializeField] private float giveUpCooldown = 5f; // Cooldown after giving up before re-attacking
    private float lastShootTime = -999f; // ★ CHANGED: Start at -999 so first shot needs cooldown
    private float blockedTime = 0f; // Time enemy has been blocked by wall
    private float lastGiveUpTime = -999f; // Time when enemy last gave up
    private float firstAttackTime = -999f; // ★ NEW: Time when enemy first entered Attack state
    
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
        // ★ CRITICAL: Block ANY clone enemy immediately!
        if (gameObject.name.Contains("(Clone)"))
        {
            Debug.LogError($"[Enemy] 🚫🚫🚫 CLONE DETECTED: {gameObject.name}!");
            Debug.LogError($"[Enemy] → Position: {transform.position}");
            Debug.LogError($"[Enemy] → Clones are FORBIDDEN! Destroying immediately!");
            Destroy(gameObject);
            return;
        }
        
        // ★ SAFETY: Prevent re-initialization using STATIC tracking
        // Use GetInstanceID() to uniquely identify this GameObject
        int instanceID = gameObject.GetInstanceID();
        
        if (initializedEnemies.Contains(instanceID))
        {
            Debug.LogError($"[Enemy] ⚠️⚠️⚠️ {gameObject.name} (ID:{instanceID}) Start() called AGAIN!");
            Debug.LogError($"[Enemy] → Position: {transform.position}");
            Debug.LogError($"[Enemy] → This is a DUPLICATE enemy from domain reload or respawn!");
            Debug.LogError($"[Enemy] → DESTROYING immediately...");
            Destroy(gameObject);
            return;
        }
        
        // Mark this enemy as initialized
        initializedEnemies.Add(instanceID);
        Debug.Log($"[Enemy] ✅ {gameObject.name} (ID:{instanceID}) initialized at {transform.position}");
        
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
    }

    void Update()
    {
        if (isDead) return;
        
        // ★ NEW: If hit, freeze everything - no state update, no actions
        if (isHit)
        {
            rb.linearVelocity = Vector2.zero; // Stop moving
            UpdateAnimator(); // Only update animator
            return; // Don't do anything else
        }
        
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
        
        EnemyState previousState = currentState;
        
        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= 6f) // Close range - attack
            {
                // ★ FIX: Check if recently gave up
                // Don't immediately go back to Attack state after giving up
                float timeSinceGiveUp = Time.time - lastGiveUpTime;
                
                if (timeSinceGiveUp < giveUpCooldown)
                {
                    // Still on cooldown → Stay in Chase or Patrol
                    if (currentState == EnemyState.Attack)
                    {
                        // Was attacking → Go to Chase instead
                        currentState = EnemyState.Chase;
                        Debug.Log($"[Enemy] ⏳ {gameObject.name} on cooldown ({timeSinceGiveUp:F1}s/{giveUpCooldown}s), staying in Chase");
                    }
                    // Otherwise keep current state (Chase or Patrol)
                }
                else
                {
                    // Cooldown expired → Can attack again
                    currentState = EnemyState.Attack;
                }
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
        
        // ★ NEW: Record time when first entering Attack state
        if (previousState != EnemyState.Attack && currentState == EnemyState.Attack)
        {
            firstAttackTime = Time.time;
            Debug.Log($"[Enemy] 🎯 {gameObject.name} ENTERED Attack state - Initial delay: {initialAttackDelay}s");
        }
        
        // Reset blocked timer when leaving Attack state
        if (previousState == EnemyState.Attack && currentState != EnemyState.Attack)
        {
            blockedTime = 0f;
            firstAttackTime = -999f; // Reset first attack time
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
        
        // ★ NEW: Check initial attack delay
        float timeSinceEnterAttack = Time.time - firstAttackTime;
        if (timeSinceEnterAttack < initialAttackDelay)
        {
            // Still in initial delay period - don't shoot yet
            return;
        }
        
        // ★ CHECK LINE OF SIGHT
        bool hasLineOfSight = HasLineOfSight();
        
        if (hasLineOfSight)
        {
            // Clear line of sight → Reset blocked timer
            blockedTime = 0f;
            
            // Shoot if cooldown ready
            if (Time.time - lastShootTime >= shootCooldown && !isAttacking)
            {
                Shoot();
            }
        }
        else
        {
            // Line of sight blocked → Increase blocked timer
            blockedTime += Time.deltaTime;
            
            // If blocked for too long → Give up and return to patrol
            if (blockedTime >= maxBlockedTime)
            {
                Debug.Log($"[Enemy] 🔄 {gameObject.name} GIVING UP - Blocked for {blockedTime:F1}s, returning to Patrol!");
                lastGiveUpTime = Time.time; // ★ Record give up time
                currentState = EnemyState.Patrol;
                blockedTime = 0f; // Reset timer
                return;
            }
            
            // Still trying...
            if (Time.time - lastShootTime >= shootCooldown)
            {
                // Get what's blocking (for debug)
                Vector2 startPos = firePoint != null ? firePoint.position : transform.position;
                Vector2 directionToPlayer = (player.position - (Vector3)startPos).normalized;
                float distanceToPlayer = Vector2.Distance(startPos, player.position);
                int layerMask = ~((1 << 9) | (1 << 10));
                RaycastHit2D hit = Physics2D.Raycast(startPos, directionToPlayer, distanceToPlayer, layerMask);
                
                string blockingObject = hit.collider != null ? hit.collider.name : "Unknown";
                Debug.Log($"[Enemy] 🚫 {gameObject.name} CANNOT shoot - Blocked by {blockingObject} ({blockedTime:F1}s/{maxBlockedTime}s)");
                lastShootTime = Time.time; // Prevent spam logs
            }
        }
    }
    
    private void Shoot()
    {
        // ★ SAFETY CHECK: Don't shoot if dead
        if (isDead)
        {
            Debug.LogWarning($"[Enemy] ⚠️ {gameObject.name} tried to shoot while DEAD! Prevented!");
            return;
        }
        
        if (bulletPrefab == null)
        {
            Debug.LogError($"[Enemy] ❌ Bullet prefab NOT ASSIGNED on {gameObject.name}!");
            return;
        }
        
        isAttacking = true;
        lastShootTime = Time.time;
        
        // Spawn bullet
        Vector2 shootDirection = (player.position - transform.position).normalized;
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        
        Debug.Log($"[Enemy] 🔫 {gameObject.name} SHOOTING at player! Spawn pos: {spawnPos}, Direction: {shootDirection}");
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = shootDirection * bulletSpeed;
            Debug.Log($"[Enemy] ✅ Bullet spawned: {bullet.name}, Velocity: {bulletRb.linearVelocity}, Speed: {bulletSpeed}");
        }
        else
        {
            Debug.LogError($"[Enemy] ❌ Bullet has NO RIGIDBODY2D! Bullet won't move!");
        }
        
        // End attack animation after delay
        Invoke(nameof(EndAttack), 0.52f); // EnermyAttack animation is ~0.52 seconds
    }
    
    private void EndAttack()
    {
        isAttacking = false;
    }
    
    /// <summary>
    /// Check if enemy has clear line of sight to player (no walls blocking)
    /// </summary>
    private bool HasLineOfSight()
    {
        if (player == null) return false;
        
        Vector2 startPos = firePoint != null ? firePoint.position : transform.position;
        Vector2 directionToPlayer = (player.position - (Vector3)startPos).normalized;
        float distanceToPlayer = Vector2.Distance(startPos, player.position);
        
        // Raycast from enemy to player
        // Ignore layers: Enemy (9), PlayerBuller (10)
        int layerMask = ~((1 << 9) | (1 << 10)); // Ignore Enemy and PlayerBuller layers
        
        RaycastHit2D hit = Physics2D.Raycast(startPos, directionToPlayer, distanceToPlayer, layerMask);
        
        if (hit.collider != null)
        {
            // Check if hit player
            if (hit.collider.CompareTag("Player"))
            {
                // Clear line of sight!
                return true;
            }
            
            // Hit something else (wall, building, etc.)
            // ★ REMOVED LOG - Was spamming every frame (60 times/second!)
            // Log is already in AttackPlayer() with cooldown
            return false;
        }
        
        // No hit at all (shouldn't happen, but treat as clear)
        return true;
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
        
        // ★ NEW: Reset shooting timers when hit
        // This forces enemy to wait full cooldown + initial delay before next shot
        lastShootTime = Time.time; // Reset shoot cooldown
        firstAttackTime = Time.time; // Reset initial attack delay
        isAttacking = false; // Cancel any ongoing attack
        
        Debug.Log($"[Enemy] 💥 {gameObject.name} HIT - All timers reset, cannot shoot for {shootCooldown}s");
        
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
        
        Debug.Log($"[Enemy] 💀 {gameObject.name} (ID:{gameObject.GetInstanceID()}) died!");
        
        // ★ Remove from initialized tracking
        initializedEnemies.Remove(gameObject.GetInstanceID());
        
        // Disable components
        if (rb != null) rb.simulated = false;
        enabled = false;
        
        // Destroy after delay
        Destroy(gameObject, 1f);
    }
    
    void OnDestroy()
    {
        // ★ Cleanup: Remove from tracking when destroyed
        if (initializedEnemies != null)
        {
            initializedEnemies.Remove(gameObject.GetInstanceID());
        }
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
        
        // ★ NEW: Draw line of sight to player
        if (Application.isPlaying && player != null)
        {
            Vector2 firePos = firePoint != null ? firePoint.position : transform.position;
            Vector2 directionToPlayer = (player.position - (Vector3)firePos).normalized;
            float distanceToPlayer = Vector2.Distance(firePos, player.position);
            
            // Check if has line of sight
            int layerMask = ~((1 << 9) | (1 << 10)); // Ignore Enemy and PlayerBuller layers
            RaycastHit2D hit = Physics2D.Raycast(firePos, directionToPlayer, distanceToPlayer, layerMask);
            
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // Clear line of sight - draw green line
                Gizmos.color = Color.green;
            }
            else
            {
                // Line of sight blocked - draw red line
                Gizmos.color = Color.red;
            }
            
            Gizmos.DrawLine(firePos, player.position);
        }
    }
}
