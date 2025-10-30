# 🗺️ HƯỚNG DẪN SETUP COLLISION CHO MAP

## ⚠️ VẤN ĐỀ HIỆN TẠI
- **Ground tilemap**: Đường đi (không có collider) ✓
- **Building tilemap**: Tòa nhà (CHƯA có collider) ❌
- **Player**: Đi xuyên qua tòa nhà
- **Enemy**: Đi xuyên qua tòa nhà

---

## 🎯 MỤC TIÊU
- Player và Enemy **CHỈ đi trên Ground** (đường)
- Player và Enemy **BỊ CHẶN bởi Building** (tòa nhà)
- Tối ưu performance với Composite Collider

---

## 🔧 BƯỚC 1: THÊM COLLIDER CHO BUILDING TILEMAP

### 1.1. Chọn Building Tilemap
```
Hierarchy → Grid → Building
```

### 1.2. Thêm TilemapCollider2D
```
1. Inspector → Add Component
2. Gõ: "Tilemap Collider 2D"
3. Enter để thêm

✅ Bạn sẽ thấy:
   - TilemapCollider2D component xuất hiện
   - Trong Scene View, các tòa nhà có viền xanh lá (collider)
```

### 1.3. Thêm CompositeCollider2D (Tối ưu performance)
```
1. Inspector → Add Component
2. Gõ: "Composite Collider 2D"
3. Enter

⚠️ Unity sẽ TỰ ĐỘNG thêm Rigidbody2D (đừng xóa!)

✅ Cấu hình TilemapCollider2D:
   - ✓ Check "Used By Composite"

✅ Cấu hình CompositeCollider2D:
   - Geometry Type: Polygons (tối ưu hơn)
   - Generation Type: Synchronous

✅ Cấu hình Rigidbody2D (tự động thêm):
   - Body Type: Static (QUAN TRỌNG!)
   - Simulated: ✓ (checked)
```

### 1.4. Kết quả
```
Building GameObject:
├─ Transform
├─ Tilemap
├─ Tilemap Renderer
├─ TilemapCollider2D ✓ (MỚI)
├─ CompositeCollider2D ✓ (MỚI)
└─ Rigidbody2D (Body Type: Static) ✓ (TỰ ĐỘNG)
```

---

## 🔧 BƯỚC 2: KIỂM TRA PLAYER COMPONENTS

### 2.1. Chọn Player GameObject
```
Hierarchy → Player
```

### 2.2. Kiểm tra Components
```
✅ Player phải có:
   - Transform
   - SpriteRenderer (Sorting Layer: Player hoặc Default)
   - Animator
   - Rigidbody2D:
     • Body Type: Dynamic
     • Gravity Scale: 0
     • Constraints: Freeze Rotation Z ✓
     • Collision Detection: Continuous
   - BoxCollider2D hoặc CapsuleCollider2D:
     • Is Trigger: ✗ (KHÔNG check)
   - LinhGiaiPhong1968 (script)
```

### 2.3. Nếu THIẾU Collider
```
1. Add Component → Box Collider 2D
2. Adjust Size để vừa với nhân vật
3. Offset: (0, 0) hoặc điều chỉnh cho đúng
```

---

## 🔧 BƯỚC 3: KIỂM TRA ENEMY COMPONENTS

### 3.1. Chọn Enemy GameObject
```
Hierarchy → Enemy (hoặc tên enemy của bạn)
```

### 3.2. Kiểm tra Components
```
✅ Enemy phải có:
   - Transform
   - SpriteRenderer (Sorting Layer: Enemy hoặc Default)
   - Animator
   - Rigidbody2D:
     • Body Type: Dynamic
     • Gravity Scale: 0
     • Constraints: Freeze Rotation Z ✓
     • Collision Detection: Continuous
   - BoxCollider2D hoặc CapsuleCollider2D:
     • Is Trigger: ✗ (KHÔNG check)
   - Enermy1968Controller (script)
```

---

## 🔧 BƯỚC 4: SETUP PHYSICS LAYERS (TÙY CHỌN - Nâng cao)

### 4.1. Tạo Physics Layers
```
Edit → Project Settings → Tags and Layers
→ Layers (mở rộng)

Layer 6: Player ✓ (đã có)
Layer 7: Enemy
Layer 8: Ground
Layer 9: Obstacles
```

### 4.2. Gán Layer cho GameObjects
```
Player GameObject → Inspector → Layer: Player (6)
Enemy GameObject → Layer: Enemy (7)
Ground Tilemap → Layer: Ground (8)
Building Tilemap → Layer: Obstacles (9)
```

### 4.3. Cấu hình Collision Matrix
```
Edit → Project Settings → Physics 2D
→ Layer Collision Matrix (cuộn xuống)

BỎ CHECK (không va chạm):
   Player ✗ Enemy (player và enemy đi xuyên qua nhau)
   Player ✗ Ground (player không va chạm với mặt đất)
   Enemy ✗ Ground (enemy không va chạm với mặt đất)

GIỮ CHECK (có va chạm):
   Player ✓ Obstacles (player va chạm tòa nhà)
   Enemy ✓ Obstacles (enemy va chạm tòa nhà)
   Obstacles ✓ Obstacles (tòa nhà va chạm với nhau)
```

---

## ✅ BƯỚC 5: TEST

### 5.1. Test trong Scene View
```
1. Scene View → Click vào Building tilemap
2. Bật Gizmos (góc trên bên phải Scene View)
3. Bạn sẽ thấy viền xanh lá quanh tòa nhà (collider)
```

### 5.2. Test trong Game
```
1. Nhấn Play ▶
2. Di chuyển Player:
   ✅ Player đi được trên đường (Ground)
   ✅ Player BỊ CHẶN bởi tòa nhà (Building)
   ✅ Player KHÔNG đi xuyên qua tường

3. Kiểm tra Enemy:
   ✅ Enemy patrol trên đường
   ✅ Enemy BỊ CHẶN bởi tòa nhà
```

### 5.3. Nếu KHÔNG HOẠT ĐỘNG
```
❌ Player vẫn đi xuyên tường:
   → Kiểm tra Player có Rigidbody2D (Dynamic) và Collider2D
   → Kiểm tra Building có TilemapCollider2D và Rigidbody2D (Static)

❌ Player bị dính/giật:
   → Rigidbody2D → Collision Detection: Continuous
   → CompositeCollider2D → Geometry Type: Polygons

❌ Enemy không patrol:
   → Kiểm tra script Enermy1968Controller
   → Adjust patrol points
```

---

## 🎨 BONUS: TINH CHỈNH COLLIDER

### Nếu collider không khớp với hình tòa nhà:

#### Cách 1: Edit từng Tile
```
1. Window → 2D → Tile Palette
2. Chọn tile tòa nhà trong palette
3. Inspector → Collider Type:
   - None: Không va chạm
   - Sprite: Va chạm theo hình sprite (khuyên dùng)
   - Grid: Va chạm toàn bộ ô vuông
4. Save tile asset
5. Quay lại Scene → Building tilemap sẽ tự update
```

#### Cách 2: Edit Composite Collider
```
1. Chọn Building tilemap
2. CompositeCollider2D → Edit Collider (button)
3. Kéo các điểm xanh lá để chỉnh hình collider
4. Done khi hoàn thành
```

---

## 📋 CHECKLIST HOÀN THÀNH

```
BUILDING TILEMAP:
[ ] Có TilemapCollider2D
[ ] Có CompositeCollider2D (Geometry: Polygons)
[ ] Có Rigidbody2D (Body Type: Static)
[ ] TilemapCollider2D: "Used By Composite" checked
[ ] Layer: Obstacles (9)

GROUND TILEMAP:
[ ] KHÔNG có TilemapCollider2D
[ ] Layer: Ground (8)

PLAYER:
[ ] Có Rigidbody2D (Dynamic, Gravity: 0)
[ ] Có BoxCollider2D hoặc CapsuleCollider2D
[ ] Collider: Is Trigger = KHÔNG check
[ ] Layer: Player (6)
[ ] Tag: Player

ENEMY:
[ ] Có Rigidbody2D (Dynamic, Gravity: 0)
[ ] Có BoxCollider2D hoặc CapsuleCollider2D
[ ] Collider: Is Trigger = KHÔNG check
[ ] Layer: Enemy (7)

TEST:
[ ] Player đi được trên Ground
[ ] Player BỊ CHẶN bởi Building
[ ] Enemy đi được trên Ground
[ ] Enemy BỊ CHẶN bởi Building
[ ] Không có lỗi trong Console
```

---

## 🚀 NHANH GỌN (TL;DR)

```
1. Chọn Building tilemap
2. Add Component → Tilemap Collider 2D
3. Add Component → Composite Collider 2D
4. TilemapCollider2D: ✓ Used By Composite
5. Rigidbody2D (tự động): Body Type = Static
6. Kiểm tra Player có Rigidbody2D + Collider2D
7. Kiểm tra Enemy có Rigidbody2D + Collider2D
8. Test game ▶
```

---

## 🆘 TROUBLESHOOTING

### Player đi chậm hoặc giật:
```
Player → Rigidbody2D:
- Interpolate: Interpolate (smooth movement)
- Collision Detection: Continuous
```

### Enemy không tìm đường:
```
Enermy1968Controller.cs đang dùng Raycast/Physics để tìm đường.
Đảm bảo enemy có Rigidbody2D và Layer Matrix đúng.
```

### Collider quá lớn/nhỏ:
```
Player/Enemy → Collider2D → Edit Collider
Kéo các điểm xanh để chỉnh size
```

---

Làm xong hãy báo tôi biết nhé! 🎮

