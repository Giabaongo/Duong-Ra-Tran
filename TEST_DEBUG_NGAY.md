# 🔍 TEST DEBUG - Kiểm Tra Đạn Player

## ⚡ CHẠY NGAY VÀ XEM LOG

Code đã thêm debug logs. Bây giờ:

1. **Save tất cả files** (Ctrl+S)
2. **Quay lại Unity** (để recompile)
3. **Play game**
4. **Bắn 1 phát**
5. **Xem Console log**

## 📊 Kết Quả Sẽ Thấy

### ✅ NẾU ĐÚNG - Sẽ thấy:

```
Player shot bullet in direction: (1.00, 0.00)
★ PlayerBullet spawned! Has Collider: True, Is Trigger: True
★ Ignored collision with player
★ PlayerBullet velocity set to: (10.00, 0.00)
```

Sau đó khi đạn va chạm với enemy:
```
Player bullet hit: Enemy (hoặc tên enemy object)
★★★ Player bullet hit enemy! Enemy taking damage! ★★★
Enemy took 1 damage! Health: 1/2
```

### ❌ NẾU SAI - Có thể thấy:

**Trường hợp 1: Không có log ★ PlayerBullet spawned!**
```
→ Đạn KHÔNG được spawn!
→ Problem: Bullet Prefab không gắn vào Player
→ Fix: Gắn Bullet_Player prefab vào LinhGiaiPhong1968
```

**Trường hợp 2: Has Collider: False**
```
★ PlayerBullet spawned! Has Collider: False, Is Trigger: False
→ Bullet prefab KHÔNG có Collider2D!
→ Fix: Mở Bullet_Player prefab → Add Component → CircleCollider2D
→ Set Is Trigger = TRUE
```

**Trường hợp 3: Velocity = (0, 0)**
```
★ PlayerBullet velocity set to: (0.00, 0.00)
→ Đạn không bay!
→ Problem: Speed = 0 hoặc direction sai
→ Fix: Check PlayerBullet settings trong prefab (Speed = 10)
```

**Trường hợp 4: KHÔNG có log "Player bullet hit:"**
```
→ Đạn bay ra nhưng không va chạm với gì
→ Enemy không có Collider2D HOẶC quá xa
→ Fix: Check Enemy có BoxCollider2D (Is Trigger = false)
```

## 🎯 Sau Khi Xem Log

### Nếu thấy "Has Collider: True" và "velocity: (10, 0)" nhưng vẫn không hit enemy:

**Kiểm tra Enemy GameObject:**

```
1. Hierarchy → Chọn Enemy
2. Inspector → Components:

✅ BoxCollider2D (hoặc CapsuleCollider2D)
   - Enabled: TRUE (tick)
   - Is Trigger: FALSE (BỎ tick) ⚠️ QUAN TRỌNG!
   - Size: Phù hợp với sprite (ví dụ: 1 x 1)

✅ Enermy1968Controller: Script có gắn

✅ Transform: Position trong scene (player phải bắn đúng hướng)
```

### Nếu enemy quá xa hoặc không đúng hướng:

```
1. Scene View: Nhìn vị trí player và enemy
2. Enemy phải ở BÊN PHẢI player (vì đạn bay (1, 0) = sang phải)
3. Nếu enemy ở bên trái → Di chuyển player qua bên trái enemy
   hoặc enemy qua bên phải player
```

## 🐛 Quick Fix Checklist

Theo thứ tự ưu tiên:

- [ ] Bullet_Player prefab đã gắn vào Player?
- [ ] Bullet_Player prefab có CircleCollider2D (Is Trigger = true)?
- [ ] Bullet_Player prefab có Rigidbody2D (Gravity = 0)?
- [ ] Enemy có BoxCollider2D (Is Trigger = FALSE)?
- [ ] Enemy có script Enermy1968Controller?
- [ ] Enemy ở đúng vị trí (bên phải player nếu player nhìn phải)?

---

**Chạy game và gửi cho tôi LOG với các dòng có ★ nhé!**

