# 🎨 HƯỚNG DẪN SETUP EXPLOSION SPRITE SHEET

**Thời gian:** 5 phút  
**Độ khó:** ⭐ Dễ

---

## 📥 **BƯỚC 1: IMPORT SPRITE SHEET (2 phút)**

### **A. Import vào Unity:**

1. **Save ảnh explosion** → Đặt tên: `Explosion_Sheet.png`

2. **Kéo file vào Unity:**
   ```
   Assets/Sprites/Effects/Explosion_Sheet.png
   ```

3. **Chọn sprite trong Project** → Inspector:

4. **Texture Type:** `Sprite (2D and UI)`

5. **Sprite Mode:** `Multiple` ⬅️ QUAN TRỌNG!

6. **Pixels Per Unit:** 100

7. **Filter Mode:** Point (no filter) - Cho pixel art sharp

8. **Compression:** None

9. **Click Apply** ✅

---

## ✂️ **BƯỚC 2: CẮT SPRITE THÀNH CÁC FRAMES (3 phút)**

### **A. Mở Sprite Editor:**

1. Chọn `Explosion_Sheet.png` trong Project

2. Click button **"Sprite Editor"** trong Inspector

3. Trong Sprite Editor window:

### **B. Automatic Slicing (Cách dễ nhất):**

1. Click menu **"Slice"** ở trên cùng

2. Chọn **Type: Automatic**

3. Click **"Slice"** button

4. Unity sẽ tự động phát hiện và cắt từng frame!

5. Click **"Apply"** ở góc trên phải

6. **Đóng** Sprite Editor

### **C. Kiểm tra:**

1. Click mũi tên **"▶"** bên cạnh `Explosion_Sheet.png` trong Project

2. Bạn sẽ thấy nhiều sprites con:
   ```
   ▼ Explosion_Sheet
      ├─ Explosion_Sheet_0 (spark nhỏ)
      ├─ Explosion_Sheet_1 
      ├─ Explosion_Sheet_2
      ├─ Explosion_Sheet_3 (nổ lớn)
      ├─ Explosion_Sheet_4
      ├─ Explosion_Sheet_5
      ├─ Explosion_Sheet_6 (khói)
      └─ Explosion_Sheet_7 (tan dần)
   ```

✅ **Done!** Giờ bạn có 7-8 explosion frames riêng biệt!

---

## 🎬 **BƯỚC 3: TẠO EXPLOSION EFFECT PREFAB**

### **A. Tạo GameObject:**

1. **Hierarchy** → Right click → **Create Empty**

2. Tên: `Explosion_Effect`

3. **Add Component** → **Sprite Renderer**
   ```
   Sprite: Kéo Explosion_Sheet_0 (frame đầu) vào
   Color: White (255, 255, 255, 255)
   Sorting Layer: Default
   Order in Layer: 100 (Hiện trên cùng)
   ```

### **B. Add Script:**

4. **Add Component** → **Explosion Effect** (script đã có)
   ```
   ⚙️ Settings:
   - Lifetime: 1.0 (1 giây - vừa đủ)
   - Explosion Scale: (2, 2, 1) hoặc (3, 3, 1) tùy thích
   - Fade Out: ✅ Checked
   
   🎨 Sprite Animation:
   - Explosion Sprites: KÉO TẤT CẢ 7-8 FRAMES VÀO ĐÂY!
     (Chọn tất cả từ Explosion_Sheet_0 đến _7)
   - Animation FPS: 12 (chậm) hoặc 20 (nhanh)
   ```

### **C. Kéo các frames vào Explosion Sprites:**

**QUAN TRỌNG - Làm đúng thứ tự:**

1. Click vào field **"Explosion Sprites"**

2. Trong popup, set **Size = 8** (hoặc số frame bạn có)

3. Kéo **LẦN LƯỢT** từng frame vào:
   ```
   Element 0: Explosion_Sheet_0
   Element 1: Explosion_Sheet_1
   Element 2: Explosion_Sheet_2
   Element 3: Explosion_Sheet_3
   Element 4: Explosion_Sheet_4
   Element 5: Explosion_Sheet_5
   Element 6: Explosion_Sheet_6
   Element 7: Explosion_Sheet_7
   ```

   **HOẶC:** Chọn tất cả 8 sprites → Kéo vào cùng lúc

### **D. Save as Prefab:**

1. Kéo `Explosion_Effect` từ Hierarchy vào:
   ```
   Assets/Prefabs/Effects/Explosion_Effect.prefab
   ```

2. **XÓA** GameObject khỏi Hierarchy (giữ prefab)

✅ **Done!** Explosion Effect Prefab hoàn chỉnh!

---

## 🎮 **BƯỚC 4: CONNECT VỚI BOM**

### **Nếu chưa tạo Bomb Prefab:**

Làm theo `SETUP_BOMBING.md` BƯỚC 1 để tạo Bomb_B52

### **Nếu đã có Bomb Prefab:**

1. **Project** → Tìm `Bomb_B52.prefab`

2. **Double click** để mở prefab

3. Component **B52 Bomb**:
   ```
   🎨 Hiệu Ứng:
   - Explosion Effect Prefab: Kéo Explosion_Effect.prefab vào ⬅️
   - Explosion Effect Duration: 1.0 (match với lifetime)
   ```

4. **Save** (Ctrl + S)

✅ **Done!** Giờ bom sẽ nổ với animation đẹp!

---

## 🔧 **BƯỚC 5 (OPTIONAL): TẠO BOMB SPRITE**

Trong sprite sheet có vẻ có cả bomb sprite (quả nhỏ đầu tiên bên trái)

### **Dùng cho Bomb:**

1. Tìm frame nhỏ nhất (Explosion_Sheet_0 - spark)

2. Mở `Bomb_B52.prefab`

3. Component **Sprite Renderer**:
   ```
   Sprite: Kéo Explosion_Sheet_0 vào
   Color: Yellow hoặc Red (để highlight)
   Sorting Order: 10
   ```

4. Component **B52 Bomb**:
   ```
   Bomb Sprite: Kéo Explosion_Sheet_0 vào
   ```

---

## 🎮 **TEST (30 giây)**

1. **Play** ▶️

2. Đợi B52 thả bom

3. Quan sát:
   - ✅ Bom rơi
   - ✅ Chạm đất
   - ✅ Animation nổ đẹp mắt! 💥
   - ✅ Khói tan dần

---

## 🎨 **TÙY CHỈNH ANIMATION:**

### **Nhanh hơn:**
```
Explosion Effect → Animation FPS: 24
Lifetime: 0.8
```

### **Chậm hơn (dramatic):**
```
Animation FPS: 10
Lifetime: 1.5
```

### **Lớn hơn:**
```
Explosion Scale: (4, 4, 1)
```

### **Màu sắc:**
```
Sprite Renderer → Color:
- Đỏ: (255, 100, 100, 255) - Fire
- Cam: (255, 200, 100, 255) - Normal
- Xanh: (100, 200, 255, 255) - Ice bomb
```

---

## 🐛 **TROUBLESHOOTING:**

### **Không thấy animation:**
- ✅ Check đã kéo tất cả frames vào `Explosion Sprites` chưa
- ✅ Check `Animation FPS` > 0
- ✅ Check `Lifetime` đủ dài để animation chạy hết

### **Animation quá nhanh/chậm:**
- ✅ Điều chỉnh `Animation FPS`
- ✅ Điều chỉnh `Lifetime`
- Công thức: `Lifetime ≥ Number of Frames / FPS`
  - VD: 8 frames / 20 FPS = 0.4s minimum

### **Explosion không hiện:**
- ✅ Check `Order in Layer` = 100 (cao)
- ✅ Check `Explosion Effect Prefab` đã assign vào Bomb chưa
- ✅ Check console có errors không

### **Sprite bị mờ/blur:**
- ✅ Chọn Explosion_Sheet.png
- ✅ Filter Mode: Point (no filter)
- ✅ Compression: None
- ✅ Click Apply

---

## 📊 **KẾT QUẢ MONG ĐỢI:**

```
1. Bom rơi xuống xoay tròn
2. Chạm đất
3. 💥 NỔ! 
4. Animation chạy qua 8 frames:
   ⚡ Spark → 🔥 Fire → 💥 Big Explosion → 💨 Smoke
5. Fade out mượt
6. Tự động xóa
```

**PERFECT! 🎆💥✨**

---

## 💡 **BONUS TIPS:**

### **1. Nhiều màu explosion:**

```
Duplicate Explosion_Effect.prefab:
- Explosion_Fire → Màu đỏ
- Explosion_Ice → Màu xanh  
- Explosion_Lightning → Màu tím

Dùng cho different bomb types!
```

### **2. Add sound cùng animation:**

Trong `ExplosionEffect.cs`, có thể thêm:
- Frame 2-3: Phát tiếng nổ
- Frame 5-6: Tiếng lửa cháy

### **3. Screen shake timing:**

Nổ lúc frame 3-4 (explosion lớn nhất)

---

## ✅ **CHECKLIST:**

- [ ] Import Explosion_Sheet.png
- [ ] Sprite Mode: Multiple
- [ ] Slice thành frames (Automatic)
- [ ] Tạo Explosion_Effect GameObject
- [ ] Add Sprite Renderer + ExplosionEffect script
- [ ] Kéo 7-8 frames vào Explosion Sprites
- [ ] Save as Prefab
- [ ] Connect vào Bomb_B52 prefab
- [ ] Test!
- [ ] Enjoy epic explosions! 💣💥

---

**Sprite sheet này PERFECT cho game! Chúc bạn thành công! 🚀💥**

