# 🚀 HƯỚNG DẪN SETUP NHANH - BÓNG MÁY BAY B52

**Thời gian:** ~5 phút  
**Độ khó:** ⭐ Rất dễ

---

## ✨ **TÍNH NĂNG MỚI - CAMERA FOLLOW MODE!**

Máy bay giờ sẽ bay qua **KHUNG HÌNH CAMERA** (tracking player), không phải random trên map!

**Lợi ích:**
- ✅ Player **LUÔN THẤY** máy bay bay qua
- ✅ Hoạt động với **Cinemachine** camera
- ✅ Tracking player tự động
- ✅ Phù hợp với mọi kích thước map

**→ Đọc thêm:** `CAMERA_FOLLOW_MODE.md`

---

## 📸 BƯỚC 1: XỬ LÝ HÌNH ẢNH (2 phút)

### Cách 1: Dùng PowerPoint (Nhanh nhất!)

1. **Mở PowerPoint** → Tạo slide mới
2. **Insert → Pictures** → Chọn hình B52
3. **Click vào hình** → Tab **Picture Format**
4. **Remove Background** (Xóa nền tự động)
   - Nếu có phần thừa, dùng "Mark Areas to Keep/Remove"
   - Click **Keep Changes**
5. **Right click ảnh** → **Save as Picture** → Chọn **PNG**
6. Đặt tên: `B52_Shadow.png`
7. ✅ Done!

### Cách 2: Dùng Remove.bg (Online - Miễn phí)

1. Vào: https://www.remove.bg/
2. Upload hình B52
3. Click **Remove Background**
4. **Download** → Chọn PNG
5. Đổi tên: `B52_Shadow.png`
6. ✅ Done!

### Cách 3: Dùng Paint.NET (Free software)

1. Mở Paint.NET
2. **File → Open** → Chọn hình B52
3. **Tools → Magic Wand** → Click vào nền xám
4. **Delete** (xóa nền)
5. Nếu còn nền sót:
   - **Select → Invert**
   - **Delete** lại
6. **File → Save As** → PNG
7. ✅ Done!

### 💡 Mẹo:
- Nếu sau khi xóa nền mà bóng quá nhạt:
  - **Adjustments → Brightness/Contrast** → Giảm Brightness
- Nếu muốn bóng đậm hơn:
  - **Adjustments → Hue/Saturation** → Giảm Lightness

---

## 📁 BƯỚC 2: IMPORT VÀO UNITY (1 phút)

1. Copy file `B52_Shadow.png` vào Unity:
   ```
   Assets/Sprites/Effects/B52_Shadow.png
   ```
   (Tạo folder Effects nếu chưa có)

2. Chọn file trong Unity:
   - **Texture Type**: `Sprite (2D and UI)`
   - **Pixels Per Unit**: `100`
   - Click **Apply**

3. ✅ Done!

---

## 🎮 BƯỚC 3: TẠO PREFAB B52 (2 phút)

### A. Tạo GameObject:

1. **Hierarchy** → Right click → **Create Empty**
2. Đặt tên: `B52_Shadow`
3. Position: `(0, 0, 0)`

### B. Add Sprite:

1. Chọn `B52_Shadow` GameObject
2. **Add Component** → **Sprite Renderer**
3. Kéo sprite `B52_Shadow.png` vào field **Sprite**
4. Cài đặt:
   - **Color**: `RGBA(0, 0, 0, 255)` (Đen)
   - **Sorting Layer**: `Default`
   - **Order in Layer**: `-1` (Dưới tất cả objects)

### C. Add Script:

1. **Add Component** → Gõ `B52ShadowSimple`
2. Chọn script vừa tạo

### D. Cài đặt Script:

**⚙️ Cài Đặt Cơ Bản:**
- **Fly Speed**: `5` (Tốc độ bay)
- **Delay Between Flights**: `30` (Bay lại sau 30 giây)
- **Auto Start**: ✅ Checked
- **Initial Delay**: `5` (Delay lần đầu 5s)

**🎨 Hiệu Ứng Hình Ảnh:**
- **Shadow Opacity**: `0.6` (Độ mờ 60%)
- **Enable Fade**: ✅ Checked
- **Fade Distance**: `5`

**🔊 Âm Thanh:**
- **B52 Sound**: Kéo file `b52-sound.mp3` vào đây ⬅️ **QUAN TRỌNG!**
- **Volume**: `0.7`
- **Fade Audio**: ✅ Checked
- **Audio Max Distance**: `15`

**🛫 Đường Bay:** ⬅️ **MỚI - CAMERA FOLLOW!**
- **Follow Camera**: ✅ Checked ⬅️ **QUAN TRỌNG!** Bay theo camera (tracking player)
- **Camera Offset**: `3` (Khoảng cách từ camera)
- **Random Flight Path**: ✅ (Không dùng khi follow camera)

### E. Save Prefab:

1. Kéo `B52_Shadow` từ Hierarchy vào folder:
   ```
   Assets/Prefabs/   /B52_Shadow.prefab
   ```
2. **XÓA** GameObject `B52_Shadow` khỏi Hierarchy (giữ lại prefab)

---

## ✅ BƯỚC 4: THÊM VÀO SCENE (30 giây)

1. Kéo prefab `B52_Shadow.prefab` vào **Hierarchy**
2. Position: `(0, 0, 0)` (không quan trọng, sẽ tự di chuyển)
3. ✅ **XONG!** 🎉

---

## 🎮 TEST NGAY:

1. **Click Play** ▶️
2. Đợi 5 giây...
3. ✈️ Máy bay sẽ bay qua với âm thanh!
4. Sau 30 giây → bay lại!
5. Lặp mãi mãi! 🔄

---

## 🎨 TÙY CHỈNH (Nếu muốn):

### Bay nhanh hơn:
- `Fly Speed`: `8-10`

### Bay chậm hơn (uy nghiêm):
- `Fly Speed`: `3-4`

### Bay thường xuyên hơn:
- `Delay Between Flights`: `15-20` (15-20 giây)

### Bay ít hơn:
- `Delay Between Flights`: `60-120` (1-2 phút)

### Bóng đậm hơn:
- `Shadow Opacity`: `0.8-1.0`

### Bóng nhạt hơn:
- `Shadow Opacity`: `0.3-0.5`

### Âm thanh to hơn:
- `Volume`: `0.9-1.0`

### Âm thanh nhỏ hơn:
- `Volume`: `0.3-0.5`

---

## 🔊 QUAN TRỌNG - AUDIO:

**PHẢI KÉO FILE `b52-sound.mp3` VÀO FIELD "B52 Sound"!**

Nếu không có âm thanh:
1. Chọn `B52_Shadow` trong Hierarchy
2. Tìm component `B52ShadowSimple`
3. Kéo file `Assets/Audio/Music/b52-sound.mp3` vào field **B52 Sound**

---

## 🐛 TROUBLESHOOTING:

### Không thấy máy bay bay:
- ✅ Kiểm tra Console có log "Chuyến bay #1 bắt đầu!" không
- ✅ Kiểm tra Camera bao phủm toàn map
- ✅ Kiểm tra Sprite Renderer đã có sprite chưa

### Không nghe âm thanh:
- ✅ Kéo `b52-sound.mp3` vào field **B52 Sound**
- ✅ Kiểm tra Volume > 0
- ✅ Kiểm tra AudioListener có trong scene (Main Camera)
- ✅ Kiểm tra volume máy tính

### Bay quá nhanh/chậm:
- ✅ Điều chỉnh **Fly Speed**

### Bay quá thường xuyên:
- ✅ Tăng **Delay Between Flights**

---

## 🎯 KẾT QUẢ MONG ĐỢI:

✅ Máy bay B52 bay qua tự động mỗi 30 giây  
✅ Random đường bay (6 đường khác nhau)  
✅ Fade mượt khi vào/ra  
✅ Âm thanh B52 to dần → nhỏ dần theo khoảng cách  
✅ Chạy liên tục suốt game  
✅ Không cần code thêm gì!  

---

## 📊 SETTINGS ĐỀ XUẤT:

### Máy bay bay thường xuyên (hành động):
```
Fly Speed: 7
Delay Between Flights: 20
Shadow Opacity: 0.5
Volume: 0.8
```

### Máy bay bay hiếm (background ambience):
```
Fly Speed: 4
Delay Between Flights: 60
Shadow Opacity: 0.6
Volume: 0.6
```

### Máy bay bay chậm (uy nghiêm):
```
Fly Speed: 3
Delay Between Flights: 45
Shadow Opacity: 0.7
Volume: 0.7
```

---

## 🎬 VIDEO DEMO (trong Unity Editor):

1. **Click Play** ▶️
2. Mở **Console** (Ctrl + Shift + C)
3. Xem logs:
   ```
   [B52Shadow] ✈️ Sẽ bay lần đầu sau 5s
   [B52Shadow] ✈️ Chuyến bay #1 bắt đầu!
   [B52Shadow] ✅ Chuyến bay #1 hoàn thành!
   ```

---

## 🚀 THÊM NHIỀU MÁY BAY:

Nếu muốn nhiều máy bay bay cùng lúc:

1. Duplicate prefab `B52_Shadow` 2-3 lần
2. Đổi **Initial Delay** khác nhau:
   - B52_Shadow_1: `5s`
   - B52_Shadow_2: `15s`
   - B52_Shadow_3: `25s`
3. Boom! 3 máy bay bay xen kẽ nhau! ✈️✈️✈️

---

**Chúc bạn thành công! ✈️🎮**

Nếu có vấn đề gì, check Console để xem logs debug!


