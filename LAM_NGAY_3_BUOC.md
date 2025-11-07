# 🚀 LÀM NGAY 3 BƯỚC - 2 PHÚT

## Bước 1️⃣: Tạo Tag "PlayerBullet"
```
Edit → Project Settings → Tags and Layers
Click "+" → Nhập "PlayerBullet" → Save
```

## Bước 2️⃣: Kiểm Tra Có Enemy Trong Scene Không?
```
Mở Hierarchy → Tìm "Enemy" 
```

### ❌ KHÔNG CÓ?
```
Kéo Enemy prefab vào scene
HOẶC
Tạo GameObject mới:
- Add Component: Enermy1968Controller
- Add Component: BoxCollider2D (Is Trigger = false)
- Add Component: Rigidbody2D (Gravity = 0)
- Tag: Enemy
```

### ✅ CÓ RỒI?
```
Chọn Enemy → Inspector:
- Tag: Enemy ✅
- Collider2D: Is Trigger = FALSE ✅
- Script: Enermy1968Controller ✅
```

## Bước 3️⃣: Play Game Và Test
```
1. Play
2. Đi tới enemy
3. Bắn (Click/Space/Enter)
4. Xem Console
```

### ✅ THÀNH CÔNG:
```
Player bullet hit: Enemy with tag: Enemy
Player bullet hit enemy! Enemy taking damage!
Enemy took 1 damage! Health: 1/2
```

### ❌ KHÔNG THẤY ENEMY:
→ **Chưa có enemy trong scene!** Quay lại Bước 2️⃣

### ❌ LOG: "with tag: Untagged"
→ **Enemy chưa có tag!** Chọn Enemy → Tag → "Enemy"

---

## 🎯 Nhanh Nhất: Copy/Paste Commands

Nếu có sẵn Enemy prefab:
1. Hierarchy → Right click → Create Empty
2. Tên: "TestEnemy"
3. Copy component từ Enemy prefab sang

Nếu không:
1. Tạo Enemy GameObject như hướng dẫn Bước 2
2. Setup theo checklist

---

**Xong 3 bước này thì sẽ chạy! 🎮🔫**

