# 🎯 SETUP CAMERA SHAKE CHO UNITY 6 (CINEMACHINE 3.x)

## ⚠️ **UNITY 6 KHÁC HOÀN TOÀN!**

Unity 6 dùng **Cinemachine 3.x** - API và UI hoàn toàn khác so với Unity 2021-2023!

---

## ✅ **HƯỚNG DẪN ĐÚNG CHO UNITY 6:**

### **BƯỚC 1: Add CameraShake Component**

```
Hierarchy → Chọn "CinemachineCamera" (đang tracking player)
→ Inspector → Add Component
→ Gõ "CameraShake" → Enter
```

**Cấu hình CameraShake:**
```
CameraShake (Script):
├─ Max Shake Magnitude: 0.5
├─ Default Duration: 0.3
├─ Damping Speed: 1.0
└─ Use Cinemachine: ☑️ CHECK THIS!
```

---

### **BƯỚC 2: Thêm Noise Component** 

**QUAN TRỌNG:** Unity 6 KHÔNG có nút "Add Extension"!

Làm theo:

1. **Scroll xuống phần "Procedural Components"** (trong CinemachineCamera)
2. **Tìm dòng "Noise"** (hiện đang là "None")
3. **Click vào dropdown "None"**
4. **Chọn: "Basic Multi Channel Perlin"**

```
Procedural Components:
├─ Position Control: Position Composer (hoặc Position Composer)
├─ Rotation Control: None
└─ Noise: None → Basic Multi Channel Perlin ⬅️ CHỌN!
```

**Sau khi chọn, sẽ xuất hiện:**

```
Noise: Basic Multi Channel Perlin
☐ ⬅️ PHẢI CHECK CÁI NÀY! (Enable component)
├─ Noise Profile: (None) ⬅️ Có thể để trống HOẶC tạo asset mới
├─ Amplitude Gain: 0    ⬅️ Để 0 (tự động)
├─ Frequency Gain: 0    ⬅️ Để 0 (tự động)
└─ Pivot Offset: (0, 0, 0)
```

**QUAN TRỌNG:**
1. ☑️ **CHECK** checkbox ở đầu component để enable!
2. Noise Profile: Có thể để (None) hoặc tạo mới
3. Amplitude/Frequency: Để 0

**NẾU THẤY WARNING "Component is disabled or has a problem":**
- Click vào icon ⚠️
- Check ☑️ checkbox để enable component
- Hoặc tạo Noise Settings asset (xem dưới)

---

## 🛠️ **TẠO NOISE SETTINGS ASSET (NẾU CẦN):**

Nếu muốn tạo Noise Settings riêng:

### **Cách tạo:**

```
1. Project → Assets folder
2. Chuột phải → Create → Cinemachine → Noise Settings
3. Đặt tên: "B52ExplosionNoise"
4. Click vào asset vừa tạo
5. Inspector sẽ hiện settings (để mặc định)
```

### **Gán vào CinemachineCamera:**

```
1. Chọn CinemachineCamera
2. Scroll xuống Noise: Basic Multi Channel Perlin
3. Kéo "B52ExplosionNoise" asset vào field "Noise Profile"
```

**LƯU Ý:** Không bắt buộc! Có thể để Noise Profile = (None) và vẫn hoạt động!

---

## 🎮 **TEST NGAY (KHÔNG CẦN PLAY):**

1. **Chọn CinemachineCamera** trong Hierarchy
2. **Scroll xuống component CameraShake**
3. **Click chuột phải** vào title "Camera Shake (Script)"
4. **Chọn: "Test Explosion Shake"**
5. **→ 📳 CAMERA RUNG NGAY!**

---

## 🔍 **KIỂM TRA SETUP ĐÚNG:**

### **Logs khi Play:**

```
[CameraShake] ✅ Cinemachine 3.x (Unity 6) setup successful!
```

**Nếu thấy log này → HOÀN HẢO!**

### **Nếu thấy warning:**

```
⚠️ Cinemachine Camera không có Noise component!
```

**→ Quay lại BƯỚC 2, thêm Noise!**

---

## 📊 **SO SÁNH UNITY 2021-2023 vs UNITY 6:**

| Tính năng | Unity 2021-2023 | **Unity 6** |
|-----------|-----------------|-------------|
| Component | `CinemachineVirtualCamera` | `CinemachineCamera` |
| Thêm Noise | Click "Add Extension" | Dropdown "Noise" |
| API | `GetCinemachineComponent<>()` | `GetComponent<>()` |
| Namespace | `Cinemachine` | `Unity.Cinemachine` |

---

## ⚙️ **THAY ĐỔI CODE (ĐÃ CẬP NHẬT):**

### **CameraShake.cs:**

```csharp
// Unity 6 support
private CinemachineCamera cinemachineCamera;
private CinemachineBasicMultiChannelPerlin perlinNoise;

void Awake()
{
    cinemachineCamera = GetComponent<CinemachineCamera>();
    if (cinemachineCamera != null)
    {
        perlinNoise = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlinNoise != null)
        {
            Debug.Log("[CameraShake] ✅ Cinemachine 3.x (Unity 6) setup successful!");
        }
    }
}
```

### **B52Bomb.cs:**

```csharp
// Tìm CinemachineCamera (Unity 6)
var cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
if (cinemachineCamera != null)
{
    cameraShake = cinemachineCamera.GetComponent<CameraShake>();
}
```

**→ ĐÃ TỰ ĐỘNG SUPPORT UNITY 6!**

---

## 🎯 **HÌNH ẢNH HƯỚNG DẪN:**

### **1. Tìm Procedural Components:**

```
Inspector → CinemachineCamera
↓
[Transform]
[CinemachineCamera]
├─ Status: Live
├─ Priority: (using default)
├─ Output Channel: Default
├─ Standby Update: Round Robin
├─ Blend Hint: Nothing
├─ Lens: 5
├─ Tracking Target: Player (Transform)
├─ Global Settings
├─ Game View Guides
└─ Procedural Components ⬅️ SCROLL ĐẾN ĐÂY!
    ├─ Position Control: Position Composer
    ├─ Rotation Control: None
    └─ Noise: None ⬅️ CLICK VÀO ĐÂY!
```

### **2. Chọn Noise:**

Click vào dropdown "None", sẽ thấy:

```
Noise:
├─ None
├─ Basic Multi Channel Perlin ⬅️ CHỌN CÁI NÀY!
└─ (có thể có thêm options khác)
```

---

## 🧪 **KIỂM TRA HOẠT ĐỘNG:**

### **Test 1: Trong Editor (không play)**

```
Chọn CinemachineCamera
→ CameraShake component
→ Chuột phải → "Test Explosion Shake"
→ Xem Scene View → Camera rung!
```

### **Test 2: Trong Game**

```
Play → Chờ B52 (10 giây) → Bom nổ
→ Console log:
   [Bomb] 💥 EXPLOSION at (...)
   [CameraShake] 💥 Explosion at 8.5m, Intensity: 0.85
   [B52Bomb] 📳 Camera shake triggered!
```

---

## ⚠️ **TROUBLESHOOTING:**

### **❌ "Không tìm thấy CinemachineCamera"**

**Nguyên nhân:** Đặt CameraShake sai nơi

**Giải pháp:**
- Add vào GameObject có component **CinemachineCamera**
- KHÔNG phải Main Camera!
- Tìm trong Hierarchy object tên "CM vcam1" hoặc "CinemachineCamera"

---

### **❌ "Không có Noise component"**

**Nguyên nhân:** Chưa thêm Noise

**Giải pháp:**
1. Chọn CinemachineCamera
2. Scroll xuống "Procedural Components"
3. Noise: None → Basic Multi Channel Perlin

---

### **❌ "Camera không rung"**

**Check list:**
- ☐ CameraShake đã add vào đúng CinemachineCamera?
- ☐ "Use Cinemachine" đã check?
- ☐ Noise đã chọn "Basic Multi Channel Perlin"?
- ☐ Console có warning không?

---

## 📝 **TÓM TẮT:**

### **Unity 6 (Cinemachine 3.x):**

**Setup 2 bước:**
1. ✅ Add `CameraShake` vào `CinemachineCamera`
2. ✅ Set `Noise` thành `Basic Multi Channel Perlin`

**KHÔNG CẦN:**
- ❌ "Add Extension" button (không tồn tại trong Unity 6)
- ❌ Noise Profile
- ❌ Chỉnh Amplitude/Frequency Gain

**API thay đổi:**
- `CinemachineVirtualCamera` → `CinemachineCamera`
- `GetCinemachineComponent<>()` → `GetComponent<>()`

---

## 🎯 **FINAL CHECKLIST:**

```
☐ CinemachineCamera có component CameraShake
☐ CameraShake → Use Cinemachine: ☑️
☐ Procedural Components → Noise: Basic Multi Channel Perlin
☐ Test bằng "Test Explosion Shake" → Rung!
☐ Play game → B52 nổ → Rung!
```

---

**🎯 ĐÃ CẬP NHẬT CODE CHO UNITY 6! CHỈ CẦN SETUP 2 BƯỚC! 📳✨**

**Files đã cập nhật:**
- ✅ `CameraShake.cs` - Support Unity 6
- ✅ `B52Bomb.cs` - Support Unity 6
- ✅ Tự động detect Cinemachine 3.x

