# ✅ CHECKLIST - Kiểm Tra Trước Khi Test

## 🎯 Enemy GameObject

Chọn Enemy trong Hierarchy, kiểm tra Inspector:

```
✅ Transform: Position trong scene view, không bị ẩn
✅ Rigidbody2D: 
   - Gravity Scale = 0
✅ Collider2D (BoxCollider2D hoặc CapsuleCollider2D):
   - Enabled = TRUE (tick)
   - Is Trigger = FALSE (BỎ tick) ⚠️ QUAN TRỌNG!
   - Size/Radius phù hợp với sprite
✅ Animator: Có gắn controller
✅ SpriteRenderer: Có sprite, có hiện trong scene
✅ Enermy1968Controller: Script có gắn
```

## 🎯 Player GameObject

```
✅ Tag: Player
✅ Collider2D: Is Trigger = FALSE
✅ LinhGiaiPhong1968: 
   - Bullet Prefab đã gắn
   - Fire Point đã gắn
```

## 🎯 Bullet_Player Prefab

```
✅ Rigidbody2D: Gravity Scale = 0
✅ CircleCollider2D:
   - Is Trigger = TRUE (TICK) ⚠️
✅ PlayerBullet script: Có gắn
```

## 🎯 Bullet_Enemy Prefab

```
✅ Rigidbody2D: Gravity Scale = 0
✅ CircleCollider2D:
   - Is Trigger = TRUE (TICK)
✅ EnemyBullet script: Có gắn
```

## 🎮 Test

1. Play
2. Di chuyển player gần enemy
3. Bắn (Click/Space/Enter)
4. Xem Console

### Kết Quả Mong Đợi:

```
✅ Player shot bullet in direction: (1.00, 0.00)
✅ Player bullet hit: Enemy (hoặc tên enemy object)
✅ ★★★ Player bullet hit enemy! Enemy taking damage! ★★★
✅ Enemy took 1 damage! Health: 1/2
```

Bắn lần 2:
```
✅ Enemy took 1 damage! Health: 0/2
✅ Enemy died!
```

---

## ⚠️ VẤN ĐỀ THƯỜNG GẶP

### Không thấy "★★★ Player bullet hit enemy!":

**Kiểm tra theo thứ tự:**

1. **Enemy có Collider2D?**
   - Không → Add Component → BoxCollider2D

2. **Collider2D có Is Trigger = false?**
   - Phải BỎ TICK (false)
   - Nếu Is Trigger = true → Sửa lại thành false

3. **Enemy có script Enermy1968Controller?**
   - Không → Add Component → Enermy1968Controller

4. **Enemy và Player có ở gần nhau không?**
   - Scene View: Kiểm tra vị trí
   - Game View: Nhìn thấy enemy không?

5. **Đạn có bay không?**
   - Thấy đạn bay ra từ player?
   - Nếu không → Check Bullet Prefab đã gắn vào Player chưa

---

**Nếu tất cả OK mà vẫn không chạy → Gửi screenshot cho tôi!**

