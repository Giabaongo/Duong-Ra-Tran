# ⚡ QUICK FIX - 3 PHÚT LÀM XONG

## 🎯 CÁCH LÀM NHANH NHẤT

### BƯỚC 1: Building Tilemap (1 phút)
```
1. Hierarchy → Grid → Building
2. Inspector → Add Component → gõ "Tilemap Collider 2D" → Enter
3. Add Component → gõ "Composite Collider 2D" → Enter
4. Trong TilemapCollider2D: ✓ Check "Used By Composite"
5. Trong Rigidbody2D (tự động thêm): 
   Body Type = Static ✓
```

### BƯỚC 2: Player (1 phút)
```
1. Hierarchy → Player
2. Kiểm tra có BoxCollider2D chưa? 
   - Có rồi → OK ✓
   - Chưa có → Add Component → Box Collider 2D
3. Kiểm tra Rigidbody2D:
   - Body Type: Dynamic ✓
   - Gravity Scale: 0 ✓
```

### BƯỚC 3: Enemy (1 phút)
```
1. Hierarchy → Enemy (hoặc tên enemy của bạn)
2. Kiểm tra có BoxCollider2D chưa?
   - Có rồi → OK ✓
   - Chưa có → Add Component → Box Collider 2D
3. Kiểm tra Rigidbody2D:
   - Body Type: Dynamic ✓
   - Gravity Scale: 0 ✓
```

### TEST
```
Nhấn Play ▶
→ Player bị chặn bởi tòa nhà = THÀNH CÔNG! 🎉
→ Player vẫn đi xuyên tường = Xem file SETUP_MAP_COLLISION.md
```

---

## 🖼️ HÌNH ẢNH MINH HỌA

### Building Tilemap phải có:
```
Building (GameObject)
├─ Transform
├─ Tilemap
├─ Tilemap Renderer
├─ TilemapCollider2D ← ✓ THÊM CÁI NÀY
├─ CompositeCollider2D ← ✓ THÊM CÁI NÀY
└─ Rigidbody2D (Static) ← ✓ TỰ ĐỘNG THÊM
```

### Player/Enemy phải có:
```
Player (GameObject)
├─ Transform
├─ Sprite Renderer
├─ Animator
├─ Rigidbody2D (Dynamic, Gravity: 0) ← ✓
├─ BoxCollider2D ← ✓ KIỂM TRA CÓ CHƯA
└─ Script (LinhGiaiPhong1968)
```

---

## ⚠️ LƯU Ý QUAN TRỌNG

1. **Building** → Rigidbody2D → Body Type = **Static** (không di chuyển)
2. **Player/Enemy** → Rigidbody2D → Body Type = **Dynamic** (di chuyển được)
3. **TilemapCollider2D** → ✓ Check "Used By Composite"
4. **Collider2D** → ✗ KHÔNG check "Is Trigger" (để va chạm)

---

Xong rồi test ngay! Có vấn đề gì xem file chi tiết `SETUP_MAP_COLLISION.md` nhé! 🚀

