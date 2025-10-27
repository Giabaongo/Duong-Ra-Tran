# Enemy Animator Controller Setup

## ✅ **Đã Hoàn Thành**

Enemy animator controller (`Enemy.controller`) đã được setup hoàn chỉnh với:

### 📊 **Parameters (3 Bool Parameters)**

| Parameter | Type | Mô Tả |
|-----------|------|-------|
| `isRunning` | Bool | Enemy đang chạy/di chuyển |
| `isAttacking` | Bool | Enemy đang tấn công |
| `isHitting` | Bool | Enemy bị đánh trúng |

### 🎭 **Animation States (4 States)**

#### 1. **EnemyIdle** (Default State)
- Default animation khi không làm gì
- Loop: ✅ Enabled (0.53s)
- **Transitions TO:**
  - → EnemyRun: `isRunning = true`
  - → EnermyAttack: `isAttacking = true`
  - → EnemyHit: `isHitting = true`

#### 2. **EnemyRun**
- Chạy/Di chuyển
- Loop: ✅ Enabled (0.43s)
- **Transitions TO:**
  - → EnemyIdle: `isRunning = false`
  - → EnermyAttack: `isAttacking = true`
  - → EnemyHit: `isHitting = true`

#### 3. **EnermyAttack**
- Animation tấn công
- Loop: ❌ Disabled (0.52s - play once)
- **Transitions TO:**
  - → EnemyRun: `isAttacking = false`
  - → EnemyIdle: `isAttacking = false` AND `isRunning = false`
  - → EnemyHit: `isHitting = true` (có thể bị interrupt)

#### 4. **EnemyHit**
- Bị đánh trúng
- Loop: ❌ Disabled (0.35s - play once)
- **Exit Time:** 0.7 (70% của animation phải chạy xong)
- **Transition Duration:** 0.1s (mượt mà)
- **Transitions TO:**
  - → EnemyIdle: sau khi play xong + không có action
  - → EnermyAttack: sau khi play xong + `isAttacking = true`
  - → EnemyRun: sau khi play xong + `isRunning = true`

## 🔄 **Transition Logic Flow**

```
┌──────────────┐
│  EnemyIdle   │ ← Default State
└──────┬───────┘
       │
       ├─→ isRunning=true ──→ EnemyRun
       ├─→ isAttacking=true ─→ EnermyAttack
       └─→ isHitting=true ───→ EnemyHit
                                    │
                      ┌─────────────┘
                      │ (Exit Time: 70%)
                      │
                      ├─→ All false ──→ EnemyIdle
                      ├─→ isAttacking ─→ EnermyAttack
                      └─→ isRunning ───→ EnemyRun
```

## 💡 **Cách Sử Dụng Trong Script**

### Ví dụ script Enemy1968.cs:

```csharp
using UnityEngine;

public class Enemy1968 : MonoBehaviour
{
    private Animator animator;
    private bool isMoving = false;
    private bool isAttacking = false;
    private bool isHit = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Update animator parameters
        animator.SetBool("isRunning", isMoving);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isHitting", isHit);
    }

    // Gọi khi enemy bắt đầu di chuyển
    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    // Gọi khi enemy tấn công
    public void Attack()
    {
        if (!isAttacking && !isHit)
        {
            isAttacking = true;
            Invoke(nameof(EndAttack), 0.52f); // Duration của attack animation
        }
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    // Gọi khi enemy bị đánh
    public void TakeDamage(int damage)
    {
        if (!isHit)
        {
            isHit = true;
            isAttacking = false; // Cancel attack
            Invoke(nameof(EndHit), 0.35f); // Duration của hit animation
        }
    }

    void EndHit()
    {
        isHit = false;
    }
}
```

## 🎯 **Ưu Điểm Của Setup Này**

1. ✅ **Hit Priority**: Khi bị đánh, enemy sẽ ngay lập tức chuyển sang hit animation
2. ✅ **No Loop on Attack/Hit**: Attack và hit animation chỉ play 1 lần
3. ✅ **Smooth Transitions**: Exit time 70% đảm bảo animation hit không bị cut đột ngột
4. ✅ **Flexible Return**: Sau khi hit xong có thể về idle, run hoặc attack tuỳ state
5. ✅ **No Weird Bugs**: Không có bug tự động attack như trước đây

## 📝 **Lưu Ý**

### Animation Duration:
- **EnemyIdle**: 0.53s (loop)
- **EnemyRun**: 0.43s (loop)
- **EnermyAttack**: 0.52s (no loop)
- **EnemyHit**: 0.35s (no loop)

### Trong code:
- Khi gọi `Attack()`, nhớ set timer `0.52f` để reset
- Khi gọi `TakeDamage()`, nhớ set timer `0.35f` để reset
- Hit animation có priority cao nhất (có thể interrupt attack)
- Attack không thể interrupt hit (phải đợi hit xong)

## 🚀 **Next Steps**

1. Tạo script `Enemy1968.cs` controller cho enemy
2. Thêm AI behavior (patrol, chase, attack player)
3. Setup collision detection với player
4. Implement damage system
5. Test trong Unity Scene

---

**Setup hoàn tất! Enemy animator sẵn sàng sử dụng! 🎮✨**

