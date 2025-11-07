# Hướng Dẫn Fix Lỗi Bullet và Collision

## ✅ Đã Sửa

### 1. **Fix Bullet_Enemy Prefab**
- ✅ Thay script từ `EnemyBulletScript` (cũ) sang `EnemyBullet` (mới trong MauThan1968)
- ✅ Prefab giờ sẽ không còn lỗi NullReference

### 2. **Tạo PlayerBullet.cs**
- ✅ Script mới để xử lý đạn của player
- ✅ Gây damage cho enemy
- ✅ Tự động destroy sau 5 giây

### 3. **Cập nhật LinhGiaiPhong1968.cs**
- ✅ Thêm lại chức năng bắn đạn
- ✅ Player có thể bắn theo hướng đang nhìn

### 4. **Cập nhật Enermy1968Controller.cs**
- ✅ Thêm collision với player
- ✅ Enemy gây damage khi chạm vào player

## 🔧 Setup Cần Làm Trong Unity

### Bước 1: Kiểm Tra Bullet_Enemy Prefab (Đã Tự Động)
Prefab `Bullet_Enemy` đã được cập nhật tự động. Khi mở Unity:
- ✅ Script giờ là `EnemyBullet` thay vì `EnemyBulletScript`
- ✅ Settings: Damage = 1, LifeTime = 5

### Bước 2: Tạo Player Bullet Prefab

**Tùy chọn A: Dùng prefab có sẵn**
Nếu bạn đã có prefab bullet cho player:
1. Chọn prefab đó
2. **Xóa script cũ** (nếu có)
3. **Add Component** → `PlayerBullet` (script mới trong MauThan1968)
4. Setup trong Inspector:
   ```
   Damage: 1
   Speed: 10
   Life Time: 5
   ```

**Tùy chọn B: Tạo mới từ đầu**
1. Tạo GameObject mới tên `Bullet_Player`
2. Add components:
   - `Rigidbody2D`:
     - Gravity Scale: 0
     - Collision Detection: Continuous
   - `CircleCollider2D`:
     - Is Trigger: ✅ true
     - Radius: 0.1
   - `SpriteRenderer`:
     - Gắn sprite cho đạn
   - Script `PlayerBullet`
3. Setup PlayerBullet:
   ```
   Damage: 1
   Speed: 10
   Life Time: 5
   ```
4. Kéo vào folder Prefabs để tạo prefab

### Bước 3: Setup Player

1. **Chọn Player GameObject trong scene**
2. **Tìm component LinhGiaiPhong1968**
3. **Gắn các field:**
   ```
   Shooting Settings:
   - Bullet Prefab: [Kéo Bullet_Player prefab vào đây]
   - Fire Point: [Tạo Empty child object làm điểm bắn - xem bước 4]
   - Bullet Speed: 10
   ```

### Bước 4: Tạo Fire Point Cho Player

1. **Click chuột phải vào Player** trong Hierarchy
2. **Create Empty** → Đặt tên `FirePoint`
3. **Đặt position** của FirePoint:
   - Nếu player nhìn phải: X = 0.5, Y = 0
   - Điều chỉnh để đạn spawn ở vị trí phù hợp
4. **Kéo FirePoint** vào slot Fire Point trong LinhGiaiPhong1968

### Bước 5: Setup Enemy

1. **Chọn Enemy GameObject**
2. **Tìm component Enermy1968Controller**
3. **Gắn Bullet_Enemy prefab** vào slot Bullet Prefab
4. **Tạo Fire Point cho Enemy** (tương tự player):
   - Create Empty child tên `FirePoint`
   - Position: X = 0.5, Y = 0 (hoặc vị trí phù hợp)
   - Gắn vào slot Fire Point

### Bước 6: Setup Tags (QUAN TRỌNG!)

1. **Player GameObject:**
   - Inspector → Tag: `Player` ✅

2. **Enemy GameObject:**
   - Inspector → Tag: `Enemy` ✅

3. **Tạo Tags nếu chưa có:**
   - Top menu: Edit → Project Settings → Tags and Layers
   - Click "+" để add tags:
     - `Player`
     - `Enemy`
     - `Wall` (nếu có tường)
     - `Obstacle` (nếu có vật cản)

### Bước 7: Setup Layers

1. **Edit → Project Settings → Tags and Layers**
2. **Thêm layers:**
   - Layer 6: `Player`
   - Layer 7: `Enemy`
   - Layer 8: `EnemyBullet` (đã có)
   - Layer 9: `PlayerBullet` (tạo mới)

3. **Gắn layers:**
   - Player: Layer = `Player`
   - Enemy: Layer = `Enemy`
   - Bullet_Enemy prefab: Layer = `EnemyBullet`
   - Bullet_Player prefab: Layer = `PlayerBullet`

4. **Setup Collision Matrix:**
   - Edit → Project Settings → Physics 2D
   - **Bỏ tick** các cặp không cần va chạm:
     - ❌ `Enemy` ↔ `EnemyBullet`
     - ❌ `Player` ↔ `PlayerBullet`
     - ❌ `EnemyBullet` ↔ `EnemyBullet`
     - ❌ `PlayerBullet` ↔ `PlayerBullet`
   - **Giữ tick** các cặp cần va chạm:
     - ✅ `Player` ↔ `EnemyBullet`
     - ✅ `Enemy` ↔ `PlayerBullet`
     - ✅ `Player` ↔ `Enemy`

### Bước 8: Kiểm Tra Colliders

**Player:**
- Phải có `Collider2D` (BoxCollider2D hoặc CapsuleCollider2D)
- Is Trigger: ❌ false (để có thể va chạm với enemy)

**Enemy:**
- Phải có `Collider2D`
- Is Trigger: ❌ false (để có thể va chạm với player)

**Bullets:**
- Phải có `Collider2D`
- Is Trigger: ✅ true (để không đẩy nhân vật)

## 🎮 Cách Chơi

- **Di chuyển:** WASD hoặc Arrow Keys
- **Bắn:** Click chuột trái, Space, hoặc Enter
- **Hướng bắn:** Đạn bay theo hướng player đang nhìn

## 🎯 Testing Checklist

Sau khi setup xong, test các tình huống:

- [ ] Player bắn được đạn
- [ ] Đạn player bay theo hướng đúng
- [ ] Đạn player trúng enemy → Enemy mất máu
- [ ] Enemy bắn được đạn về phía player
- [ ] Đạn enemy bay về phía player
- [ ] Đạn enemy trúng player → Player mất máu
- [ ] Player chạm vào enemy → Player mất máu
- [ ] Player chết sau 5 lần bị damage → Game Over
- [ ] Enemy chết sau 2 lần bị damage
- [ ] Đạn tự động biến mất sau 5 giây

## ⚠️ Lỗi Thường Gặp

### Lỗi: "NullReferenceException"
**Nguyên nhân:** Chưa gắn đủ references
**Giải pháp:**
- Kiểm tra Bullet Prefab đã gắn vào Player/Enemy chưa
- Kiểm tra Fire Point đã được tạo và gắn chưa
- Kiểm tra Tags đã set đúng chưa

### Lỗi: Đạn không bay
**Nguyên nhân:** Bullet prefab thiếu Rigidbody2D
**Giải pháp:**
- Add Rigidbody2D vào bullet prefab
- Set Gravity Scale = 0

### Lỗi: Đạn không gây damage
**Nguyên nhân:** Tags không đúng hoặc thiếu Collider
**Giải pháp:**
- Kiểm tra Player có tag "Player"
- Kiểm tra Enemy có tag "Enemy"
- Kiểm tra Collider2D có Is Trigger = true

### Lỗi: Player không mất máu khi chạm enemy
**Nguyên nhân:** Collision Matrix hoặc Tags
**Giải pháp:**
- Kiểm tra Player có tag "Player"
- Kiểm tra Collision Matrix: Player ↔ Enemy phải tick
- Kiểm tra cả Player và Enemy đều có Collider2D (không phải trigger)

### Lỗi: Đạn đi xuyên qua
**Nguyên nhân:** Collision Detection Mode
**Giải pháp:**
- Rigidbody2D của bullet: Collision Detection = Continuous
- Collider2D của bullet: Is Trigger = true

## 📝 Lưu Ý

1. **Script cũ EnemyBulletScript:** KHÔNG XÓA, vì scene khác đang dùng
2. **Chỉ dùng scripts trong thư mục MauThan1968** cho scene này
3. **Prefab Bullet_Enemy** đã được sửa tự động, không cần assign thủ công
4. **Nhớ Save Scene** sau khi setup xong

---

**Chúc bạn fix thành công! 🎮🔫**

