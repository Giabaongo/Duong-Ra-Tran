# 🛩️ HƯỚNG DẪN TẠO HIỆU ỨNG B52 BAY QUA

Hướng dẫn đầy đủ để tạo hiệu ứng máy bay B52 bay qua với bóng đổ trong Unity.

---

## 📋 MỤC LỤC
1. [Tìm/Tạo Sprite Bóng Máy Bay](#1-tìmtạo-sprite-bóng-máy-bay)
2. [Setup trong Unity](#2-setup-trong-unity)
3. [Cấu hình Prefab](#3-cấu-hình-prefab)
4. [Trigger Máy Bay](#4-trigger-máy-bay)
5. [Tùy chỉnh nâng cao](#5-tùy-chỉnh-nâng-cao)

---

## 1. TÌM/TẠO SPRITE BÓNG MÁY BAY

### 🔍 CÁCH 1: Tìm sprite có sẵn

**Từ khóa tìm kiếm Google/Bing:**
```
- "B52 bomber shadow sprite"
- "airplane shadow top down PNG"
- "aircraft shadow silhouette 2D"
- "plane shadow overlay transparent"
```

**Nguồn miễn phí:**
- **OpenGameArt.org** → Search "aircraft shadow"
- **Itch.io** → Filter "Free" → Search "plane shadow"
- **Kenney.nl** → Vehicle/Aircraft packs
- **FreePik** → Search "plane shadow vector" (chọn free)

### 🎨 CÁCH 2: Tự tạo từ ảnh máy bay

#### Dùng Photoshop/GIMP:

1. **Import ảnh B52 (top-down view)**
   - Tìm ảnh B52 nhìn từ trên xuống
   - Kích thước đề xuất: 512x512px hoặc lớn hơn

2. **Tạo bóng:**
   ```
   - Duplicate layer
   - Select All → Fill với màu đen #000000
   - Giữ nguyên shape máy bay (dùng alpha channel)
   ```

3. **Làm mềm bóng:**
   ```
   - Filter → Blur → Gaussian Blur (3-8px)
   - Opacity: 50-70%
   ```

4. **Scale lớn hơn một chút (optional):**
   ```
   - Transform → Scale 110-120%
   - Lý do: Bóng thường lớn hơn vật thể khi ánh sáng xa
   ```

5. **Export:**
   ```
   - File → Export As → PNG
   - ✅ Enable Transparency (Alpha channel)
   - Tên file: B52_Shadow.png
   ```

#### Dùng Paint.NET (Free, Windows):

1. Mở ảnh B52
2. **Layers** → Duplicate layer
3. **Adjustments** → **Hue/Saturation** → Saturation = -100 (Grayscale)
4. **Adjustments** → **Brightness/Contrast** → Brightness = -100 (Đen)
5. **Effects** → **Blurs** → **Gaussian Blur** (Radius: 5)
6. **Layers** → Layer Properties → Opacity = 60%
7. **File** → **Save As** → PNG (with transparency)

### 📐 KỸ THUẬT VẼ NHANH (Nếu không có ảnh):

Nếu không tìm được ảnh, vẽ shape đơn giản:

```
┌─────────────────────────┐
│     ╱▔▔▔▔▔▔▔▔▔▔▔╲       │  ← Thân máy bay
│    ╱               ╲     │
│   █████████████████████  │  ← Cánh chính
│    ╲               ╱     │
│     ╲_____________╱      │
│          ╱╲              │  ← Đuôi máy bay
│         ╱  ╲             │
└─────────────────────────┘
```

**Tool vẽ nhanh:**
- **Inkscape** (Free vector editor)
- **Adobe Illustrator** (nếu có)
- **PowerPoint/Google Slides** (!) → Vẽ shapes → Export as PNG

---

## 2. SETUP TRONG UNITY

### Bước 1: Import Sprite

1. Copy file `B52_Shadow.png` vào folder:
   ```
   Assets/Sprites/Effects/
   ```

2. Chọn file trong Unity:
   - **Texture Type**: Sprite (2D and UI)
   - **Sprite Mode**: Single
   - **Pixels Per Unit**: 100 (hoặc match với sprites khác)
   - **Filter Mode**: Bilinear
   - **Format**: RGBA (với alpha)
   - Click **Apply**

### Bước 2: Tạo B52 Shadow Prefab

1. **Tạo Empty GameObject:**
   ```
   Hierarchy → Right click → Create Empty
   Name: B52_Shadow
   ```

2. **Add Sprite:**
   ```
   - Add Component → Sprite Renderer
   - Sprite: Kéo B52_Shadow.png vào
   - Color: RGBA(0, 0, 0, 128) - Đen, alpha 50%
   - Sorting Layer: "Default" hoặc "Effects"
   - Order in Layer: -1 (dưới các object khác)
   ```

3. **Add Script:**
   ```
   - Add Component → B52Flyover (script đã tạo)
   ```

4. **Configure B52Flyover:**

   **Movement Settings:**
   - Start Position: `(-20, 10)` (ngoài màn hình bên trái)
   - End Position: `(20, -10)` (ngoài màn hình bên phải)
   - Fly Speed: `5` units/second

   **Visual Settings:**
   - ✅ Fade In Out: Checked
   - Fade Distance: `3`
   - Shadow Opacity: `0.5` (50%)
   - Shadow Scale: `(1.2, 1.2, 1)` (Lớn hơn 20%)

   **Audio Settings:**
   - Engine Sound: Kéo file âm thanh B52 vào
   - ✅ Loop Sound: Checked
   - Max Volume: `0.8`
   - Audio Fade Distance: `10`

   **Automation:**
   - ☐ Auto Start: Unchecked (sẽ trigger thủ công)
   - ✅ Destroy On Complete: Checked

5. **Save as Prefab:**
   ```
   Kéo B52_Shadow từ Hierarchy vào folder:
   Assets/Prefabs/Effects/B52_Shadow.prefab
   ```

6. **Xóa khỏi scene** (giữ lại prefab)

### Bước 3: Setup B52 Trigger

1. **Tạo GameObject quản lý:**
   ```
   Hierarchy → Right click → Create Empty
   Name: B52_FlyoverManager
   Position: (0, 0, 0)
   ```

2. **Add Script:**
   ```
   Add Component → B52FlyoverTrigger
   ```

3. **Configure:**

   **B52 Prefab:**
   - Kéo `B52_Shadow.prefab` vào field "B52 Shadow Prefab"

   **Auto Trigger Settings:**
   - ✅ Auto Trigger: Checked (nếu muốn tự động)
   - Trigger Delay: `10` (bay lần đầu sau 10s)
   - Interval Min: `30` (ít nhất 30s giữa các lần)
   - Interval Max: `60` (tối đa 60s)
   - ✅ Random Path: Checked

   **Flight Path Presets:** (mặc định đã có 4 paths)
   - Path 0: Trái → Phải (trên)
   - Path 1: Phải → Trái (trên)
   - Path 2: Trái → Phải (giữa)
   - Path 3: Phải → Trái (giữa)

---

## 3. CẤU HÌNH PREFAB

### Sorting Layer (quan trọng!)

Để bóng hiển thị DƯỚI tất cả objects:

1. **Edit → Project Settings → Tags and Layers**
2. **Sorting Layers** → Add layer: `Background`
3. Chọn B52_Shadow prefab:
   - Sprite Renderer → Sorting Layer: `Background`
   - Order in Layer: `10`

### Layer Order đề xuất:
```
Background (-10)    ← Ground/Terrain
Background (0)      ← Shadows
Background (10)     ← B52 Shadow ✈️
Default (0)         ← Buildings, Objects
Default (10)        ← Enemies
Default (20)        ← Player
Default (30)        ← Effects
UI (100)            ← UI Elements
```

---

## 4. TRIGGER MÁY BAY

### 🎮 CÁCH 1: Trigger Tự Động (Auto Mode)

Đã setup ở Bước 3 → B52 sẽ tự bay qua định kỳ

### 🎮 CÁCH 2: Trigger Thủ Công từ Code

#### A. Trigger random flyover:

```csharp
// Trong bất kỳ script nào
B52FlyoverTrigger trigger = FindObjectOfType<B52FlyoverTrigger>();
if (trigger != null)
{
    trigger.TriggerRandomFlyover();
}
```

#### B. Trigger path cụ thể:

```csharp
B52FlyoverTrigger trigger = FindObjectOfType<B52FlyoverTrigger>();
if (trigger != null)
{
    trigger.TriggerFlyover(0); // Path 0: Trái sang phải
}
```

#### C. Trigger với path custom:

```csharp
B52FlyoverTrigger trigger = FindObjectOfType<B52FlyoverTrigger>();
if (trigger != null)
{
    Vector2 start = new Vector2(-15f, 20f);
    Vector2 end = new Vector2(15f, -20f);
    float speed = 7f;
    
    trigger.TriggerFlyover(start, end, speed);
}
```

### 🎮 CÁCH 3: Trigger từ Event (Game Event)

Ví dụ: Bay qua khi đánh bại boss, hoặc khi win:

```csharp
// Trong GameManager1968.cs
public void Victory()
{
    hasWon = true;
    Debug.Log("🎉🎉🎉 === VICTORY! === 🎉🎉🎉");
    
    // ✈️ Trigger B52 flyover khi thắng!
    B52FlyoverTrigger b52 = FindObjectOfType<B52FlyoverTrigger>();
    if (b52 != null)
    {
        b52.TriggerRandomFlyover();
    }
    
    // ... rest of code
}
```

### 🎮 CÁCH 4: Trigger từ Inspector

1. Chọn `B52_FlyoverManager` trong Hierarchy
2. Click chuột phải vào component `B52FlyoverTrigger`
3. Chọn **"Test Flyover - Random"**
4. B52 sẽ bay qua ngay lập tức!

---

## 5. TÙY CHỈNH NÂNG CAO

### 🎨 Hiệu ứng thêm

#### A. Thêm Rotation (máy bay xoay):

Trong `B52Flyover.cs`, thêm vào `Update()`:

```csharp
// Xoay về hướng di chuyển
Vector2 direction = (endPosition - startPosition).normalized;
float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // -90 nếu sprite hướng lên
```

#### B. Thêm Scale Animation (máy bay to dần):

```csharp
// Trong Update(), sau phần movement:
float scaleProgress = Mathf.Lerp(0.8f, 1.2f, journeyProgress);
transform.localScale = shadowScale * scaleProgress;
```

#### C. Thêm nhiều bóng (fleet of planes):

Tạo prefab `B52_Fleet` với 3-5 B52_Shadow, cách nhau một khoảng.

### 🔊 Hiệu ứng âm thanh 3D

Nếu muốn âm thanh spatial (gần to, xa nhỏ):

1. Chọn prefab B52_Shadow
2. Audio Source:
   - Spatial Blend: `1.0` (3D)
   - Min Distance: `5`
   - Max Distance: `50`
   - Volume Rolloff: `Linear` hoặc `Logarithmic`

### 🌍 Multiple Paths

Thêm nhiều paths trong `B52FlyoverTrigger`:

```csharp
[SerializeField] private Vector2[] startPositions = new Vector2[]
{
    new Vector2(-20f, 15f),    // Path 0: Trái → Phải (trên)
    new Vector2(20f, 15f),     // Path 1: Phải → Trái (trên)
    new Vector2(-20f, 0f),     // Path 2: Trái → Phải (giữa)
    new Vector2(20f, 0f),      // Path 3: Phải → Trái (giữa)
    new Vector2(-20f, -15f),   // Path 4: Trái → Phải (dưới)
    new Vector2(20f, -15f),    // Path 5: Phải → Trái (dưới)
    new Vector2(0f, 25f),      // Path 6: Trên → Dưới (thẳng)
    new Vector2(0f, -25f),     // Path 7: Dưới → Trên (thẳng)
};
```

---

## 🐛 TROUBLESHOOTING

### Vấn đề: Không thấy bóng máy bay

**Giải pháp:**
1. Kiểm tra Sorting Layer: Phải thấp hơn Player/Enemy
2. Kiểm tra Camera: Camera phải bao phủm start/end positions
3. Kiểm tra Opacity: Shadow Opacity > 0
4. Kiểm tra Console: Có log "B52 flyover started!" không?

### Vấn đề: Không nghe thấy âm thanh

**Giải pháp:**
1. Kiểm tra Audio Source có attached không
2. Kiểm tra Engine Sound đã assign chưa
3. Kiểm tra Max Volume > 0
4. Kiểm tra AudioListener có trong scene không (thường ở Main Camera)

### Vấn đề: Máy bay bay quá nhanh/chậm

**Giải pháp:**
- Adjust `Fly Speed` trong B52Flyover script
- Đề xuất: 3-8 units/second

### Vấn đề: Bóng quá đậm/quá nhạt

**Giải pháp:**
- Adjust `Shadow Opacity` (0-1)
- Hoặc adjust Color Alpha trong Sprite Renderer

---

## 📊 THÔNG SỐ ĐỀ XUẤT

### Máy bay bay chậm, uy nghiêm:
```
Fly Speed: 3-4
Shadow Opacity: 0.6-0.7
Fade Distance: 5
```

### Máy bay bay nhanh, hành động:
```
Fly Speed: 7-10
Shadow Opacity: 0.4-0.5
Fade Distance: 2
```

### Máy bay bay xa (high altitude):
```
Fly Speed: 5
Shadow Opacity: 0.3-0.4
Shadow Scale: (1.5, 1.5, 1) - Lớn hơn nhiều
Fade Distance: 8
```

---

## 🎬 KẾT QUẢ MONG ĐỢI

Khi setup xong, bạn sẽ có:

✅ Bóng máy bay B52 di chuyển êm ái qua map  
✅ Fade in từ từ khi vào, fade out khi ra  
✅ Âm thanh động cơ to dần khi lại gần, nhỏ dần khi xa  
✅ Tự động trigger theo khoảng thời gian random  
✅ Có thể trigger thủ công từ code  
✅ Visualization trong Scene view (Gizmos)  

---

## 🚀 NEXT STEPS

Sau khi có hiệu ứng cơ bản, bạn có thể:

1. **Thêm bombing effect**: Spawn explosions dọc đường bay
2. **Thêm camera shake**: Rung camera nhẹ khi máy bay bay qua
3. **Thêm particle effects**: Khói, mây theo máy bay
4. **Multiple planes**: Fleet với nhiều máy bay bay theo đội hình
5. **Player warning**: UI cảnh báo "Air strike incoming!"

---

**Good luck! ✈️🎮**


