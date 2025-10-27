# Hướng Dẫn Setup Lính Giải Phóng 1968

## 📋 Tổng Quan

Script `LinhGiaiPhong1968.cs` là controller hoàn chỉnh cho nhân vật lính giải phóng với:
- ✅ Di chuyển 4 hướng (WASD/Arrow keys)
- ✅ Animation states: Idle, Run, Attack, Hit
- ✅ Hệ thống máu
- ✅ Attack cooldown
- ✅ Sprite flip theo hướng di chuyển

## 🎮 Cách Setup Trong Unity

### Bước 1: Tạo GameObject Cho Player

1. Trong Hierarchy, tạo GameObject mới (Right Click → Create Empty)
2. Đổi tên thành `LinhGiaiPhong1968`
3. Reset Transform (Position: 0, 0, 0)

### Bước 2: Thêm Components Cần Thiết

Thêm các component sau vào GameObject:

#### 2.1. Sprite Renderer
- Add Component → Rendering → Sprite Renderer
- Gán sprite idle đầu tiên của nhân vật vào Sprite field

#### 2.2. Rigidbody2D
- Add Component → Physics 2D → Rigidbody 2D
- **Cấu hình tự động bởi script**, nhưng có thể check:
  - Body Type: Dynamic
  - Gravity Scale: 0
  - Constraints: Freeze Rotation (Z)
  - Collision Detection: Continuous

#### 2.3. Collider2D
- Add Component → Physics 2D → Box Collider 2D (hoặc Capsule Collider 2D)
- Điều chỉnh size cho vừa với sprite

#### 2.4. Animator
- Add Component → Animation → Animator
- Gán Animator Controller:
  - Vào folder `Assets/Annimations/LinhGiaiPhong1968/`
  - Kéo file `Player.controller` vào Animator component
  - Check: Controller = `Player`

#### 2.5. LinhGiaiPhong1968 Script
- Add Component → Scripts → Linh Giai Phong 1968
- Hoặc kéo script `LinhGiaiPhong1968.cs` vào GameObject

### Bước 3: Cấu Hình Script Parameters

Trong Inspector, bạn sẽ thấy các settings:

#### Movement Settings:
- **Move Speed**: `3` (tốc độ di chuyển, có thể tăng/giảm)
- **Attack Cooldown**: `0.5` (thời gian giữa các lần tấn công)

#### Health:
- **Max Health**: `100` (máu tối đa)

### Bước 4: Kiểm Tra Animator Setup

Mở Animator Window (Window → Animation → Animator):

✅ **Kiểm tra Parameters:**
- `isRunning` (Bool)
- `isAttacking` (Bool)
- `isHitting` (Bool)

✅ **Kiểm tra States:**
- PlayerIdle (Default state)
- PlayerRun
- PlayerAttack
- PlayerHit

## 🎹 Phím Điều Khiển

### Di Chuyển:
- **W** hoặc **↑**: Lên
- **S** hoặc **↓**: Xuống
- **A** hoặc **←**: Trái
- **D** hoặc **→**: Phải

### Tấn Công:
- **Chuột Trái** (Left Click)
- **Enter**
- **Space**

## 🔧 Sử Dụng Nâng Cao

### 1. Gọi TakeDamage từ script khác:

```csharp
// Trong script Enemy hoặc Bullet
LinhGiaiPhong1968 player = collision.GetComponent<LinhGiaiPhong1968>();
if (player != null)
{
    player.TakeDamage(10); // Gây 10 damage
}
```

### 2. Hồi máu cho player:

```csharp
// Trong script Health Pack
LinhGiaiPhong1968 player = collision.GetComponent<LinhGiaiPhong1968>();
if (player != null)
{
    player.Heal(20); // Hồi 20 HP
}
```

### 3. Kiểm tra trạng thái:

```csharp
if (player.IsAlive())
{
    int currentHP = player.GetCurrentHealth();
    int maxHP = player.GetMaxHealth();
    Debug.Log($"HP: {currentHP}/{maxHP}");
}
```

## 🐛 Troubleshooting

### Nhân vật không di chuyển?
- ✅ Check Rigidbody2D Body Type = Dynamic
- ✅ Check Gravity Scale = 0
- ✅ Check script có được enable không
- ✅ Xem Console có log "LinhGiaiPhong1968 initialized successfully!"

### Animation không chạy?
- ✅ Check Animator Controller đã được gán
- ✅ Check các animation clips đã được assign trong controller
- ✅ Mở Animator window xem parameters có đổi giá trị không

### Sprite không flip?
- ✅ Sprite đang ở scale (1, 1, 1) ban đầu
- ✅ Pivot point của sprite ở giữa

### Nhân vật xoay tròn?
- ✅ Rigidbody2D Constraints: Freeze Rotation (Z) phải được tick

## 📝 Ghi Chú

- Script hỗ trợ cả **Old Input System** và **New Input System**
- Animation timing được tính dựa trên animation clips hiện có:
  - Attack: ~0.27s
  - Hit: ~0.53s
- Có thể điều chỉnh timing trong code nếu animation của bạn khác

## 🎯 Tips Tối Ưu

1. **Tốc độ phù hợp**: Move Speed từ 2-5 là hợp lý cho game 2D
2. **Attack Cooldown**: 0.3-0.7s để tránh spam attack
3. **Collision Layer**: Nên tạo layer riêng cho Player và Enemy để tối ưu collision detection

---

**Chúc bạn code game thành công! 🎮✨**

