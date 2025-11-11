# 📷 B52 BAY THEO CAMERA (TRACKING PLAYER)

**✨ MỚI:** Máy bay giờ sẽ bay qua **khung hình camera** của player, không phải random trên map!  
**Kết quả:** Player sẽ **LUÔN THẤY** máy bay bay qua, bất kể đang ở đâu trên map!

---

## 🎯 SO SÁNH 2 MODE:

### ❌ **LEGACY MODE (Random toàn map):**
```
- Bay ở vị trí cố định trên map
- Player có thể không thấy nếu đang ở xa
- Chỉ phù hợp với map nhỏ
- Follow Camera = ☐ Unchecked
```

### ✅ **CAMERA FOLLOW MODE (Recommended!):**
```
- Bay qua khung hình camera
- Player LUÔN THẤY máy bay
- Hoạt động với mọi kích thước map
- Tracking player với Cinemachine
- Follow Camera = ✅ Checked ⬅️ MẶC ĐỊNH
```

---

## ⚙️ SETUP (1 phút):

### **BƯỚC 1: Tạo B52_Shadow GameObject**

Làm theo file `SETUP_NHANH_B52.md` như bình thường, **NHƯNG** cài đặt khác một chút:

### **BƯỚC 2: Cài đặt Script (QUAN TRỌNG!)**

```
⚙️ CÀI ĐẶT CƠ BẢN:
- Fly Speed: 5
- Delay Between Flights: 30
- Auto Start: ✅
- Initial Delay: 5

🎨 HIỆU ỨNG HÌNH ẢNH:
- Shadow Opacity: 0.6
- Enable Fade: ✅
- Fade Distance: 5

🔊 ÂM THANH:
- B52 Sound: Kéo b52-sound.mp3 vào ⬅️
- Volume: 0.7
- Fade Audio: ✅
- Audio Max Distance: 15

🛫 ĐƯỜNG BAY: ⬅️ MỚI!
- Follow Camera: ✅ Checked ⬅️ QUAN TRỌNG!
- Camera Offset: 3 (Khoảng cách từ camera)
- Random Flight Path: ✅ (Không dùng khi follow camera)
```

### **BƯỚC 3: XONG!**

Save prefab và kéo vào scene. Máy bay sẽ tự động bay theo camera!

---

## 🎮 CÁCH HOẠT ĐỘNG:

```
┌─────────────────────────────────────┐
│         [Player ở đây]              │
│             🎮                       │
│                                     │
│   ✈️ Máy bay bay qua                │
│   ←──────────────────→              │
│   (Luôn trong khung hình camera)   │
│                                     │
└─────────────────────────────────────┘
      📷 Camera Viewport
```

**Máy bay sẽ bay:**
1. **Random 4 hướng:**
   - Trái → Phải
   - Phải → Trái
   - Trên → Dưới
   - Dưới → Trên

2. **Luôn đi qua khung hình camera**

3. **Tự động điều chỉnh theo vị trí player:**
   - Player di chuyển → Camera di chuyển
   - Camera di chuyển → Máy bay bay theo camera

---

## 🔧 TÙY CHỈNH:

### **Camera Offset** (Khoảng cách từ camera)

```
Camera Offset: 3  ← Bắt đầu bay cách camera 3 units
```

- **Nhỏ (1-2)**: Máy bay xuất hiện đột ngột
- **Vừa (3-5)**: Smooth, có thời gian fade ✅ Recommended
- **Lớn (6+)**: Máy bay bay từ rất xa

### **Fly Speed** (Tốc độ bay)

```
Fly Speed: 5  ← 5 units/giây
```

- **Chậm (3-4)**: Uy nghiêm, dễ quan sát
- **Vừa (5-7)**: Cân bằng ✅ Recommended
- **Nhanh (8-10)**: Nhanh, hành động

---

## 🎬 TEST:

1. **Click Play** ▶️
2. **Di chuyển player** (W/A/S/D hoặc Arrow keys)
3. **Đợi 5 giây...**
4. ✈️ **BOOM!** Máy bay bay qua khung hình!
5. **Di chuyển player ra chỗ khác**
6. **Đợi 30 giây...**
7. ✈️ Máy bay bay qua **VỊ TRÍ MỚI** của player!

---

## 📊 DEMO SCENARIOS:

### Scenario 1: Player ở góc trên-trái map
```
Camera position: (-10, 10)
→ Máy bay bay qua khung hình (-10, 10)
Player thấy: ✅
```

### Scenario 2: Player di chuyển xuống góc dưới-phải
```
Camera position: (15, -15)
→ Máy bay bay qua khung hình (15, -15)
Player thấy: ✅
```

### Scenario 3: Player ở giữa map
```
Camera position: (0, 0)
→ Máy bay bay qua khung hình (0, 0)
Player thấy: ✅
```

**KẾT LUẬN:** Player **LUÔN LUÔN** thấy máy bay, bất kể ở đâu! 🎉

---

## 🐛 TROUBLESHOOTING:

### Máy bay vẫn bay ở vị trí cố định, không theo player:

**Nguyên nhân:** `Follow Camera` chưa được check!

**Giải pháp:**
1. Chọn `B52_Shadow` trong Hierarchy
2. Component `B52ShadowSimple`
3. Section **"🛫 Đường Bay"**
4. **Follow Camera**: ✅ Check vào đây!

---

### Máy bay bay quá gần/xa camera:

**Giải pháp:**
- Điều chỉnh **Camera Offset**:
  - Quá gần → Tăng lên (5-7)
  - Quá xa → Giảm xuống (2-3)

---

### Máy bay không theo player đúng:

**Kiểm tra:**
1. Main Camera có trong scene không?
2. Console có logs `📷 Camera-relative path` không?
3. Trong Scene view (khi Play), có thấy **khung cyan** (camera viewport) không?

---

## 🎨 DEBUG VISUAL (Scene View):

Khi **Play** game, trong **Scene view** bạn sẽ thấy:

```
┌─────────────────────┐
│   📷 Camera Viewport│  ← Text trên khung
│  ╔═══════════════╗  │
│  ║               ║  │  ← Khung cyan (camera bounds)
│  ║   ✈️          ║  │  ← Máy bay bay qua
│  ║               ║  │
│  ╚═══════════════╝  │
└─────────────────────┘
```

- **Khung cyan**: Camera viewport
- **Dấu ✈️ cyan**: Máy bay đang bay
- **Đường vàng**: Đường bay
- **Chấm đỏ**: Điểm đích

---

## 💡 TIPS & TRICKS:

### 1. Nhiều máy bay bay xen kẽ:

```
Duplicate B52_Shadow 2-3 lần:
- B52_Shadow_1: Initial Delay = 5s
- B52_Shadow_2: Initial Delay = 15s
- B52_Shadow_3: Initial Delay = 25s

Kết quả: 3 máy bay bay xen kẽ theo camera!
```

### 2. Bay thường xuyên hơn khi combat:

```csharp
// Trong GameManager hoặc EnemySpawner:
void OnEnemySpawn()
{
    B52ShadowSimple b52 = FindObjectOfType<B52ShadowSimple>();
    if (b52 != null)
    {
        b52.TriggerImmediateFlight(); // Bay ngay!
    }
}
```

### 3. Bay chậm rãi khi khám phá:

```
Fly Speed: 3
Delay Between Flights: 60
Volume: 0.5
```

### 4. Bay nhanh, dày đặc khi boss fight:

```
Fly Speed: 8
Delay Between Flights: 15
Volume: 0.9
```

---

## 🎯 SETTINGS ĐỀ XUẤT:

### 🎮 **Normal Gameplay:**
```
Follow Camera: ✅
Camera Offset: 3
Fly Speed: 5
Delay Between Flights: 30
Shadow Opacity: 0.6
Volume: 0.7
```

### ⚔️ **Combat Intensive:**
```
Follow Camera: ✅
Camera Offset: 2
Fly Speed: 7
Delay Between Flights: 20
Shadow Opacity: 0.5
Volume: 0.8
```

### 🌅 **Exploration/Peaceful:**
```
Follow Camera: ✅
Camera Offset: 5
Fly Speed: 3
Delay Between Flights: 60
Shadow Opacity: 0.7
Volume: 0.5
```

---

## 📐 TECHNICAL DETAILS:

### Cách tính toán đường bay:

```csharp
// 1. Lấy vị trí camera (player)
Vector3 cameraPos = mainCamera.transform.position;

// 2. Tính viewport size
float height = mainCamera.orthographicSize * 2f;
float width = height * mainCamera.aspect;

// 3. Random hướng (0-3)
int direction = Random.Range(0, 4);

// 4. Tính start/end dựa trên camera viewport + offset
// VD: Direction 0 (Trái → Phải)
start = (cameraPos.x - width/2 - offset, random Y)
end = (cameraPos.x + width/2 + offset, random Y)
```

### Camera Tracking:

- Mỗi lần bay, script **TỰ ĐỘNG** lấy vị trí camera hiện tại
- Không cần Cinemachine Brain hay component đặc biệt
- Hoạt động với **Main Camera** hoặc **Cinemachine Virtual Camera**
- Compatible với **Cinemachine Confiner**, **Follow**, **LookAt**

---

## 🚀 KẾT LUẬN:

✅ **Camera Follow Mode** là mode **TỐT NHẤT** cho game của bạn vì:

1. ✅ Player luôn thấy máy bay
2. ✅ Tăng immersion (đắm chìm)
3. ✅ Hoạt động với mọi kích thước map
4. ✅ Tracking player tự động
5. ✅ Setup đơn giản (chỉ check 1 box!)
6. ✅ Visual debug trong Scene view

**Recommended: Dùng mode này! 🎯**

---

**Chúc bạn thành công! ✈️📷**

