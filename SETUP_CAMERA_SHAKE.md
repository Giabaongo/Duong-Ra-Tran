# 📳 SETUP CAMERA SHAKE CHO BOM B52

## ⚠️ **QUAN TRỌNG: KIỂM TRA UNITY VERSION!**

### **🎮 UNITY 6 (6000.x.x):**
→ **ĐỌC FILE: `SETUP_UNITY6_CINEMACHINE3.md`** ⬅️ **QUAN TRỌNG!**

Unity 6 dùng **Cinemachine 3.x** - hoàn toàn khác!
- Component: `CinemachineCamera` (không phải VirtualCamera)
- KHÔNG có nút "Add Extension"
- Setup khác hoàn toàn!

### **🎮 UNITY 2021-2023 (2021.x - 2023.x):**
→ Đọc hướng dẫn dưới đây ⬇️

---

## ✅ **ĐÃ TẠO XONG!**

Đã có 2 thay đổi:
1. ✅ **Tiếng nổ tăng: 50 → 80** (to hơn 60%!)
2. ✅ **Camera Shake script** đã được tạo
3. ✅ **Support cả Unity 6 và Unity 2021-2023**

---

## 🎯 **CÁCH SETUP (2 BƯỚC ĐƠN GIẢN):**

### **⚠️ CHO UNITY 2021-2023 ONLY!**

### **BƯỚC 1: Thêm CameraShake vào Camera**

**UNITY 2021-2023:** Bạn có **Cinemachine Virtual Camera** (tracking player), làm theo:

#### **Option A: Nếu dùng Cinemachine (khuyến nghị):**

1. **Mở scene `MauthanScene`**
2. **Tìm Cinemachine Virtual Camera** trong Hierarchy (tên có thể là `CM vcam1` hoặc `VirtualCamera`)
3. **Chọn Virtual Camera đó**
4. **Add Component → Search `CameraShake`**
5. **Cấu hình:**
   ```
   CameraShake component:
   ├─ Max Shake Magnitude: 0.5
   ├─ Default Duration: 0.3
   ├─ Damping Speed: 1.0
   └─ Use Cinemachine: ☑️ CHECK THIS! (QUAN TRỌNG!)
   ```

6. **Thêm Noise vào Virtual Camera:**
   - Chọn Virtual Camera
   - Click **Add Extension** (ở Inspector)
   - Chọn **CinemachineBasicMultiChannelPerlin**
   - Để Amplitude Gain = 0, Frequency Gain = 0 (sẽ tự động set khi bom nổ)

---

#### **Option B: Nếu dùng Main Camera thường:**

1. **Hierarchy → Main Camera**
2. **Add Component → Search `CameraShake`**
3. **Cấu hình:**
   ```
   CameraShake component:
   ├─ Max Shake Magnitude: 0.5
   ├─ Default Duration: 0.3
   ├─ Damping Speed: 1.0
   └─ Use Cinemachine: ☐ UNCHECK!
   ```

---

### **BƯỚC 2: Kiểm tra B52_Bomb prefab**

1. **Project → Assets/Prefabs/Effects → B52_Bomb**
2. **Chọn prefab**
3. **Xem component `B52 Bomb`:**
   ```
   📳 CAMERA SHAKE (MỚI!):
   ├─ Shake Intensity: 1.2 (có thể tăng lên 2.0 cho rung mạnh hơn!)
   └─ Shake Max Distance: 25
   ```

---

## 🎮 **TEST NGAY:**

1. **Play** scene
2. Chờ **B52 bay qua** (10 giây)
3. Khi **bom nổ**, bạn sẽ thấy:
   - 💥 **TIẾNG NỔ TO GẤP 1.6 LẦN!** (80 thay vì 50)
   - 📳 **CAMERA RUNG!** (shake effect)
   - 🎥 **Khung hình rung lắc theo explosion!**

---

## 📊 **THÔNG SỐ CAMERA SHAKE:**

### **Trong CameraShake component:**

| Tham số | Mặc định | Giải thích |
|---------|----------|------------|
| **Max Shake Magnitude** | 0.5 | Độ rung tối đa (0-1, càng cao càng mạnh) |
| **Default Duration** | 0.3 | Thời gian rung mặc định (giây) |
| **Damping Speed** | 1.0 | Tốc độ giảm dần (1-3) |
| **Use Cinemachine** | Tùy | ☑️ Nếu dùng Cinemachine |

### **Trong B52_Bomb prefab:**

| Tham số | Mặc định | Giải thích |
|---------|----------|------------|
| **Shake Intensity** | 1.2 | Cường độ rung (0-2) |
| **Shake Max Distance** | 25 | Khoảng cách tối đa còn rung (units) |

---

## 🔧 **TÙY CHỈNH:**

### **Muốn rung MẠNH hơn:**

```
B52_Bomb prefab:
→ Shake Intensity: 1.2 → 2.0
→ Shake Max Distance: 25 → 40

CameraShake component:
→ Max Shake Magnitude: 0.5 → 0.8
```

### **Muốn rung LÂU hơn:**

```
CameraShake component:
→ Default Duration: 0.3 → 0.5
→ Damping Speed: 1.0 → 0.5
```

### **Muốn rung ÍT hơn (giảm motion sickness):**

```
B52_Bomb prefab:
→ Shake Intensity: 1.2 → 0.5

CameraShake component:
→ Max Shake Magnitude: 0.5 → 0.3
```

---

## 🎯 **CÁCH HOẠT ĐỘNG:**

```
BOM NỔ → B52Bomb.ShakeCamera()
           ↓
       Tìm Camera
           ↓
   Tìm CameraShake component
           ↓
   Tính khoảng cách explosion → camera
           ↓
   Giảm intensity theo khoảng cách
           ↓
   GỌI: cameraShake.ShakeFromExplosion()
           ↓
       📳 CAMERA RUNG!
```

---

## ⚠️ **TROUBLESHOOTING:**

### **❌ Camera không rung:**

**Log hiển thị:**
```
[B52Bomb] ⚠️ CameraShake component not found!
```

**Giải pháp:**
1. Kiểm tra đã **Add Component CameraShake** vào camera chưa
2. Nếu dùng Cinemachine, thêm vào **Virtual Camera**, không phải Main Camera
3. Check **Use Cinemachine** trong CameraShake component

---

### **❌ Rung quá yếu:**

**Giải pháp:**
```
Tăng các thông số:
- B52_Bomb → Shake Intensity: 2.0
- CameraShake → Max Shake Magnitude: 0.8
```

---

### **❌ Rung cả khi bom nổ xa:**

**Giải pháp:**
```
Giảm:
- B52_Bomb → Shake Max Distance: 25 → 15
```

---

## 📝 **SUMMARY:**

### **Đã tạo:**
- ✅ `CameraShake.cs` - Script rung camera
- ✅ Tích hợp vào `B52Bomb.cs`
- ✅ Support cả Cinemachine và Regular Camera
- ✅ Tăng tiếng nổ: 50 → **80**

### **Cần làm:**
1. ⬜ Add `CameraShake` component vào camera
2. ⬜ Nếu dùng Cinemachine: Check "Use Cinemachine"
3. ⬜ Nếu dùng Cinemachine: Add Noise extension
4. ⬜ Test và tùy chỉnh intensity

---

## 🎥 **CINEMACHINE SETUP (CHI TIẾT):**

Nếu bạn dùng Cinemachine Virtual Camera (tracking player):

### **Bước 1: Add CameraShake vào Virtual Camera**
```
Hierarchy → CM vcam1 (hoặc VirtualCamera)
→ Add Component → CameraShake
→ Use Cinemachine: ☑️
```

### **Bước 2: Add Noise Extension**
```
Chọn Virtual Camera
→ Inspector → Add Extension
→ CinemachineBasicMultiChannelPerlin
→ Amplitude Gain: 0
→ Frequency Gain: 0
```

### **Bước 3: Chọn Noise Profile**
```
Basic Multi Channel Perlin component:
→ Noise Profile: "6D Shake" (có sẵn trong Cinemachine)
```

**XONG!** Khi bom nổ, `CameraShake` sẽ tự động set `AmplitudeGain` và `FrequencyGain`!

---

## 💡 **TIPS:**

### **Realistic Shake:**
```
Shake Intensity: 1.0-1.5
Max Shake Magnitude: 0.3-0.5
Duration: 0.2-0.3s
→ Rung ngắn, mạnh, giống thực tế
```

### **Cinematic Shake:**
```
Shake Intensity: 0.5-0.8
Max Shake Magnitude: 0.4-0.6
Duration: 0.4-0.6s
→ Rung lâu, mượt, điện ảnh
```

### **Arcade Shake:**
```
Shake Intensity: 1.5-2.0
Max Shake Magnitude: 0.6-1.0
Duration: 0.3-0.4s
→ Rung mạnh, sốc, arcade-style
```

---

**🎯 SETUP XONG LÀ XONG! BOM NỔ SẼ RUNG CẢ KHUNG HÌNH! 📳💥**

**Logs sẽ hiển thị:**
```
[B52Bomb] 💥 EXPLOSION at (-10, -5, 0)!
[CameraShake] 💥 Explosion at 8.5m, Intensity: 0.85
[B52Bomb] 📳 Camera shake triggered! Intensity: 1.2
```

