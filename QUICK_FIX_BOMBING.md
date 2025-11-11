# ⚡ QUICK FIX - BOMBING KHÔNG HOẠT ĐỘNG

**Thời gian:** 2 phút  
**Fix:** Bật bombing trong B52_Shadow prefab

---

## 🔧 **CẦN LÀM NGAY (2 phút):**

### **BƯỚC 1: Cấu hình B52_Shadow Prefab**

1. **Project** → Tìm `Assets/Prefabs/Effects/B52_Shadow.prefab`

2. **Double click** để mở prefab

3. Component **B52 Shadow Simple** → Scroll xuống section **💣 BOMBING**:

```
✅ Enable Bombing: CHECK VÀO ĐÂY! ⬅️ QUAN TRỌNG!

Bomb Prefab: Kéo B52_Bomb.prefab vào ⬅️ QUAN TRỌNG!
   (Tìm trong Assets/Prefabs/Effects/B52_Bomb.prefab)

✅ Drop All At Once: CHECK VÀO ĐÂY! (Thả 4 bom cùng lúc)

Bomb Spacing: 1.5 (Khoảng cách giữa các bom)

Bomb Count: 4 (Số lượng bom)

Bomb Drop Sound: (Để trống - mỗi bom có âm thanh riêng)
```

4. **Save** (Ctrl + S)

---

### **BƯỚC 2: Cấu hình B52_Bomb Prefab (Âm thanh)**

1. **Project** → Tìm `Assets/Prefabs/Effects/B52_Bomb.prefab`

2. **Double click** để mở prefab

3. Component **B52 Bomb** → Section **🔊 ÂM THANH**:

```
Falling Sound: Kéo bom_falling2.mp3 vào ⬅️ MỚI!
   (Tìm trong Assets/Audio/Music/bom_falling2.mp3)

Explosion Sound: Kéo bomb_explode_sound.mp3 vào
   (Tìm trong Assets/Audio/Music/bomb_explode_sound.mp3)

Explosion Volume: 0.8
```

4. **Save** (Ctrl + S)

---

### **BƯỚC 3: Test**

1. **Click Play** ▶️

2. Đợi B52 bay qua (5 giây)

3. Quan sát:
   - ✅ Khi B52 đến giữa hành trình
   - 💣💣💣💣 **4 BOM RƠI CÙNG LÚC!**
   - 🔊 Nghe tiếng rít từng bom
   - 💥💥💥💥 Tất cả nổ khi chạm đất!
   - 🔊 Nghe tiếng nổ to!

4. Check **Console**:
   ```
   [B52Shadow] 💥 SALVO! Dropped 4 bombs at once!
   [Bomb] 🔊 Playing falling sound!
   [Bomb] 💥 EXPLOSION at (x, y)!
   ```

---

## 🎯 **GIẢI THÍCH THAY ĐỔI:**

### **✅ ĐÃ FIXED:**

1. **Enable Bombing = true** → Bật tính năng thả bom

2. **Bomb Prefab assigned** → B52 biết thả prefab nào

3. **Drop All At Once = true** → Thả 4 bom cùng lúc thay vì từng quả

4. **Falling Sound** → Mỗi bom phát âm thanh rơi riêng

5. **Explosion Sound** → Âm thanh nổ

---

## 🎨 **TÙY CHỈNH:**

### **Nhiều bom hơn:**
```
Bomb Count: 8 (Thả 8 quả!)
Bomb Spacing: 1.0 (Gần nhau hơn)
```

### **Bom cách xa nhau:**
```
Bomb Spacing: 2.5 (Xa hơn)
```

### **Thả từng quả (carpet bombing):**
```
✅ Drop All At Once: UNCHECK
Bomb Interval: 0.3 (Thả mỗi 0.3 giây)
```

### **Damage mạnh hơn:**
```
Trong B52_Bomb.prefab:
- Damage: 50 (Tăng lên từ 5)
- Explosion Radius: 5 (Tăng bán kính)
```

---

## 📊 **KẾT QUẢ MONG ĐỢI:**

```
1. ✈️ B52 bay qua camera
2. 🎵 Nghe tiếng máy bay B52
3. 💣💣💣💣 Thả 4 bom cùng lúc (giữa hành trình)
4. 🔊🔊🔊🔊 Nghe 4 tiếng rít cùng lúc
5. 🔥🔥🔥🔥 4 bom rơi xuống xoay tròn
6. 💥💥💥💥 NỔ khi chạm đất!
7. 🔊🔊🔊🔊 Tiếng nổ đồng loạt!
8. 🎆🎆🎆🎆 4 hiệu ứng nổ đẹp mắt
9. ⚔️⚔️⚔️⚔️ Enemies chết hàng loạt!
```

**SPECTACULAR! 💣💥✈️🔥**

---

## 🐛 **NẾU VẪN KHÔNG HOẠT ĐỘNG:**

### **Không thả bom:**
- ✅ Check `Enable Bombing` = true
- ✅ Check `Bomb Prefab` đã kéo vào chưa
- ✅ Check B52_Bomb.prefab tồn tại không

### **Không có âm thanh rơi:**
- ✅ Check `Falling Sound` đã assign chưa
- ✅ Check file bom_falling2.mp3 có trong Assets/Audio/Music/
- ✅ Check volume > 0

### **Không có âm thanh nổ:**
- ✅ Check `Explosion Sound` đã assign
- ✅ Check file bomb_explode_sound.mp3 có

### **Bom không nổ:**
- ✅ Check Ground/Wall có tag "Ground" hoặc collider
- ✅ Check Bomb có CircleCollider2D (Is Trigger = true)

---

## 💡 **BONUS: Multiple B52 Bombing!**

Muốn nhiều máy bay thả bom cùng lúc?

1. Duplicate B52_Shadow trong scene → Tên: `B52_Shadow_2`

2. Cấu hình:
   ```
   Initial Delay: 10 (Delay khác với B52 đầu tiên)
   Enable Bombing: ✅
   Bomb Count: 4
   ```

3. Kết quả: **2 máy bay bay qua, mỗi máy thả 4 bom!**

**DOUBLE BOMBING! 💣💣💣💣💣💣💣💣**

---

**LÀM THEO 2 BƯỚC TRÊN VÀ ENJOY! 🚀💥**

