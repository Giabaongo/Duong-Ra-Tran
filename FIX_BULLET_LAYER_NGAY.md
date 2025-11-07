# ⚡ FIX Đạn Đi Xuyên Building - 30 GIÂY

## 🎯 VẤN ĐỀ
Đạn Player và Enemy đi xuyên qua Building vì **LAYERS KHÔNG KHỚP**!

```
Bullet_Player → Layer 8
Bullet_Enemy  → Layer 8
Building      → Layer 0 (Default)
→ Layer 8 không va chạm với Layer 0!
```

---

## ✅ FIX NGAY (30 GIÂY)

### Bước 1: Fix Player Bullet
```
Unity → Project → Assets/Prefabs/Bullet_Player
→ Click vào prefab
→ Inspector (góc trên)
→ Layer: Default  (đổi từ 8 về Default)
→ Apply (nếu có hỏi)
```

### Bước 2: Fix Enemy Bullet
```
Project → Assets/Prefabs/Bullet_Enemy
→ Click vào prefab
→ Inspector
→ Layer: Default  (đổi từ 8 về Default)
→ Apply
```

### Bước 3: Test
```
Play ▶
→ Bắn vào tòa nhà
→ Đạn bị chặn và biến mất ✓
```

---

## 🎨 HÌNH ẢNH MINH HỌA

### Trước khi fix:
```
Inspector → Bullet_Player prefab
├─ Tag: Untagged
└─ Layer: Layer 8  ← ❌ SAI

→ Đạn đi xuyên tường
```

### Sau khi fix:
```
Inspector → Bullet_Player prefab
├─ Tag: Untagged
└─ Layer: Default  ← ✅ ĐÚNG

→ Đạn bị chặn bởi tường ✓
```

---

## 🔍 TẠI SAO LẠI XẢY RA?

Unity có **Layer Collision Matrix** quy định layer nào va chạm với layer nào:

```
        Default  Layer 8
Default    ✓        ?
Layer 8    ?        ✓
```

Nếu ô "?" không được check → Không va chạm!

**Giải pháp đơn giản nhất:** Đổi tất cả về Layer Default (Layer 0) → Tất cả đều va chạm với nhau!

---

## 📋 CHECKLIST

```
[ ] Bullet_Player.prefab → Layer = Default
[ ] Bullet_Enemy.prefab → Layer = Default
[ ] Play game
[ ] Bắn vào tường → Đạn biến mất (không xuyên qua)
[ ] Bắn player → Player mất máu
[ ] Bắn enemy → Enemy mất máu
```

---

## 🆘 NẾU VẪN XUYÊN TƯỜNG

### Kiểm tra:

1. **Prefab đã được áp dụng chưa?**
   ```
   Inspector → Bullet prefab
   → Overrides button (nếu có)
   → Apply All
   ```

2. **Layer đã đổi chưa?**
   ```
   Inspector → Layer dropdown
   → Phải hiện "Default" (không phải "Layer 8")
   ```

3. **Building có collider chưa?**
   ```
   Hierarchy → Grid → Building
   → Inspector phải có:
     - TilemapCollider2D ✓
     - CompositeCollider2D ✓
     - Rigidbody2D (Static) ✓
   ```

4. **Đã Save scene chưa?**
   ```
   Ctrl+S hoặc File → Save
   ```

---

## 💡 GIẢI PHÁP NÂNG CAO (TÙY CHỌN)

Nếu bạn muốn giữ Layer 8 cho đạn:

```
1. Edit → Project Settings → Physics 2D
2. Cuộn xuống "Layer Collision Matrix"
3. Tìm giao điểm Layer 8 và Default
4. ✓ Check để cho phép va chạm
5. OK
```

**NHƯNG** giải pháp đơn giản hơn là đổi về Default!

---

## 🎉 KẾT QUẢ MONG ĐỢI

Sau khi fix:
- ✅ Đạn Player/Enemy bị chặn bởi tòa nhà
- ✅ Đạn biến mất khi chạm tường
- ✅ Player và Enemy vẫn di chuyển bình thường
- ✅ Đạn vẫn gây damage

---

Làm xong hãy test và báo tôi biết nhé! 🚀

