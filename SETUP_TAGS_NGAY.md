# 🏷️ SETUP TAGS - BẮT BUỘC!

## ⚠️ Vấn Đề Vừa Fix

Code đã được sửa để:
1. ✅ Đạn player không bắn trúng player
2. ✅ Đạn player không va chạm với đạn player khác
3. ✅ Đạn player chỉ gây damage cho enemy

## 📋 PHẢI LÀM NGAY (3 phút)

### Bước 1: Tạo Tag "PlayerBullet"

1. **Unity Top Menu:** Edit → Project Settings
2. **Chọn:** Tags and Layers (bên trái)
3. **Mở rộng:** Tags
4. **Click:** nút "+" 
5. **Nhập:** `PlayerBullet`
6. **Click:** nút "+" lần nữa
7. **Nhập:** `Enemy` (nếu chưa có)
8. **Save và đóng**

### Bước 2: Kiểm Tra Enemy Trong Scene

**QUAN TRỌNG:** Bạn có enemy trong scene không?

1. **Mở Hierarchy**
2. **Tìm:** GameObject có tên chứa "Enemy" hoặc "Enermy"
3. **Nếu KHÔNG có enemy:**
   - Kéo Enemy prefab từ Prefabs folder vào scene
   - Hoặc tạo Enemy GameObject mới

### Bước 3: Setup Enemy GameObject

**Chọn Enemy trong Hierarchy, kiểm tra:**

```
✅ Inspector → Tag: Enemy
✅ Inspector → Layer: Enemy (hoặc Default)
✅ Components:
   - Rigidbody2D (Gravity Scale = 0)
   - Collider2D (BoxCollider2D)
     ├── Is Trigger: FALSE ❌
   - Animator
   - SpriteRenderer  
   - Enermy1968Controller (script)
     ├── Max Health: 2
     ├── Bullet Prefab: Bullet_Enemy
     └── Fire Point: [child object]
```

**Nếu thiếu script:** Add Component → `Enermy1968Controller`

**Nếu thiếu Bullet Prefab:** Kéo `Bullet_Enemy` từ Prefabs folder vào slot

**Nếu thiếu Fire Point:** 
- Right click Enemy → Create Empty → Tên "FirePoint"
- Position: X = 0.5, Y = 0
- Kéo FirePoint vào slot Fire Point

### Bước 4: Kiểm Tra Player

**Chọn Player trong Hierarchy:**

```
✅ Tag: Player
✅ Components:
   - Rigidbody2D
   - Collider2D (Is Trigger: FALSE)
   - Animator
   - LinhGiaiPhong1968
     ├── Bullet Prefab: [Player bullet prefab]
     └── Fire Point: [child object]
```

## 🎮 Test Ngay

1. **Play game**
2. **Chờ enemy xuất hiện** (nếu dùng spawner) hoặc **đi tới enemy**
3. **Bắn vào enemy**
4. **Xem Console log**

### ✅ KẾT QUẢ MONG ĐỢI:

```
Player shot bullet in direction: (1.00, 0.00)
Player bullet hit: Enemy with tag: Enemy
Player bullet hit enemy! Enemy taking damage!
Enemy took 1 damage! Health: 1/2
```

Bắn lần 2:
```
Player bullet hit: Enemy with tag: Enemy
Player bullet hit enemy! Enemy taking damage!
Enemy took 1 damage! Health: 0/2
Enemy died!
```

### ❌ NẾU VẪN KHÔNG THẤY LOG VỀ ENEMY:

Có nghĩa là **KHÔNG CÓ ENEMY TRONG SCENE**!

**Fix:**
1. Tìm Enemy prefab trong Project → Prefabs folder
2. Kéo vào scene
3. Hoặc tạo GameObject mới và setup như bước 3

## 🔧 Collision Matrix (Tùy Chọn)

Nếu muốn setup layers đúng chuẩn:

**Edit → Project Settings → Physics 2D**

**Layers:**
- Player: Layer 6
- Enemy: Layer 7  
- PlayerBullet: Layer 8
- EnemyBullet: Layer 9

**Collision Matrix - BỎ TICK:**
- ❌ Player ↔ PlayerBullet
- ❌ Enemy ↔ EnemyBullet
- ❌ PlayerBullet ↔ PlayerBullet
- ❌ EnemyBullet ↔ EnemyBullet

**Collision Matrix - GIỮ TICK:**
- ✅ Player ↔ Enemy
- ✅ Player ↔ EnemyBullet
- ✅ Enemy ↔ PlayerBullet

## ⚡ Quick Checklist

Trước khi test:
- [ ] Tag "PlayerBullet" đã được tạo
- [ ] Tag "Enemy" đã được tạo
- [ ] Enemy có trong scene (nhìn thấy trong Hierarchy)
- [ ] Enemy có tag "Enemy"
- [ ] Enemy có Collider2D (Is Trigger = false)
- [ ] Enemy có script Enermy1968Controller
- [ ] Player có Collider2D (Is Trigger = false)

## 🎯 Tắt Debug Logs (Sau Khi Hoạt Động)

Khi mọi thứ đã chạy tốt, tắt debug logs để console sạch:

**File:** `Assets/Scripts/MauThan1968/PlayerBullet.cs`

Xóa hoặc comment dòng 59:
```typescript
// Debug.Log($"Player bullet hit: {collision.gameObject.name} with tag: {collision.tag}");
```

Và dòng 69:
```typescript
// Debug.Log("Player bullet hit enemy! Enemy taking damage!");
```

---

**Làm xong chạy lại game nhé! 🎮**

