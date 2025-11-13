# 🔧 FIX: RESTART GAME VÀ BOMB KILL COUNT

## 🐛 VẤN ĐỀ ĐÃ PHÁT HIỆN:

### **1. Enemy không spawn lại khi restart game** ❌
**Nguyên nhân:**
- `EnemyHealth1968` có system **anti-respawn** với `static HashSet<string> deadEnemies`
- HashSet này là **STATIC** nên KHÔNG tự động xóa khi reload scene!
- Khi restart game bằng `SceneManager.LoadScene()`, scene mới load nhưng `deadEnemies` vẫn giữ danh sách enemy cũ
- → Tất cả enemy "mới" spawn ra bị nhận diện là "đã chết" → bị `Destroy()` ngay lập tức!

**Code lỗi trong `EnemyHealth1968.cs`:**
```csharp
// Line 26: Static HashSet không bị reset!
private static HashSet<string> deadEnemies = new HashSet<string>();

// Line 35-40: Kiểm tra enemy đã chết
if (deadEnemies.Contains(enemyID))
{
    Debug.LogWarning($"[EnemyHealth] ⚠️ {gameObject.name} đã chết trước đó! DESTROYING RESPAWN CLONE!");
    Destroy(gameObject);
    return;
}
```

---

### **2. Enemy kill count không cập nhật khi bị bomb giết** ❌
**Nguyên nhân:**
- Bomb B52 chỉ gây `damageToEnemies = 1` damage cho enemy (line 21 trong `B52Bomb.cs`)
- Enemy có `maxHealth = 20` HP (default trong `EnemyHealth1968`)
- → Cần **20 quả bomb** mới giết được 1 enemy!
- → Enemy không chết → không gọi `GameManager.EnemyKilled()` → kill count không tăng!

**Code lỗi trong `B52Bomb.cs`:**
```csharp
// Line 21: Damage quá nhỏ!
[SerializeField] private int damageToEnemies = 1;
```

---

## ✅ GIẢI PHÁP ĐÃ THỰC HIỆN:

### **Fix 1: Clear Dead Enemies khi Restart**

**File: `EnemyHealth1968.cs`**
```csharp
// ✅ ADDED: Public method để clear dead enemies
public static void ClearDeadEnemies()
{
    int count = deadEnemies.Count;
    deadEnemies.Clear();
    Debug.Log($"[EnemyHealth] 🔄 Cleared {count} dead enemy IDs for game restart!");
}
```

**File: `GameManager1968.cs`**
```csharp
public void RestartGame()
{
    Debug.Log("🔄 Restarting game...");
    
    // ★ CRITICAL FIX: Clear dead enemies tracking TRƯỚC KHI reload scene!
    EnemyHealth1968.ClearDeadEnemies();
    
    // Đặt lại time scale nếu đã dừng
    Time.timeScale = 1f;
    
    // Reload current scene
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    
    Debug.Log("✅ Scene reloaded! Enemies will spawn again!");
}
```

**→ Kết quả:** Khi restart game, tất cả enemy sẽ spawn lại bình thường! ✅

---

### **Fix 2: Tăng Bomb Damage cho Enemy**

**File: `B52Bomb.cs`**
```csharp
// ✅ CHANGED: Tăng damage từ 1 → 50 (đủ để giết 1 hit!)
[Tooltip("Damage cho cả enemies (friendly fire nếu trong bán kính) - ĐỦ ĐỂ GIẾT 1 HIT!")]
[SerializeField] private int damageToEnemies = 50;
```

**Logic hoạt động:**
1. Bomb B52 nổ → gọi `DamageTargetsInRadius()`
2. Tìm enemy trong bán kính → gọi `enemyHealth.TakeDamage(50)`
3. Enemy có 20 HP → nhận 50 damage → HP = -30 → chết!
4. `EnemyHealth.Die()` được gọi → `GameManager.EnemyKilled()` được gọi
5. Enemy kill count tăng lên! ✅
6. Khi kill đủ enemy → `Victory()` được gọi! 🎉

**→ Kết quả:** Bomb B52 giờ có thể giết enemy 1 hit và kill count cập nhật đúng! ✅

---

## 📋 TESTING CHECKLIST:

### **Test 1: Restart Game**
```
1. Play game
2. Giết 1-2 enemy
3. Pause game → chọn "Restart" / "Chơi lại"
4. ✅ CHECK: Tất cả enemy (6 con) phải xuất hiện lại!
5. ✅ CHECK: Enemy kill count reset về 0
```

**Logs mong đợi:**
```
[EnemyHealth] 🔄 Cleared 2 dead enemy IDs for game restart!
🔄 Restarting game...
✅ Scene reloaded! Enemies will spawn again!
[EnemyHealth] Enemy (1) initialized with MaxHP: 20, CurrentHP: 20
[EnemyHealth] Enemy (2) initialized with MaxHP: 20, CurrentHP: 20
... (6 enemies total)
```

---

### **Test 2: Bomb Kill Enemy**
```
1. Play game
2. Chờ B52 bay qua (sau 5s)
3. Đứng gần enemy để bomb B52 rơi trúng enemy
4. ✅ CHECK: Enemy bị giết chết (animation chết + biến mất)
5. ✅ CHECK: Enemy kill counter tăng lên (UI)
6. ✅ CHECK: Logs hiển thị enemy killed
```

**Logs mong đợi:**
```
[B52Bomb] 💥 Friendly fire! Enemy hit: Enemy (2), dealt 50 damage!
[EnemyHealth] 💥 Enemy (2) took 50 damage. HP: -30/20
[EnemyHealth] Enemy (2) HP reached 0, calling Die()...
💀💀💀 [EnemyHealth] Enemy (2) has been killed!
[GameManager1968] ⚔️ Enemy killed! Progress: 1/6
```

---

### **Test 3: Victory khi kill hết enemy**
```
1. Play game
2. Để B52 bomb giết hết 6 enemy (hoặc tự bắn)
3. ✅ CHECK: Victory UI hiện ra!
4. ✅ CHECK: Logs hiển thị victory
```

**Logs mong đợi:**
```
[GameManager1968] ⚔️ Enemy killed! Progress: 6/6
[GameManager1968] Victory condition met: 6 >= 6
🎉🎉🎉 === VICTORY! === 🎉🎉🎉
[GameManager1968] ✅ Victory UI 'VictoryPanel' is now VISIBLE
```

---

## 🎮 TÙY CHỈNH (OPTIONAL):

### **Điều chỉnh Bomb Damage trong Unity Inspector:**

Nếu muốn thay đổi mức độ damage của bomb:

**`B52_Bomb` Prefab:**
```
Assets/Prefabs/Effects/B52_Bomb.prefab
→ B52 Bomb (Script)
→ Damage To Enemies: 50 (có thể đổi thành 20, 100, v.v.)
→ Damage To Player: 5 (B52 target chính là player!)
→ Damage All: ✓ (bật friendly fire)
```

**Explosion Radius:**
```
→ Explosion Radius: 3 (bán kính nổ)
```

---

## 🔍 DEBUG TIPS:

### **Nếu enemy vẫn không spawn lại:**
1. Check Console logs khi restart:
   - Phải thấy: `[EnemyHealth] 🔄 Cleared X dead enemy IDs`
   - Phải thấy: `✅ Scene reloaded!`
2. Check scene có đủ 6 enemy trong Hierarchy không
3. Check `EnemyHealth1968` component trên mỗi enemy

### **Nếu kill count không tăng:**
1. Check bomb có trúng enemy không (xem Gizmos màu đỏ)
2. Check logs khi bomb nổ:
   - Phải thấy: `[B52Bomb] 💥 Friendly fire! Enemy hit`
   - Phải thấy: `[EnemyHealth] 💥 Enemy took 50 damage`
   - Phải thấy: `💀💀💀 [EnemyHealth] Enemy has been killed!`
3. Check `GameManager1968` có trong scene không

---

## ✅ SUMMARY:

| Vấn đề | Trạng thái | File đã sửa |
|--------|-----------|-------------|
| Enemy không spawn lại khi restart | ✅ FIXED | `EnemyHealth1968.cs`, `GameManager1968.cs` |
| Bomb kill count không cập nhật | ✅ FIXED | `B52Bomb.cs` |
| Victory khi kill hết enemy | ✅ WORKS | Logic đã có sẵn, chỉ cần fix kill count |

---

## 🎯 KẾT QUẢ CUỐI CÙNG:

✅ **Restart game → Enemy spawn lại đầy đủ!**  
✅ **Bomb B52 giết enemy → Kill count tăng lên!**  
✅ **Kill hết enemy → Victory screen hiện ra!**  

**→ GAME LOOP HOÀN THIỆN! 🎉**

