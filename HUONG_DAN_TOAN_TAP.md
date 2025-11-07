# 📚 HƯỚNG DẪN TOÀN TẬP - COLLISION & MAP SETUP

## 🎯 BẠN ĐANG GÆP VẤN ĐỀ GÌ?

### ❓ "Player và Enemy đi xuyên qua tòa nhà"
→ **ĐỌC:** `QUICK_FIX_COLLISION.md` (3 phút fix xong)

### ❓ "Tôi muốn hướng dẫn chi tiết từng bước"
→ **ĐỌC:** `SETUP_MAP_COLLISION.md` (hướng dẫn đầy đủ)

### ❓ "Làm sao để test/debug collision?"
→ **ĐỌC:** `CACH_DUNG_DEBUG_SCRIPT.md` (visualize colliders)

---

## 📖 CÁC FILE HƯỚNG DẪN

### 1. ⚡ QUICK_FIX_COLLISION.md
**Cho ai:** Người muốn fix nhanh trong 3 phút  
**Nội dung:**
- 3 bước đơn giản
- Không giải thích nhiều
- Làm là xong

**Khi nào đọc:** Khi bạn đã hiểu cơ bản, chỉ cần checklist nhanh

---

### 2. 📋 SETUP_MAP_COLLISION.md
**Cho ai:** Người muốn hiểu rõ từng bước  
**Nội dung:**
- Hướng dẫn chi tiết từng bước
- Giải thích tại sao phải làm như vậy
- Troubleshooting đầy đủ
- Checklist hoàn chỉnh

**Khi nào đọc:** 
- Lần đầu setup
- Khi QUICK_FIX không hoạt động
- Muốn hiểu sâu hơn

---

### 3. 🔍 CACH_DUNG_DEBUG_SCRIPT.md
**Cho ai:** Người muốn visualize collision boundaries  
**Nội dung:**
- Cách dùng script `CollisionDebugger.cs`
- Nhìn thấy colliders trong game
- Debug thông tin component

**Khi nào đọc:**
- Sau khi setup xong, muốn test
- Khi vẫn có vấn đề, cần debug
- Muốn chụp screenshot gửi lỗi

---

## 🚀 LỘ TRÌNH KHUYÊN DÙNG

### Lần đầu tiên setup:
```
1. Đọc: SETUP_MAP_COLLISION.md (hiểu cơ bản)
2. Làm theo: QUICK_FIX_COLLISION.md (3 phút)
3. Test: Chạy game xem có hoạt động không
4. Nếu OK → XONG! 🎉
5. Nếu lỗi → Bước 6
6. Setup: CACH_DUNG_DEBUG_SCRIPT.md
7. Chụp screenshot gửi để debug
```

### Đã biết, chỉ cần làm lại:
```
1. Đọc: QUICK_FIX_COLLISION.md
2. Làm xong! 🎉
```

### Gặp lỗi lạ:
```
1. Setup: CollisionDebugger (theo CACH_DUNG_DEBUG_SCRIPT.md)
2. Chạy game
3. Xem thông tin debug góc trên trái
4. So sánh với checklist trong SETUP_MAP_COLLISION.md
5. Fix theo hướng dẫn Troubleshooting
```

---

## 🎓 KIẾN THỨC NỀN TẢNG

### Collision trong Unity 2D hoạt động như thế nào?

#### 1. Rigidbody2D (Body Type)
```
Dynamic: Di chuyển được, chịu ảnh hưởng vật lý
Kinematic: Di chuyển được, KHÔNG chịu ảnh hưởng vật lý
Static: KHÔNG di chuyển, dùng cho tường/sàn
```

#### 2. Collider2D (Is Trigger)
```
Is Trigger = FALSE: Va chạm vật lý (không đi xuyên)
Is Trigger = TRUE: Trigger event (đi xuyên nhưng detect)
```

#### 3. Layer Collision Matrix
```
Quy định layer nào va chạm với layer nào
Ví dụ: Player layer có va chạm với Obstacles layer
```

---

## 🗂️ CẤU TRÚC MAP KHUYÊN DÙNG

```
Grid
│
├─ Ground (Tilemap)
│  ├─ Đường đi
│  ├─ Sorting Layer: Ground
│  └─ ❌ KHÔNG có Collider
│
├─ Building (Tilemap)
│  ├─ Tòa nhà, tường
│  ├─ Sorting Layer: Building
│  └─ ✅ CÓ TilemapCollider2D + CompositeCollider2D
│
└─ Foreground (Tilemap) - tùy chọn
   ├─ Cây, bụi cỏ
   ├─ Sorting Layer: Foreground
   └─ ❌ KHÔNG có Collider (hoặc có nếu muốn chặn)
```

---

## 👤 SETUP PLAYER/ENEMY CHUẨN

```
Player/Enemy GameObject:
│
├─ Transform
├─ SpriteRenderer (Sorting Layer: Player/Enemy)
├─ Animator
│
├─ Rigidbody2D:
│  ├─ Body Type: Dynamic ✓
│  ├─ Gravity Scale: 0 ✓
│  ├─ Constraints: Freeze Rotation Z ✓
│  └─ Collision Detection: Continuous ✓
│
├─ BoxCollider2D (hoặc CapsuleCollider2D):
│  └─ Is Trigger: FALSE ✓
│
└─ Script (LinhGiaiPhong1968 / Enermy1968Controller)
```

---

## 🔗 SCRIPTS LIÊN QUAN

### 1. LinhGiaiPhong1968.cs
- Player controller
- Movement, shooting, health
- Location: `Assets/Scripts/MauThan1968/`

### 2. Enermy1968Controller.cs
- Enemy AI controller
- Patrol, chase, attack
- Location: `Assets/Scripts/MauThan1968/`

### 3. CollisionDebugger.cs (MỚI)
- Debug collision boundaries
- Visualize colliders
- Location: `Assets/Scripts/MauThan1968/`

---

## 📊 CHECKLIST TOÀN BỘ

### Building Tilemap
```
[ ] Có TilemapCollider2D
[ ] Có CompositeCollider2D
[ ] Có Rigidbody2D (Body Type: Static)
[ ] TilemapCollider2D: "Used By Composite" = ✓
[ ] CompositeCollider2D: Geometry Type = Polygons
[ ] Sorting Layer: Building (hoặc layer cao hơn Ground)
```

### Ground Tilemap
```
[ ] KHÔNG có TilemapCollider2D
[ ] Sorting Layer: Ground
```

### Player
```
[ ] Rigidbody2D: Body Type = Dynamic, Gravity = 0
[ ] BoxCollider2D: Is Trigger = FALSE
[ ] Tag: Player
[ ] Layer: Player (hoặc Default)
[ ] Script: LinhGiaiPhong1968
```

### Enemy
```
[ ] Rigidbody2D: Body Type = Dynamic, Gravity = 0
[ ] BoxCollider2D: Is Trigger = FALSE
[ ] Tag: Enemy
[ ] Layer: Enemy (hoặc Default)
[ ] Script: Enermy1968Controller
```

### Test
```
[ ] Player bị chặn bởi Building
[ ] Enemy bị chặn bởi Building
[ ] Player di chuyển trên Ground
[ ] Enemy patrol trên Ground
[ ] Không có lỗi trong Console
```

---

## 🆘 KHI GẶP VẤN ĐỀ

### 1. Player vẫn đi xuyên tường
```
→ Kiểm tra: Building có TilemapCollider2D + Rigidbody2D (Static)?
→ Kiểm tra: Player có Rigidbody2D + BoxCollider2D?
→ Kiểm tra: Collider Is Trigger = FALSE?
→ Dùng: CollisionDebugger để visualize
```

### 2. Player bị giật/lag khi di chuyển
```
→ Rigidbody2D: Interpolate = Interpolate
→ Rigidbody2D: Collision Detection = Continuous
→ CompositeCollider2D: Geometry Type = Polygons
```

### 3. Enemy không patrol
```
→ Kiểm tra: Enemy có Rigidbody2D?
→ Kiểm tra: Enemy script có lỗi trong Console?
→ Adjust: patrol range trong Inspector
```

### 4. Collider không khớp hình sprite
```
→ Edit: Tile Palette → Tile → Collider Type = Sprite
→ Hoặc: Edit Collider trong Scene View
```

---

## 💬 LIÊN HỆ/HỖ TRỢ

Nếu làm theo hướng dẫn mà vẫn không được:

1. **Setup CollisionDebugger** theo `CACH_DUNG_DEBUG_SCRIPT.md`
2. **Chạy game** và chụp screenshot thông tin debug
3. **Chụp Console** nếu có lỗi
4. **Chụp Inspector** của Player, Enemy, Building
5. **Gửi cho tôi** để debug

---

## 📅 LỊCH SỬ CẬP NHẬT

- **2024-10-28**: Tạo hướng dẫn toàn tập
  - QUICK_FIX_COLLISION.md
  - SETUP_MAP_COLLISION.md
  - CACH_DUNG_DEBUG_SCRIPT.md
  - CollisionDebugger.cs

---

## 🎉 KẾT LUẬN

Bạn có đủ công cụ và hướng dẫn để setup collision cho map rồi!

**Bắt đầu từ:** `QUICK_FIX_COLLISION.md`  
**Nếu cần thêm:** `SETUP_MAP_COLLISION.md`  
**Debug bằng:** `CACH_DUNG_DEBUG_SCRIPT.md`

Chúc may mắn! 🚀🎮

