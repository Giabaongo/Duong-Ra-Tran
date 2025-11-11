# 🔧 SỬA LỖI: MÁY BAY KHÔNG XOAY ĐÚNG HƯỚNG

## ❌ **VẤN ĐỀ:**

Máy bay bay theo hướng xiên nhưng mũi máy bay không chỉ đúng hướng bay.

**Nguyên nhân:** Sprite B52 ban đầu có thể hướng **LÊN TRÊN** (90°) thay vì **SANG PHẢI** (0°)

---

## ✅ **GIẢI PHÁP NHANH (30 GIÂY):**

### **BƯỚC 1: Điều chỉnh Rotation Offset**

1. Chọn GameObject `B52_Shadow` trong Hierarchy
2. Component **B52 Shadow Simple**
3. Section **🎨 Hiệu Ứng Hình Ảnh**
4. Tìm field **Rotation Offset** (mới thêm)
5. **Thử các giá trị sau:**

```
Rotation Offset = -90  ← Nếu sprite hướng LÊN (thường gặp) ✅
Rotation Offset = 0    ← Nếu sprite hướng PHẢI
Rotation Offset = 90   ← Nếu sprite hướng XUỐNG
Rotation Offset = 180  ← Nếu sprite hướng TRÁI
```

6. **Click Play** và xem máy bay bay
7. **Điều chỉnh giá trị** cho đến khi mũi máy bay chỉ đúng hướng!

---

## 🎯 **CÁCH KIỂM TRA HƯỚNG SPRITE GỐC:**

### **Trong Unity Editor:**

1. Chọn GameObject `B52_Shadow`
2. **Transform** → **Rotation Z**: Set về `0` (tạm thời)
3. Nhìn sprite trong **Scene view**:

```
Nếu đầu máy bay hướng:
→ (Phải)  = Rotation Offset = 0
↑ (Trên)  = Rotation Offset = -90  ✅ Thường gặp nhất!
← (Trái)  = Rotation Offset = 180
↓ (Dưới) = Rotation Offset = 90
```

4. Sau khi xác định, set **Rotation Offset** đúng

---

## 📊 **VÍ DỤ THỰC TẾ:**

### **Trường hợp 1: Sprite hướng LÊN (90°)** ✅ Thường gặp

```
Sprite gốc:
    ✈️
    ↑

Cần bay SANG PHẢI →
→ Cần xoay -90°
→ Rotation Offset = -90

Kết quả:
    →✈️ (Đúng!)
```

### **Trường hợp 2: Sprite hướng PHẢI (0°)**

```
Sprite gốc:
    →✈️

Cần bay SANG PHẢI →
→ Không cần xoay
→ Rotation Offset = 0

Kết quả:
    →✈️ (Đúng!)
```

---

## 🔍 **DEBUG LOGS MỚI:**

Sau khi cập nhật, Console sẽ hiển thị:

```
[B52Shadow] 📍 Rotation: 83.8° (Base: 173.8° + Offset: -90.0°)
                          ↑           ↑              ↑
                    Góc cuối  Góc tính toán  Offset bạn set
```

**Để kiểm tra:**
- Xem máy bay bay theo hướng nào
- Xem góc cuối (Rotation) có hợp lý không

**Ví dụ:**
```
Bay PHẢI →     : Rotation ≈ 0° (hoặc -90° nếu offset = -90)
Bay TRÁI ←     : Rotation ≈ 180° (hoặc 90° nếu offset = -90)
Bay LÊN ↑      : Rotation ≈ 90° (hoặc 0° nếu offset = -90)
Bay XUỐNG ↓    : Rotation ≈ -90° (hoặc -180° nếu offset = -90)
```

---

## 🎮 **HƯỚNG DẪN TỪ TRÙNG BƯỚC:**

### **Bước 1: Reset về mặc định**
```
Chọn B52_Shadow → B52 Shadow Simple:
- Rotation Offset: -90  ← Bắt đầu với giá trị này
```

### **Bước 2: Test**
```
Click Play ▶️
Đợi máy bay bay qua
```

### **Bước 3: Quan sát**
```
Máy bay bay PHẢI → Nhưng mũi hướng:
- ↑ LÊN?    → Rotation Offset = -90 ✅
- ↓ XUỐNG?  → Rotation Offset = 90
- ← TRÁI?   → Rotation Offset = 180
- → ĐÚNG!   → Rotation Offset = 0
```

### **Bước 4: Điều chỉnh**
```
Dừng game
Chỉnh lại Rotation Offset
Play lại
Lặp lại cho đến khi đúng!
```

---

## 🚀 **GIẢI PHÁP CUỐI CÙNG (Nếu vẫn không đúng):**

### **Cách 1: Xoay sprite trong Photoshop/GIMP**

Nếu sprite luôn xoay sai, tốt nhất là **xoay sprite gốc**:

1. Mở sprite `B52_Shadow.png` trong Photoshop/GIMP
2. **Image → Rotate → 90° Clockwise** (hoặc theo nhu cầu)
3. **Đảm bảo đầu máy bay hướng SANG PHẢI** →
4. Save lại
5. Trong Unity, set **Rotation Offset = 0**

### **Cách 2: Flip trong Unity**

Nếu máy bay bay ngược:

```
Component: Sprite Renderer
→ Flip: 
   - Flip X: ☐/✅ (Thử check/uncheck)
   - Flip Y: ☐/✅ (Thử check/uncheck)
```

---

## 💡 **TIPS:**

### **Điều chỉnh trực tiếp khi Play:**

1. **Click Play** ▶️
2. Chọn `B52_Shadow` trong Hierarchy (khi đang Play)
3. Kéo **Rotation Offset** slider trong Inspector
4. **THẤY NGAY** máy bay xoay real-time!
5. Khi tìm được giá trị đúng → **NHỚ GHI LẠI**
6. Stop game
7. Set lại giá trị đúng vào Inspector
8. Play lại để confirm

---

## 📐 **CÔNG THỨC TOÁN HỌC:**

```
Góc cuối = Góc bay + Rotation Offset

Ví dụ:
- Bay phải (0°) + Offset (-90°) = -90° → Mũi hướng xuống
- Bay phải (0°) + Offset (0°)   = 0°   → Mũi hướng phải ✅
- Bay lên (90°) + Offset (-90°) = 0°   → Mũi hướng phải ✅
```

---

## ✅ **KẾT QUẢ MONG ĐỢI:**

Sau khi set đúng **Rotation Offset**:

```
Máy bay bay: Phải →     Mũi chỉ: →
Máy bay bay: Trái ←     Mũi chỉ: ←
Máy bay bay: Lên ↑      Mũi chỉ: ↑
Máy bay bay: Xuống ↓    Mũi chỉ: ↓
Máy bay bay: Xiên ↗     Mũi chỉ: ↗
```

**HOÀN HẢO! ✈️**

---

## 📝 **CHECKLIST:**

- [ ] Đã set Rotation Offset (thử -90 trước)
- [ ] Click Play và test
- [ ] Mũi máy bay chỉ ĐÚNG hướng bay
- [ ] Máy bay không bị xoay lộn xộn
- [ ] Console có log "Rotation: X° (Base: Y° + Offset: Z°)"

---

**Nếu đã làm tất cả mà vẫn không đúng, screenshot và paste logs, tôi sẽ giúp tiếp! 🚀**

