# 💣 HƯỚNG DẪN SETUP BOMBING - B52 THẢ BOM

**Thời gian:** ~10 phút  
**Độ khó:** ⭐⭐ Trung bình

---

## 🎯 **TÍNH NĂNG:**

Máy bay B52 giờ có thể **THẢ BOM** khi bay qua!

```
✈️ B52 bay qua
    ↓ 💣 Thả bom
    ↓ 🔥 Bom rơi xuống
    💥 NỔ khi chạm đất!
    ⚔️ Gây damage cho enemies trong bán kính
```

---

## 📦 **CẦN CHUẨN BỊ:**

### 1. **Sprites:**
- 🎨 **Bomb sprite** (quả bom) - Tìm hoặc vẽ đơn giản
- 🔥 **Explosion sprites** (hiệu ứng nổ) - 4-8 frames

### 2. **Âm thanh:**
- 🔊 **Bomb drop sound** (tiếng rít - whistle) khi thả
- 💥 **Explosion sound** (tiếng nổ - boom)

### 3. **Nguồn tài nguyên miễn phí:**

**Sprites:**
- **    ** → Search "explosion sprite sheet"
- **Kenney.nl** → Particle Pack
- **Itch.io** → Free VFX

**Âm thanh:**
- **Freesound.org** → Search:
  - "bomb whistle"
  - "explosion boom"
  - "bomb drop"
- **Zapsplat.com** → Free SFX

---

## 🚀 **BƯỚC 1: TẠO BOMB PREFAB (5 phút)**

### **A. Tạo GameObject cho Bomb:**

1. **Hierarchy** → Right click → **Create Empty**
2. Tên: `Bomb_B52`
3. **Add Component** → **   **
   ```
   Is Trigger: ✅ Checked
   Radius: 0.2
   ```

4. **Add Component** → **Sprite Renderer**
   ```
   Sprite: Kéo sprite bom vào (hoặc dùng hình tròn đen tạm)
   Color: Đen hoặc xám
   Sorting Order: 10
   ```

5. **Add Component** → **B52 Bomb** (script vừa tạo)
   ```
   💣 Bom Settings:
   - Fall Speed: 10
   - Explosion Radius: 3 (3 units)
   - Damage: 50 (gây 50 damage)
   
   🔊 Âm Thanh:
   - Explosion Sound: Kéo file explosion.mp3 vào
   - Explosion Volume: 0.8
   
   🎨 Hiệu Ứng:
   - Explosion Effect Prefab: (sẽ tạo ở bước 2)
   - Explosion Effect Duration: 2
   - Bomb Sprite: (Same as Sprite Renderer)
   ```

6. **Tag:** Tạo tag "Bomb" và gán cho GameObject này

7. **Save as Prefab:**
   ```
   Kéo từ Hierarchy vào:
   Assets/Prefabs/Effects/Bomb_B52.prefab
   ```

8. **XÓA** khỏi Hierarchy (giữ prefab)

---

## 🔥 **BƯỚC 2: TẠO EXPLOSION EFFECT PREFAB (3 phút)**

### **Cách 1: Dùng Sprite Animation (Đơn giản)**

1. **Hierarchy** → Right click → **Create Empty**
2. Tên: `Explosion_Effect`
3. **Add Component** → **Sprite Renderer**
   ```
   Sprite: Kéo explosion frame đầu tiên vào
   Color: Trắng (hoặc cam/đỏ)
   Sorting Order: 100 (Hiện trên cùng)
   ```

4. **Add Component** → **Explosion Effect** (script vừa tạo)
   ```
   ⚙️ Settings:
   - Lifetime: 1.5 (giây)
   - Explosion Scale: (2, 2, 1)
   - Fade Out: ✅ Checked
   
   🎨 Sprite Animation:
   - Explosion Sprites: Kéo tất cả explosion frames vào (4-8 sprites)
   - Animation FPS: 20
   ```

5. **Save as Prefab:**
   ```
   Kéo vào:
   Assets/Prefabs/Effects/Explosion_Effect.prefab
   ```

6. **XÓA** khỏi Hierarchy

### **Cách 2: Dùng Particle System (Nâng cao)**

1. **Hierarchy** → Right click → **Effects** → **Particle System**
2. Tên: `Explosion_Particles`
3. Cài đặt Particle System:
   ```
   Duration: 0.5
   Start Lifetime: 0.5-1.0
   Start Speed: 5-10
   Start Size: 0.5-2.0
   Start Color: Vàng/Cam/Đỏ
   Emission: Burst 50-100 particles
   Shape: Sphere, Radius: 0.5
   ```

4. **Add Component** → **Explosion Effect**
5. **Save as Prefab**
6. **XÓA** khỏi Hierarchy

---

## ⚙️ **BƯỚC 3: CẤU HÌNH B52 BOMBING (2 phút)**

1. Chọn GameObject `B52_Shadow` trong Hierarchy

2. Component **B52 Shadow Simple**

3. Section **💣 BOMBING (Thả bom)**:
   ```
   ✅ Enable Bombing: CHECK VÀO ĐÂY để bật!
   
   Bomb Prefab: Kéo Bomb_B52.prefab vào ⬅️ QUAN TRỌNG!
   
   Bomb Interval: 0.5 (Thả mỗi 0.5 giây)
   Max Bombs: 10 (Tối đa 10 quả/chuyến bay)
   
   Bomb Drop Sound: Kéo whistle.mp3 vào (tiếng rít)
   ```

4. Mở prefab **Bomb_B52.prefab**:
   ```
   Component: B52 Bomb
   → Explosion Effect Prefab: Kéo Explosion_Effect.prefab vào
   ```

5. **Save All** (Ctrl + S)

---

## 🎮 **BƯỚC 4: TEST (30 giây)**

1. **Click Play** ▶️
2. **Đợi B52 bay qua**
3. Quan sát:
   - ✅ Thấy bom rơi xuống từ máy bay
   - ✅ Nghe tiếng rít khi thả
   - ✅ Bom nổ khi chạm đất
   - ✅ Nghe tiếng nổ
   - ✅ Thấy hiệu ứng nổ
   - ✅ Enemies gần bị damage

4. Check **Console**:
   ```
   [B52Shadow] 💣 Bomb dropped! (1/10)
   [Bomb] 💥 EXPLOSION at (x, y)
   [Bomb] ⚔️ Hit enemy: Enemy (2), dealt 50 damage!
   ```

---

## 🎨 **TÙY CHỈNH:**

### **Thả nhiều bom hơn:**
```
Bomb Interval: 0.3 (Thả nhanh hơn)
Max Bombs: 20 (Nhiều hơn)
```

### **Bom mạnh hơn:**
```
Bomb Prefab → B52 Bomb:
- Damage: 100 (Tăng damage)
- Explosion Radius: 5 (Tăng bán kính nổ)
```

### **Hiệu ứng to hơn:**
```
Explosion Effect Prefab → Explosion Effect:
- Explosion Scale: (3, 3, 1) (Lớn hơn)
- Lifetime: 2.5 (Lâu hơn)
```

### **Bombing chọn lọc (Tactical):**
```
Max Bombs: 3 (Chỉ 3 quả)
Bomb Interval: 1.5 (Thả chậm)
Explosion Radius: 5 (Nhưng mạnh!)
Damage: 150
```

---

## 🐛 **TROUBLESHOOTING:**

### **Không thả bom:**
- ✅ Check `Enable Bombing` đã bật chưa
- ✅ Check `Bomb Prefab` đã assign chưa
- ✅ Xem Console có log "Bomb dropped!" không

### **Bom không rơi:**
- ✅ Check Bomb prefab có Rigidbody2D không
- ✅ Check Fall Speed > 0
- ✅ Check Bomb có collider không

### **Bom không nổ:**
- ✅ Check Ground/Wall objects có tag "Ground" hoặc "Wall" chưa
- ✅ Check Bomb Collider `Is Trigger = true`
- ✅ Check Ground có Collider không

### **Không có hiệu ứng nổ:**
- ✅ Check `Explosion Effect Prefab` đã assign chưa
- ✅ Check prefab có script ExplosionEffect không

### **Không có âm thanh:**
- ✅ Check đã kéo audio clips vào chưa
- ✅ Check volume > 0
- ✅ Check AudioListener có trong scene không

---

## 💡 **TIPS & TRICKS:**

### **1. Bombing khi Boss xuất hiện:**

```csharp
// Trong BossController:
void OnBossSpawn()
{
    B52ShadowSimple b52 = FindObjectOfType<B52ShadowSimple>();
    if (b52 != null)
    {
        // Bật bombing
        // b52.EnableBombing(true);
        b52.TriggerImmediateFlight();
    }
}
```

### **2. Nhiều máy bay thả bom cùng lúc:**

```
Duplicate B52_Shadow 3 lần
Mỗi máy bay:
- Initial Delay khác nhau (5s, 10s, 15s)
- Enable Bombing: ✅
- Max Bombs: 5
```

### **3. Bom có màu khác nhau:**

```
Bomb_Fire  → Đỏ, Fire damage, DOT
Bomb_Ice   → Xanh, Slow enemies
Bomb_Napalm → Vàng, Area burn effect
```

### **4. Chain explosion (Nổ liên hoàn):**

```
Trong B52Bomb.cs, khi nổ:
- Spawn thêm 3-4 bom nhỏ xung quanh
- Chúng nổ sau 0.2s
- Tạo hiệu ứng nổ dây chuyền!
```

---

## 📊 **SETTINGS ĐỀ XUẤT:**

### **🎮 Normal Mode (Vừa phải):**
```
Enable Bombing: ✅
Bomb Interval: 0.5s
Max Bombs: 10
Damage: 50
Explosion Radius: 3
```

### **⚔️ Hard Mode (Nhiều bom!):**
```
Enable Bombing: ✅
Bomb Interval: 0.3s
Max Bombs: 20
Damage: 30
Explosion Radius: 2.5
```

### **💀 Boss Fight (Mạnh!):**
```
Enable Bombing: ✅
Bomb Interval: 1.0s
Max Bombs: 5
Damage: 150
Explosion Radius: 5
```

### **🎨 Cinematic (Đẹp!):**
```
Enable Bombing: ✅
Bomb Interval: 0.8s
Max Bombs: 8
Explosion Scale: (4, 4, 1) ← Lớn!
Explosion Duration: 3s ← Lâu!
```

---

## 🎬 **KẾT QUẢ MONG ĐỢI:**

```
1. ✈️ B52 bay qua khung hình
2. 💣 Thả bom mỗi 0.5s
3. 🔊 Nghe tiếng rít (whistle)
4. 🔥 Bom rơi xuống xoay tròn
5. 💥 NỔ khi chạm đất!
6. 🔊 Nghe tiếng nổ to!
7. 🎆 Hiệu ứng nổ đẹp mắt
8. ⚔️ Enemies bị damage!
9. 💀 Enemies chết hàng loạt!
```

**EPIC! 💣💥✈️**

---

## 📁 **FILES ĐÃ TẠO:**

1. ✅ `B52ShadowSimple.cs` - Đã cập nhật với bombing
2. ✅ `B52Bomb.cs` - Script điều khiển bom
3. ✅ `ExplosionEffect.cs` - Script hiệu ứng nổ
4. ✅ `Bomb_B52.prefab` - Prefab bom (cần tạo)
5. ✅ `Explosion_Effect.prefab` - Prefab hiệu ứng (cần tạo)

---

## 🚀 **NEXT STEPS:**

1. **Tìm/Tạo sprites** cho bom và explosion
2. **Tìm âm thanh** cho whistle và explosion
3. **Setup prefabs** theo hướng dẫn
4. **Test** và điều chỉnh
5. **ENJOY THE BOMBING! 💣**

---

**Chúc bạn thành công! Nếu cần giúp, hỏi tôi! 🚀✈️💥**

