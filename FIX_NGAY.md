# 🚨 FIX NGAY - Enemy Không Chết

## ⚡ Fix Nhanh Nhất (30 giây)

### 1️⃣ Kiểm Tra Tag Enemy
```
1. Chọn Enemy GameObject trong Hierarchy
2. Inspector → TAG (dropdown ở trên cùng)
3. Chọn "Enemy"
   - Nếu không có → Edit → Project Settings → Tags and Layers
   - Click "+" → Nhập "Enemy" → Save
4. Quay lại chọn Enemy → Tag → Enemy
```

### 2️⃣ Kiểm Tra Player Bullet Collider
```
1. Mở Player Bullet Prefab (trong folder Prefabs)
2. Tìm CircleCollider2D (hoặc BoxCollider2D)
3. ✅ Is Trigger = TRUE (BẮT BUỘC!)
```

### 3️⃣ Test Ngay
```
1. Play game
2. Bắn vào enemy
3. Xem Console log
```

## 📊 Xem Log Console

Sau khi bắn, bạn sẽ thấy:

### ✅ NẾU ĐÚNG:
```
Player bullet hit: Enemy with tag: Enemy
Player bullet hit enemy! Enemy taking damage!
Enemy took 1 damage! Health: 1/2
```

### ❌ NẾU SAI:
```
Player bullet hit: Enemy with tag: Untagged
```
→ **Enemy chưa có tag "Enemy"** → Làm bước 1️⃣

### ❌ NẾU KHÔNG CÓ LOG NÀO:
→ **Bullet không va chạm** → Kiểm tra:
1. Player Bullet prefab có Collider2D với Is Trigger = true?
2. Enemy có Collider2D?

## 🎯 Sau Khi Fix Xong

Khi enemy đã chết được, bạn có thể **tắt debug log** để console sạch hơn:

**Mở file:** `Assets/Scripts/MauThan1968/PlayerBullet.cs`

**Xóa dòng 38:**
```typescript
Debug.Log($"Player bullet hit: {collision.gameObject.name} with tag: {collision.tag}");
```

Hoặc thêm `//` phía trước để comment:
```typescript
// Debug.Log($"Player bullet hit: {collision.gameObject.name} with tag: {collision.tag}");
```

## 📸 Nếu Vẫn Không Fix Được

Gửi cho tôi screenshot của:
1. **Enemy Inspector** (toàn bộ)
2. **Player Bullet Prefab Inspector** (toàn bộ)
3. **Console logs** khi bắn

---

**File chi tiết: CHECK_ENEMY_DAMAGE.md**

