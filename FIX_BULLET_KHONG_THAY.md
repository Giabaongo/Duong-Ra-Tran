# 🎨 FIX Đạn Player Không Thấy

## 🔍 VẤN ĐỀ

Đạn enemy thấy được, đạn player KHÔNG thấy → **Sorting Layer sai!**

**So sánh:**
- ✅ Bullet_Enemy: Sorting Layer = 2 (layer cao, hiện trên)
- ❌ Bullet_Player: Sorting Layer = 0 (Default, bị che)

## ✅ FIX TRONG UNITY (2 PHÚT)

### Cách 1: Copy Sorting Layer từ Enemy Bullet (NHANH NHẤT)

1. **Mở Project → Prefabs**
2. **Click Bullet_Enemy prefab**
3. **Inspector → Sprite Renderer:**
   - Xem "Sorting Layer" đang là gì (ví dụ: "Projectiles" hoặc "Foreground")
   - Xem "Order in Layer" là bao nhiêu
4. **Click Bullet_Player prefab**
5. **Inspector → Sprite Renderer:**
   - **Sorting Layer:** Chọn GIỐNG Bullet_Enemy
   - **Order in Layer:** Set GIỐNG Bullet_Enemy
6. **Save** (Ctrl+S)

### Cách 2: Set Sorting Layer Mới

1. **Project → Prefabs → Bullet_Player**
2. **Inspector → Sprite Renderer**
3. **Sorting Layer → Add Sorting Layer...**
4. **Tạo layer mới:**
   - Tên: `Projectiles` hoặc `Bullets`
   - Kéo lên trên để có order cao
5. **Quay lại Bullet_Player**
6. **Sorting Layer → Chọn "Projectiles"**
7. **Order in Layer:** 0 hoặc 1
8. **Save**

### Cách 3: Tăng Order in Layer (ĐƠN GIẢN NHẤT)

1. **Project → Prefabs → Bullet_Player**
2. **Inspector → Sprite Renderer**
3. **Order in Layer:** Đổi từ 0 → **10** (hoặc số cao)
4. **Save**

## 🎮 TEST NGAY

1. **Play game**
2. **Bắn**
3. **Xem có thấy đạn bay không?**

### ✅ NẾU THẤY:
→ **Fix xong!** Giờ có thể giảm Order in Layer xuống nếu cần

### ❌ NẾU VẪN KHÔNG THẤY:

Kiểm tra thêm:

#### A. Scale quá nhỏ?
```
Bullet_Player prefab → Transform
Scale: x: 0.01, y: 0.01, z: 0.01
→ Rất nhỏ! Thử tăng lên: x: 0.05, y: 0.05, z: 0.05
```

#### B. Sprite không có?
```
Bullet_Player prefab → Sprite Renderer
Sprite: [Phải có hình]
Nếu None → Gán sprite từ Assets/Sprites
```

#### C. Color = Transparent?
```
Bullet_Player prefab → Sprite Renderer
Color: Phải là trắng (R:255, G:255, B:255, A:255)
Nếu A = 0 → Invisible!
```

## 📊 DEBUG THÊM

Thêm log để xem đạn có spawn không:

Trong `LinhGiaiPhong1968.cs`, sau dòng spawn bullet:
```csharp
GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

// Thêm dòng này:
Debug.Log($"★ BULLET SPAWNED at {spawnPos}, scale: {bullet.transform.localScale}");
```

Chạy game và xem:
- Position có đúng không?
- Scale có quá nhỏ không?

## 🎯 GIẢI PHÁP NHANH NHẤT

**Trong Unity:**

```
1. Bullet_Player prefab
2. Sprite Renderer → Order in Layer = 10
3. Save
4. Test
```

Nếu vẫn không thấy:
```
1. Bullet_Player prefab
2. Transform → Scale: x: 0.05, y: 0.05, z: 0.05
3. Save
4. Test
```

---

**Làm theo Cách 3 trước (đơn giản nhất), 99% sẽ fix được! 🎨**

