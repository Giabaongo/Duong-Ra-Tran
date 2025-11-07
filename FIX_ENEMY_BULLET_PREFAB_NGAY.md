# 🔧 FIX ENEMY BULLET PREFAB - VẤN ĐỀ 2 SCRIPTS

## 🔴 VẤN ĐỀ

`Bullet_Enemy.prefab` có **2 SCRIPTS**:
1. ❌ `PlayerBullet1968` (disabled) - **SAI!**
2. ✅ `EnemyBullet` (enabled) - **ĐÚNG!**

Unity đang chạy **CẢ HAI** hoặc chạy **CÁI SAI** → Enemy bắn đạn player!

---

## ✅ FIX (3 CÁCH)

### **CÁCH 1: XÓA SCRIPT SAI (KHUYÊN DÙNG - 30 GIÂY)**

#### Trong Unity Editor:

```
1. Project → Prefabs → Bullet_Enemy (double-click)
   → Prefab mode mở ra

2. Inspector panel, tìm:
   - "Player Bullet 1968 (Script)" component
   - Có thể có checkbox tắt hoặc dòng chữ "Disabled"

3. Click vào ⚙ (gear icon) bên phải component
   → "Remove Component"

4. Kiểm tra CHỈ còn những component này:
   ✅ Transform
   ✅ Sprite Renderer
   ✅ Rigidbody 2D
   ✅ Circle Collider 2D
   ✅ Enemy Bullet (Script)  ← CHỈ CÓ CÁI NÀY!
   
   ❌ KHÔNG CÓ "Player Bullet 1968 (Script)"

5. File → Save (Ctrl+S)

6. Đóng Prefab mode (click mũi tên ← ở trên Hierarchy)
```

---

### **CÁCH 2: TẮT SCRIPT SAI (NẾU KHÔNG MUỐN XÓA)**

```
1. Project → Prefabs → Bullet_Enemy

2. Inspector → "Player Bullet 1968 (Script)"
   - Tìm checkbox ở đầu component
   - Đảm bảo checkbox = TẮT (unchecked)

3. Inspector → "Enemy Bullet (Script)"
   - Đảm bảo checkbox = BẬT (checked)

4. Save (Ctrl+S)
```

**LƯU Ý:** Cách này không tốt bằng cách 1 vì script vẫn tồn tại!

---

### **CÁCH 3: TẠO PREFAB MỚI (NẾU 2 CÁCH TRÊN KHÔNG WORK)**

#### Bước 1: Tạo GameObject mới

```
1. Hierarchy → Right-click → Create Empty
   - Đặt tên: "Bullet_Enemy_New"

2. Add components:
   - Add Component → Sprite Renderer
     * Sprite: (kéo sprite bullet vào)
     * Sorting Layer: Bullet (hoặc 3)
     * Order in Layer: 0
   
   - Add Component → Rigidbody 2D
     * Body Type: Dynamic
     * Gravity Scale: 0
     * Collision Detection: Continuous
   
   - Add Component → Circle Collider 2D
     * Is Trigger: ✅ BẬT
     * Radius: 0.1
   
   - Add Component → Enemy Bullet (Script)
     * Damage: 1
     * Life Time: 5

3. Transform:
   - Scale: (0.008, 0.008, 0.008)

4. Tag: EnemyBullet (hoặc Untagged)
5. Layer: 0 (Default)
```

#### Bước 2: Tạo Prefab

```
1. Kéo "Bullet_Enemy_New" từ Hierarchy vào Prefabs folder
   → Prefab mới được tạo

2. Xóa "Bullet_Enemy_New" khỏi Hierarchy

3. Đổi tên prefab: "Bullet_Enemy_New" → "Bullet_Enemy_Clean"
```

#### Bước 3: Assign vào Enemy

```
1. Hierarchy → Enemy (hoặc Enemy1968)

2. Inspector → Enermy1968 Controller (Script)
   - Bullet Prefab: Kéo "Bullet_Enemy_Clean" vào

3. Save Scene (Ctrl+S)
```

---

## 🔍 KIỂM TRA SAU KHI FIX

### **Test 1: Kiểm tra Prefab**

```
1. Project → Prefabs → Bullet_Enemy

2. Inspector:
   ✅ CHỈ có 1 script: "Enemy Bullet (Script)"
   ❌ KHÔNG có "Player Bullet 1968 (Script)"
   
   Circle Collider 2D:
   ✅ Is Trigger = BẬT (checked)
```

### **Test 2: Kiểm tra trong Game**

```
1. Clear Console
2. Play ▶
3. Đợi Enemy bắn

Logs phải hiển thị:
✅ ★★★ ENEMY BULLET HIT: Player, Layer: 6, Tag: 'Player'
✅ [EnemyBullet] ⚔️ Hit player, dealing damage!

KHÔNG được thấy:
❌ [PlayerBullet] ✅ Initialized...
❌ PlayerBullet1968:Awake ()
```

---

## 📋 CHECKLIST

```
[ ] Bullet_Enemy CHỈ có "EnemyBullet" script
[ ] Bullet_Enemy KHÔNG có "PlayerBullet1968" script
[ ] Circle Collider 2D: Is Trigger = BẬT
[ ] Tag = "EnemyBullet" hoặc "Untagged"
[ ] Layer = 0 (Default)
[ ] Enemy Controller → Bullet Prefab = Bullet_Enemy (đúng prefab)
[ ] Test: Enemy bắn → Console log "★★★ ENEMY BULLET HIT"
[ ] Test: Player bị mất máu khi enemy bắn
[ ] Test: KHÔNG thấy log "[PlayerBullet] Initialized"
```

---

## 💡 TẠI SAO CÓ 2 SCRIPTS?

Có thể do:
1. **Nhầm lẫn khi assign:** Ai đó đã add nhầm `PlayerBullet1968` vào `Bullet_Enemy`
2. **Copy-paste prefab:** Copy Bullet_Player → Bullet_Enemy nhưng quên xóa script cũ
3. **Merge conflict:** Git merge đã gộp 2 version khác nhau của prefab

**Giải pháp:** XÓA script thừa, CHỈ giữ lại `EnemyBullet`!

---

Bạn thử **CÁCH 1** trước (xóa script trong Unity Editor), rồi test lại nhé! 🎯

