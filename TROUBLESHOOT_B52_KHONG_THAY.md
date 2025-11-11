# 🐛 SỬA LỖI: KHÔNG THẤY MÁY BAY B52

## ✅ **SCRIPT ĐÃ CHẠY NHƯNG KHÔNG THẤY MÁY BAY?**

Nếu bạn thấy logs này trong Console:
```
[B52Shadow] ✈️ Chuyến bay #1 bắt đầu!
[B52Shadow] ✅ Chuyến bay #1 hoàn thành!
```

**→ Script ĐANG HOẠT ĐỘNG!** Nhưng sprite không hiển thị.

---

## 🔍 **KIỂM TRA TỪNG BƯỚC:**

### ❌ **VẤN ĐỀ 1: CHƯA CÓ SPRITE** (99% là lý do này!)

**Triệu chứng:**
```
Console có log:
[B52Shadow] ❌❌❌ CHƯA CÓ SPRITE!
```

**Giải pháp:**
1. Chọn GameObject `B52_Shadow` trong Hierarchy
2. Component **Sprite Renderer**
3. Field **Sprite** đang TRỐNG? ← ĐÂY LÀ VẤN ĐỀ!
4. **KÉO** sprite `B52_Shadow.png` từ Project vào field này!

```
Assets/Sprites/Effects/B52_Shadow.png
      ↓ KÉO VÀO
Sprite Renderer → Sprite: [ B52_Shadow.png ]
```

5. **Click Play** lại → Thấy máy bay ngay! ✅

---

### ❌ **VẤN ĐỀ 2: SORTING LAYER SAI** (Bị che khuất)

**Triệu chứng:**
```
Console có log:
[B52Shadow] ⚠️ Order in Layer < 50 - Có thể bị che!
```

**Giải pháp:**
1. Chọn `B52_Shadow`
2. Component **Sprite Renderer**
3. **Order in Layer**: Đổi thành `100` ← SỐ LỚN
4. Click Apply

**Tại sao?** Số càng lớn = hiển thị càng trên!

---

### ❌ **VẤN ĐỀ 3: SPRITE QUÁ NHỎ**

**Triệu chứng:**
- Không có log error
- Nhưng vẫn không thấy

**Giải pháp:**
1. Chọn `B52_Shadow`
2. Component **B52 Shadow Simple**
3. Section **🎨 Hiệu Ứng Hình Ảnh**
4. **Sprite Scale**: Đổi thành `(5, 5, 1)` ← PHÓNG TO LÊN!
5. Click Play lại

---

### ❌ **VẤN ĐỀ 4: OPACITY QUÁ THẤP**

**Triệu chứng:**
```
Console có log:
[B52Shadow] 🎨 Color: ..., Opacity: 0.1
```

**Giải pháp:**
1. Component **B52 Shadow Simple**
2. **Shadow Opacity**: Đổi thành `1.0` ← ĐẬM NHẤT
3. **Enable Fade**: UNCHECK (tắt fade tạm thời)
4. Click Play lại

---

### ❌ **VẤN ĐỀ 5: CAMERA Z-POSITION**

**Triệu chứng:**
- Camera ở z = -10
- Máy bay ở z = 0
- Không thấy máy bay

**Giải pháp:**
1. Chọn `B52_Shadow`
2. **Transform** → **Position Z**: Đổi thành `-9` hoặc `-5`
3. (Phải trước camera một chút)

---

## 🚀 **GIẢI PHÁP NHANH - TEST NGAY:**

### **Cách 1: Force Visibility (Test thử)**

1. Chọn `B52_Shadow` trong Hierarchy
2. Component **Sprite Renderer**:
   ```
   Sprite: B52_Shadow.png ← Kéo vào
   Color: TRẮNG (R255, G255, B255, A255)
   Sorting Layer: Default
   Order in Layer: 200 ← SỐ RẤT LỚN
   ```

3. Component **B52 Shadow Simple**:
   ```
   Shadow Opacity: 1.0 ← MAX
   Enable Fade: ☐ Uncheck
   Sprite Scale: (5, 5, 1) ← LỚN
   ```

4. **Transform**:
   ```
   Position: (0, 0, -5) ← Trước camera
   Scale: (5, 5, 1)
   ```

5. **Click Play** → Nhìn vào giữa màn hình → PHẢI THẤY!

---

### **Cách 2: Debug Visual (Xem trong Scene View)**

1. **Click Play** ▶️
2. Chuyển sang tab **Scene** (không phải Game)
3. Trong Scene view:
   - Có thấy **khung cyan** (camera viewport)?
   - Có thấy **máy bay màu cyan** bay qua?
   - Có thấy **đường vàng** (flight path)?

**Nếu THẤY trong Scene nhưng KHÔNG THẤY trong Game:**
→ Vấn đề là Sorting Layer hoặc Camera culling!

**Nếu KHÔNG THẤY cả trong Scene:**
→ Vấn đề là không có sprite!

---

## 📊 **CHECKLIST ĐẦY ĐỦ:**

Đi qua từng mục này:

- [ ] ✅ GameObject `B52_Shadow` có trong Hierarchy
- [ ] ✅ Component `Sprite Renderer` có attached
- [ ] ✅ Field `Sprite` ĐÃ CÓ hình B52_Shadow.png
- [ ] ✅ `Order in Layer` >= 100
- [ ] ✅ `Shadow Opacity` = 0.6 đến 1.0
- [ ] ✅ `Sprite Scale` = (2, 2, 1) trở lên
- [ ] ✅ GameObject không bị disable
- [ ] ✅ Console có logs "Chuyến bay #1 bắt đầu"
- [ ] ✅ Console KHÔNG có log "❌ CHƯA CÓ SPRITE"

**Nếu tất cả đều ✅ mà vẫn không thấy:**
→ Đọc tiếp phần Debug Advanced bên dưới

---

## 🔧 **DEBUG ADVANCED:**

### **Xem Logs Chi Tiết:**

Khi Click Play, Console PHẢI có những logs này:

```
[B52Shadow] ✅ Sprite loaded: B52_Shadow  ← PHẢI CÓ!
[B52Shadow] ✅ Scale set to: (2, 2, 1)
[B52Shadow] Sorting Layer: 'Default', Order: 100
[B52Shadow] ✅ Camera found: Main Camera
[B52Shadow] ✈️ Sẽ bay lần đầu sau 5s...

... 5 giây sau ...

[B52Shadow] 📷 Camera-relative path: Direction 0, Camera at (x, y, z)
[B52Shadow] 🎨 Sprite enabled: True, Has sprite: True  ← PHẢI True!
[B52Shadow] 📍 Position: (x, y), Scale: (2, 2, 1)
[B52Shadow] 🎨 Color: ..., Opacity: 0.6  ← Phải > 0!
[B52Shadow] ✈️ Chuyến bay #1 bắt đầu!
```

**Nếu thiếu log nào → ĐÓ LÀ VẤN ĐỀ!**

---

### **Test Thủ Công (Manual Test):**

1. **Click Play** ▶️
2. Chọn `B52_Shadow` trong Hierarchy (khi đang Play)
3. **Component B52 Shadow Simple**
4. **Right click** → **Bay Ngay** (Context Menu)
5. Máy bay sẽ bay NGAY LẬP TỨC!
6. Nếu vẫn không thấy → Chắc chắn là vấn đề Sprite!

---

### **Test với Sprite Khác:**

Nếu vẫn không thấy, test với sprite BIẾT CHẮC hoạt động:

1. Tìm sprite BẤT KỲ trong Project (VD: Player sprite)
2. Kéo vào field `Sprite` của B52_Shadow
3. Set **Color = Đỏ** (R255, G0, B0, A255)
4. Set **Scale = (10, 10, 1)** ← CỰC LỚN
5. Click Play

**Nếu THẤY sprite test:**
→ Vấn đề là sprite B52_Shadow.png không đúng!
→ Xem lại Bước 1: Xử lý hình ảnh

**Nếu VẪN KHÔNG THẤY:**
→ Vấn đề là setup Unity, không phải sprite!

---

## 🎯 **GIẢI PHÁP CUỐI CÙNG:**

Nếu đã thử TẤT CẢ trên mà vẫn không được:

### **Recreate từ đầu:**

1. **XÓA** GameObject `B52_Shadow` cũ
2. **Tạo lại** từ đầu:

```
Hierarchy → Right click → Create Empty
Name: B52_Shadow_NEW
Position: (0, 0, -5)
Scale: (5, 5, 1)

Add Component → Sprite Renderer
- Sprite: Kéo B52_Shadow.png vào
- Color: ĐEN (R0, G0, B0, A255)
- Sorting Layer: Default
- Order in Layer: 200

Add Component → B52 Shadow Simple
- Sprite Scale: (5, 5, 1)
- Shadow Opacity: 1.0
- Enable Fade: UNCHECK
- Follow Camera: ✅ CHECK
- Camera Offset: 3
- B52 Sound: Kéo b52-sound.mp3 vào
- Auto Start: ✅ CHECK
- Initial Delay: 5

Click Play → ĐỢI 5 GIÂY → PHẢI THẤY MÁY BAY TO SU!
```

---

## 📸 **SO SÁNH ĐÚNG/SAI:**

### ❌ **SAI - Không thấy gì:**
```
Sprite Renderer:
- Sprite: None ← TRỐNG!
- Order in Layer: -1
```

### ✅ **ĐÚNG - Thấy rõ:**
```
Sprite Renderer:
- Sprite: B52_Shadow.png ← CÓ HÌNH!
- Color: Đen, Alpha 255
- Order in Layer: 100+

B52 Shadow Simple:
- Sprite Scale: (2+, 2+, 1)
- Shadow Opacity: 0.6 - 1.0
```

---

## 💡 **MẸO CUỐI CÙNG:**

### Không cần sprite đẹp để test:

Để test xem script có hoạt động không:

1. Component **Sprite Renderer**
2. **Sprite**: Kéo BẤT KỲ hình nào vào (player, enemy, bullet...)
3. **Color**: Đổi thành **ĐỎ CHÓI** (R255, G0, B0, A255)
4. **Scale**: (10, 10, 1) ← CỰC LỚN
5. Click Play

→ Bạn SẼ THẤY một hình ĐỎ TO SU bay qua màn hình!
→ Nếu thấy = Script hoạt động tốt!
→ Sau đó chỉ cần thay sprite B52 đúng là OK!

---

**Good luck! Nếu vẫn không được, paste FULL LOGS từ Console và tôi sẽ giúp! 🚀**

