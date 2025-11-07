# Hướng Dẫn Setup Enemy System - Mậu Thân 1968

## 📋 Tổng Quan

Hệ thống Enemy đã được tạo với các tính năng:
- ✅ Enemy có 2 máu (bị bắn 2 lần thì chết)
- ✅ Enemy tự động di chuyển patrol
- ✅ Enemy phát hiện player và đuổi theo
- ✅ Enemy tự động bắn player khi ở gần
- ✅ Player bị bắn 5 lần sẽ GAME OVER
- ✅ GameManager tự động restart game

## 🎮 Scripts Đã Tạo

1. **Enermy1968Controller.cs** - AI cho enemy
2. **EnemyBullet.cs** - Đạn của enemy
3. **LinhGiaiPhong1968.cs** - Đã cập nhật với logic bị bắn 5 lần
4. **GameManager1968.cs** - Quản lý game over

## 🔧 Setup Trong Unity

### Bước 1: Setup Enemy Prefab

1. **Tạo Enemy GameObject:**
   - Tạo Empty GameObject, đặt tên `Enemy1968`
   - Thêm các components:
     - `Rigidbody2D` (Gravity Scale = 0, Freeze Rotation = true)
     - `BoxCollider2D` hoặc `CapsuleCollider2D`
     - `Animator` (gắn Enemy.controller)
     - `SpriteRenderer`
     - Script `Enermy1968Controller`

2. **Setup Enermy1968Controller trong Inspector:**
   ```
   Enemy Stats:
   - Max Health: 2
   
   Movement Settings:
   - Move Speed: 2
   - Patrol Range: 5
   
   Detection Settings:
   - Detection Range: 8
   - Player Layer: Player (chọn layer)
   
   Shooting Settings:
   - Bullet Prefab: [Gắn Enemy_Bullet prefab - xem bước 2]
   - Fire Point: [Tạo Empty child GameObject làm điểm bắn]
   - Shoot Cooldown: 2
   - Bullet Speed: 10
   ```

3. **Tạo Fire Point:**
   - Tạo Empty GameObject con của Enemy1968
   - Đặt tên là `FirePoint`
   - Đặt position ở phía trước enemy (ví dụ: x: 0.5, y: 0)
   - Gắn FirePoint vào slot Fire Point trong Enermy1968Controller

### Bước 2: Setup Enemy Bullet Prefab

1. **Tạo Bullet GameObject:**
   - Tạo GameObject mới, đặt tên `Enemy_Bullet`
   - Thêm components:
     - `Rigidbody2D` (Gravity Scale = 0)
     - `CircleCollider2D` (Is Trigger = true, Radius ≈ 0.1)
     - `SpriteRenderer` (gắn sprite cho đạn)
     - Script `EnemyBullet`

2. **Setup EnemyBullet trong Inspector:**
   ```
   Bullet Settings:
   - Damage: 1
   - Life Time: 5
   ```

3. **Tạo Prefab:**
   - Kéo `Enemy_Bullet` vào folder Prefabs
   - Xóa Enemy_Bullet trong scene
   - Gắn prefab vào slot Bullet Prefab của Enermy1968Controller

### Bước 3: Setup Player

1. **Thêm Tag cho Player:**
   - Chọn Player GameObject
   - Ở Inspector, đặt Tag = `Player`

2. **Kiểm tra LinhGiaiPhong1968:**
   - Max Health đã được set = 5
   - Player sẽ tự động gọi GameManager khi chết

### Bước 4: Setup GameManager

1. **Tạo GameManager GameObject:**
   - Tạo Empty GameObject, đặt tên `GameManager`
   - Thêm script `GameManager1968`

2. **Setup GameManager1968 trong Inspector:**
   ```
   Game Over Settings:
   - Restart Delay: 3 (giây chờ trước khi restart)
   
   UI References:
   - Game Over UI: [Optional - nếu bạn có UI game over]
   ```

### Bước 5: Setup Layers (Quan Trọng!)

1. **Tạo Layers:**
   - Edit → Project Settings → Tags and Layers
   - Thêm layer mới: `Player`
   - Thêm layer mới: `Enemy`
   - Thêm layer mới: `EnemyBullet`

2. **Gắn Layers:**
   - Player GameObject: Layer = `Player`
   - Enemy GameObject: Layer = `Enemy`
   - Enemy_Bullet prefab: Layer = `EnemyBullet`

3. **Setup Collision Matrix:**
   - Edit → Project Settings → Physics 2D
   - Bỏ tick giữa:
     - `Enemy` ↔ `EnemyBullet` (đạn không va chạm enemy)
     - `EnemyBullet` ↔ `EnemyBullet` (đạn không va chạm nhau)

### Bước 6: Setup Animator

Enemy đã có Animator Controller sẵn (`Enemy.controller`) với các state:
- **EnemyIdle** (mặc định)
- **EnemyRun** (khi isRunning = true)
- **EnermyAttack** (khi isAttacking = true)
- **EnemyHit** (khi isHitting = true)

Đảm bảo gắn đúng Animator Controller vào component Animator của Enemy.

### Bước 7: Spawn Enemy Vào Scene

1. **Cách 1: Đặt Enemy Trong Scene:**
   - Kéo Enemy1968 vào scene ở vị trí muốn spawn
   - Enemy sẽ patrol xung quanh vị trí spawn ban đầu

2. **Cách 2: Spawn Enemy Bằng Code (Advanced):**
   - Tạo script `EnemySpawner`:
   ```csharp
   using UnityEngine;
   
   public class EnemySpawner : MonoBehaviour
   {
       [SerializeField] private GameObject enemyPrefab;
       [SerializeField] private Transform[] spawnPoints;
       [SerializeField] private float spawnInterval = 5f;
       [SerializeField] private int maxEnemies = 5;
       
       private float lastSpawnTime;
       private int currentEnemyCount = 0;
       
       void Start()
       {
           SpawnEnemy();
       }
       
       void Update()
       {
           if (currentEnemyCount < maxEnemies && 
               Time.time - lastSpawnTime >= spawnInterval)
           {
               SpawnEnemy();
           }
       }
       
       void SpawnEnemy()
       {
           if (spawnPoints.Length == 0) return;
           
           Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
           Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
           
           currentEnemyCount++;
           lastSpawnTime = Time.time;
       }
   }
   ```

## 🎯 Testing Checklist

- [ ] Enemy patrol qua lại được
- [ ] Enemy nhìn thấy player (khi player vào vùng màu vàng trong Gizmos)
- [ ] Enemy đuổi theo player
- [ ] Enemy dừng lại và bắn khi player gần (vùng màu đỏ)
- [ ] Đạn enemy bay về phía player
- [ ] Player bị trúng đạn giảm máu
- [ ] Player bị trúng 5 phát → Game Over
- [ ] Game tự động restart sau 3 giây
- [ ] Enemy bị bắn 2 phát → chết và biến mất

## ⚠️ Lưu Ý

1. **Player phải có Tag "Player"** để enemy tìm được
2. **Enemy_Bullet phải có Collider2D với Is Trigger = true**
3. **Player phải có Collider2D** để va chạm với đạn
4. **Kiểm tra Layers** để tránh collision không mong muốn
5. **Gắn Animator Controller** cho cả Player và Enemy

## 🐛 Troubleshooting

**Q: Enemy không thấy Player?**
- Kiểm tra Player có tag "Player" chưa
- Kiểm tra Detection Range có đủ lớn không

**Q: Đạn không gây damage?**
- Kiểm tra EnemyBullet có script EnemyBullet chưa
- Kiểm tra Player có Collider2D và script LinhGiaiPhong1968
- Kiểm tra tag "Player" đã set đúng

**Q: Game không restart khi player chết?**
- Kiểm tra có GameManager1968 trong scene chưa
- Kiểm tra scene đã được add vào Build Settings

**Q: Enemy không bắn?**
- Kiểm tra Bullet Prefab đã gắn vào Enermy1968Controller
- Kiểm tra Fire Point đã được assign
- Kiểm tra trong Console có warning gì không

## 📝 Tùy Chỉnh

### Thay đổi độ khó:

**Dễ hơn:**
- Tăng Player Max Health (ví dụ: 10)
- Giảm Enemy Detection Range (ví dụ: 5)
- Tăng Enemy Shoot Cooldown (ví dụ: 3)

**Khó hơn:**
- Giảm Player Max Health (ví dụ: 3)
- Tăng Enemy Detection Range (ví dụ: 12)
- Giảm Enemy Shoot Cooldown (ví dụ: 1)
- Tăng Enemy Move Speed (ví dụ: 3)

## 🎨 UI Game Over (Optional)

Nếu muốn thêm UI:

1. Tạo Canvas → Text "GAME OVER"
2. Tạo Panel làm background
3. Gắn vào slot Game Over UI trong GameManager1968
4. UI sẽ tự động hiện khi player chết

---

**Chúc bạn thành công! 🎮🔫**

