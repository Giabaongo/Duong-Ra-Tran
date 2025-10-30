# 🔍 CÁCH DÙNG COLLISION DEBUGGER

## 📝 SCRIPT LÀ GÌ?
`CollisionDebugger.cs` là script giúp bạn **NHÌN THẤY** collision boundaries trong khi chơi game.

---

## 🚀 CÁCH SETUP (30 GIÂY)

### Bước 1: Đính Script vào GameObject
```
1. Hierarchy → Click vào "Main Camera" (hoặc GameObject nào cũng được)
2. Inspector → Add Component
3. Gõ: "Collision Debugger"
4. Enter
```

### Bước 2: Cấu hình (Tùy chọn)
```
Inspector → Collision Debugger component:

Show Player Collider: ✓ (hiện collider của Player)
Show Enemy Collider: ✓ (hiện collider của Enemy)
Show Building Collider: ✓ (hiện collider của tòa nhà)
Show Ground Collider: ✗ (không cần - ground không có collider)
Show Debug GUI: ✓ (hiện thông tin debug góc trên trái)

Colors:
- Player Color: Green (xanh lá)
- Enemy Color: Red (đỏ)
- Building Color: Yellow (vàng)
- Ground Color: Cyan (xanh dương)
```

### Bước 3: Chạy Game
```
1. Nhấn Play ▶
2. Bạn sẽ THẤY:
   - Viền xanh lá quanh Player (collider)
   - Viền đỏ quanh Enemy (collider)
   - Viền vàng quanh tòa nhà (collider)
   - Thông tin debug ở góc trên bên trái màn hình
```

---

## 📊 THÔNG TIN DEBUG HIỂN THỊ

### Trong Game View (góc trên trái):
```
=== COLLISION DEBUG ===
Player: ✓ RB, ✓ Collider
Building: ✓ TC, ✓ CC, ✓ RB
  Body Type: Static
Enemies: 2 found
```

**Giải thích:**
- `✓ RB` = Có Rigidbody2D
- `✓ Collider` = Có Collider2D
- `✓ TC` = Có TilemapCollider2D
- `✓ CC` = Có CompositeCollider2D
- `Body Type: Static` = Tòa nhà không di chuyển (đúng!)

### Trong Scene View:
```
Bật Gizmos → Bạn sẽ thấy viền màu xung quanh tất cả colliders
```

---

## ✅ KIỂM TRA SETUP ĐÚNG

### ✓ ĐÚNG - Mọi thứ hoạt động:
```
Player: ✓ RB, ✓ Collider (màu xanh lá)
Building: ✓ TC, ✓ CC, ✓ RB (màu xanh lá)
  Body Type: Static (màu xanh lá)
```
→ Player bị chặn bởi tòa nhà ✓

### ✗ SAI - Player thiếu Collider:
```
Player: ✓ RB, ✗ Collider (màu đỏ)
```
→ Fix: Add Component → Box Collider 2D cho Player

### ✗ SAI - Building thiếu Collider:
```
Building: ✗ TC, ✗ CC, ✗ RB (màu đỏ)
```
→ Fix: Làm theo file `QUICK_FIX_COLLISION.md`

### ⚠️ CẢNH BÁO - Building không Static:
```
Building: ✓ TC, ✓ CC, ✓ RB
  Body Type: Dynamic (màu vàng)
```
→ Fix: Building → Rigidbody2D → Body Type = Static

---

## 🎨 HÌNH ẢNH TRONG GAME

Khi chạy game với debug script, bạn sẽ thấy:

```
     [Tòa nhà] ← Viền vàng (Building collider)
        │
        │
     ╔═══╗
     ║ P ║ ← Viền xanh lá (Player collider)
     ╚═══╝
        │
     ╔═══╗
     ║ E ║ ← Viền đỏ (Enemy collider)
     ╚═══╝
```

Nếu Player đi vào tòa nhà:
- Viền xanh lá **ĐỤ NG** viền vàng = VA CHẠM ĐÚNG ✓
- Viền xanh lá **ĐI XU YÊN** viền vàng = LỖI ✗

---

## 🔧 TÙY CHỈNH

### Thay đổi màu sắc:
```
Inspector → Collision Debugger:
Player Color: Click vào ô màu → Chọn màu khác
```

### Tắt debug một số đối tượng:
```
Show Enemy Collider: ✗ (bỏ check) → Không hiện enemy collider
Show Debug GUI: ✗ (bỏ check) → Tắt thông tin text góc trên trái
```

### Xóa script khi không cần:
```
Inspector → Collision Debugger → ⚙ → Remove Component
```

---

## 🆘 TROUBLESHOOTING

### Không thấy viền màu:
```
1. Kiểm tra GameObject có script CollisionDebugger
2. Scene View → Bật Gizmos (góc trên phải)
3. Game View → Chạy game để thấy thông tin debug
```

### Thông tin debug không hiện:
```
1. Kiểm tra script được đính vào GameObject đang active
2. Inspector → CollisionDebugger → Checkboxes đều được check
```

### "Player: ✗ Not Found":
```
1. Kiểm tra Player có Tag "Player"
2. Hierarchy → Player → Inspector → Tag: Player
```

### "Building: ✗ Not Found":
```
1. Kiểm tra Grid → Building có tồn tại
2. Hierarchy → Grid → Building phải có đúng tên
```

---

## 💡 TIPS

1. **Chỉ dùng khi debug** - Xóa script khi build game ra (tốn performance)
2. **Xem Scene View** - Bật Gizmos để thấy rõ hơn
3. **Screenshot** - Chụp màn hình gửi tôi nếu có lỗi, dễ debug!

---

## 📋 CHECKLIST

```
[ ] Script được đính vào Main Camera hoặc GameObject nào đó
[ ] Chạy game thấy thông tin debug góc trên trái
[ ] Thấy viền màu quanh Player, Enemy, Building
[ ] Tất cả đều hiển thị ✓ (màu xanh lá)
[ ] Player bị chặn bởi tòa nhà
```

---

Xong! Giờ bạn có thể NHÌN THẤY collision để debug dễ hơn! 🎉

