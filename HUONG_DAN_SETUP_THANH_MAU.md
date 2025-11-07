# 🩸 HƯỚNG DẪN SETUP THANH MÁU CHO PLAYER

## 📋 Tổng quan
Script `HealthBarUI.cs` đã được tạo để hiển thị thanh máu của player trong MauthanScene.
- ✅ Tự động đồng bộ với HP của player
- ✅ Cập nhật **NGAY LẬP TỨC** khi bị damage
- ✅ Hỗ trợ cả `PlayerHealth1968` và `LinhGiaiPhong1968`
- ✅ Có hiệu ứng màu sắc gradient (xanh → vàng → đỏ)
- ✅ Smooth transition animation (tùy chọn)

---

## 🛠️ CÁCH SETUP TRONG UNITY (5 BƯỚC)

### Bước 1: Tạo UI Canvas
1. Mở scene `MauthanScene.unity`
2. Click chuột phải trong Hierarchy → `UI` → `Canvas`
3. Đổi tên Canvas thành `PlayerUI` hoặc `GameUI`
4. Đảm bảo Canvas Settings:
   - **Render Mode**: Screen Space - Overlay
   - **Canvas Scaler**: Scale With Screen Size
   - **Reference Resolution**: 1920x1080 (hoặc resolution game của bạn)

### Bước 2: Tạo Health Bar Panel
1. Click chuột phải vào Canvas → `UI` → `Panel`
2. Đổi tên thành `HealthBarPanel`
3. Trong Inspector, chỉnh **Rect Transform**:
   - **Anchors**: Top-Left
   - **Position X**: 150
   - **Position Y**: -50
   - **Width**: 300
   - **Height**: 40
4. Tắt component **Image** nếu không muốn background

### Bước 3: Tạo Slider (Thanh Máu)
1. Click chuột phải vào `HealthBarPanel` → `UI` → `Slider`
2. Đổi tên thành `HealthSlider`
3. Trong Inspector:
   - **Min Value**: 0
   - **Max Value**: 1
   - **Value**: 1
   - **Interactable**: ❌ (Tắt - vì player không cần click vào)

4. Setup các thành phần con của Slider:
   
   **Background:**
   - Color: Đỏ đậm `(80, 0, 0, 255)` hoặc đen
   
   **Fill:**
   - Color: Xanh lá `(0, 255, 0, 255)` (script sẽ tự động đổi màu)
   - **Rect Transform**: Đảm bảo Fill Area có Left = 0, Right = 0
   
   **Handle Slide Area:**
   - Xóa hoặc disable (không cần handle cho health bar)

### Bước 4: Thêm Text Hiển Thị Số HP (Tùy chọn)
1. Click chuột phải vào `HealthBarPanel` → `UI` → `Text - TextMeshPro`
2. Đổi tên thành `HealthText`
3. Trong Inspector:
   - **Text**: "100 / 100"
   - **Font Size**: 24
   - **Alignment**: Center
   - **Color**: Trắng
4. Đặt ở giữa hoặc bên cạnh thanh máu

### Bước 5: Add Script `HealthBarUI`
1. Select `HealthBarPanel` trong Hierarchy
2. Trong Inspector → **Add Component** → Tìm `HealthBarUI`
3. Assign các references:

   **UI References:**
   - `Health Slider`: Kéo `HealthSlider` vào đây
   - `Fill Image`: Kéo `Fill` (con của Slider) vào đây
   - `Health Text`: Kéo `HealthText` vào đây (nếu có)

   **Colors:**
   - `Full Health Color`: Xanh lá (0, 255, 0)
   - `Half Health Color`: Vàng (255, 255, 0)
   - `Low Health Color`: Đỏ (255, 0, 0)
   - `Use Gradient Color`: ✅ (Bật để thanh máu đổi màu theo HP)

   **Animation:**
   - `Smooth Transition`: ✅ (Bật để có hiệu ứng mượt)
   - `Transition Speed`: 5 (tốc độ animation)

---

## ✅ KIỂM TRA SETUP

1. **Chạy game** (Play Mode)
2. Kiểm tra Console log:
   ```
   [HealthBarUI] Initialized - Max Health: 20, Current Health: 20
   [HealthBarUI] Subscribed to LinhGiaiPhong1968.OnHealthChanged event
   ```

3. **Để player bị damage** và xem:
   - ✅ Thanh máu giảm xuống **NGAY LẬP TỨC**
   - ✅ Màu sắc thay đổi (xanh → vàng → đỏ)
   - ✅ Text cập nhật số HP
   - ✅ Console log: `[HealthBarUI] ⚡ Health Changed! 15/20 (75%)`

---

## 🎨 TÙY CHỈNH NÂNG CAO

### 1. Thay đổi vị trí thanh máu
```
Top-Left:    Anchors: (0, 1) | Position: (150, -50)
Top-Center:  Anchors: (0.5, 1) | Position: (0, -50)
Top-Right:   Anchors: (1, 1) | Position: (-150, -50)
Bottom-Left: Anchors: (0, 0) | Position: (150, 50)
```

### 2. Thêm Icon Player
- Thêm `UI` → `Image` vào `HealthBarPanel`
- Assign sprite icon của player
- Đặt bên trái thanh máu

### 3. Thêm Border cho thanh máu
- Thêm `UI` → `Image` làm background
- Chỉnh **Image Type**: Sliced
- Assign sprite border (khung viền)

### 4. Thêm Animation Effect
- Tạo Animator cho `HealthBarPanel`
- Thêm animation "Shake" khi bị damage
- Gọi từ script: `animator.SetTrigger("Shake")`

---

## 🐛 TROUBLESHOOTING

### ❌ Thanh máu không hiển thị
**Giải pháp:**
1. Kiểm tra Canvas có trong scene không
2. Kiểm tra `HealthSlider` đã được assign trong Inspector chưa
3. Kiểm tra Layer và Sorting của Canvas

### ❌ Thanh máu không cập nhật khi bị damage
**Giải pháp:**
1. Kiểm tra Console log có lỗi không
2. Đảm bảo Player có script `PlayerHealth1968` hoặc `LinhGiaiPhong1968`
3. Kiểm tra Events đã được setup đúng:
   ```csharp
   // Trong PlayerHealth1968.cs và LinhGiaiPhong1968.cs
   public UnityEvent<int, int> OnHealthChanged;
   ```

### ❌ Fill Image không có màu
**Giải pháp:**
1. Select `Fill` trong Hierarchy
2. Kiểm tra component **Image** có Sprite và Color đã set
3. Đảm bảo `Use Gradient Color` đã bật trong `HealthBarUI`

### ❌ Thanh máu bị lỗi scale
**Giải pháp:**
1. Select `HealthSlider` → `Fill Area` → `Fill`
2. Kiểm tra **Rect Transform**:
   - Left: 0, Top: 0, Right: 0, Bottom: 0
3. Đảm bảo **Image Type**: Filled hoặc Simple

---

## 📝 CODE REFERENCE

### Script đã được update:
1. ✅ `PlayerHealth1968.cs` - Thêm `OnHealthChanged` event
2. ✅ `LinhGiaiPhong1968.cs` - Thêm `OnHealthChanged` event  
3. ✅ `HealthBarUI.cs` - Script mới để hiển thị thanh máu

### Cách hoạt động:
```
Player bị damage 
    ↓
PlayerHealth1968.TakeDamage() được gọi
    ↓
OnHealthChanged.Invoke(currentHealth, maxHealth)
    ↓
HealthBarUI.OnPlayerHealthChanged() được trigger
    ↓
UpdateUI() - Cập nhật Slider và Text NGAY LẬP TỨC ⚡
```

---

## 🎯 KẾT LUẬN

Thanh máu đã được setup với:
- ✅ Cập nhật real-time qua Events
- ✅ Không cần gọi Update() liên tục (tối ưu performance)
- ✅ Smooth transition animation
- ✅ Gradient color effect
- ✅ Hỗ trợ cả 2 loại player script

**Chúc bạn setup thành công! 🎮**

