# 🔧 FIX: Screen position out of view frustum

## ⚠️ CẢNH BÁO
```
Screen position out of view frustum (screen pos 540.000000, 338.000000, 0.000000) 
(Camera rect 0 0 1035 501)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)
```

---

## 🎯 NGUYÊN NHÂN

Warning này xuất hiện khi:
- Script có `OnGUI()` cố gắng vẽ GUI ở vị trí ngoài camera
- GUI được vẽ khi game chưa Play (trong Editor mode)
- Canvas/GUI position không hợp lệ

Trong trường hợp này: **CollisionDebugger.cs** đang vẽ GUI cả khi ở Editor mode.

---

## ✅ ĐÃ FIX

Tôi đã cập nhật `CollisionDebugger.cs`:

### Trước (có warning):
```csharp
void OnGUI()
{
    GUIStyle style = new GUIStyle();
    // ... vẽ GUI
}
```

### Sau (không warning):
```csharp
void OnGUI()
{
    // Chỉ hiện GUI khi game đang chạy và được bật
    if (!Application.isPlaying || !showDebugGUI)
        return;

    GUIStyle style = new GUIStyle();
    // ... vẽ GUI
}
```

---

## 🎮 CÁCH DÙNG

### Option mới: Show Debug GUI

Trong Inspector → CollisionDebugger:
```
Show Debug GUI: ✓ (bật - hiện thông tin debug)
Show Debug GUI: ✗ (tắt - không hiện gì cả)
```

### Khi nào dùng?

**Bật (✓) khi:**
- Đang debug collision
- Muốn xem thông tin component realtime
- Test trong Play mode

**Tắt (✗) khi:**
- Không cần debug nữa
- Build game (để tối ưu performance)
- Chỉ muốn xem Gizmos trong Scene View

---

## 🔍 KIỂM TRA

### Test xem đã fix chưa:

1. **Trong Editor (không Play):**
   - Console → Không còn warning "Screen position out of view frustum" ✅

2. **Khi Play game:**
   - Nếu `Show Debug GUI` = ✓ → Thấy thông tin debug góc trên trái ✅
   - Nếu `Show Debug GUI` = ✗ → Không thấy text debug (nhưng vẫn thấy Gizmos) ✅

3. **Console sạch sẽ:**
   - Không còn warning liên quan GUI ✅

---

## 💡 LƯU Ý

### Script vẫn hoạt động bình thường:
- ✅ `OnDrawGizmos()` vẫn hiện collider boundaries trong Scene View
- ✅ `OnGUI()` chỉ hiện khi Play game và được bật
- ✅ Không ảnh hưởng đến gameplay

### Performance:
- `OnGUI()` chạy mỗi frame → Tốn performance
- Nên **TẮT** `Show Debug GUI` khi không cần
- Hoặc **XÓA script** khi build game ra

---

## 🎨 CÁC OPTION DEBUG

```
CollisionDebugger Component:
│
├─ Show Player Collider: ✓/✗
│  └─ Hiện/ẩn collider của Player (Gizmos)
│
├─ Show Enemy Collider: ✓/✗
│  └─ Hiện/ẩn collider của Enemy (Gizmos)
│
├─ Show Building Collider: ✓/✗
│  └─ Hiện/ẩn collider của Building (Gizmos)
│
├─ Show Ground Collider: ✓/✗
│  └─ Hiện/ẩn collider của Ground (Gizmos)
│
└─ Show Debug GUI: ✓/✗ (MỚI)
   └─ Hiện/ẩn thông tin text góc trên trái (OnGUI)
```

---

## 🆘 NẾU VẪN CÒN WARNING

### Từ CollisionDebugger:
- ❌ **KHÔNG THỂ XẢY RA NỮA** - Đã fix bằng `Application.isPlaying` check

### Từ script khác:
1. Tìm script có `OnGUI()` trong Console error message
2. Thêm check:
```csharp
void OnGUI()
{
    if (!Application.isPlaying)
        return;
    
    // ... GUI code của bạn
}
```

### Từ Canvas:
1. Hierarchy → Tìm Canvas
2. Inspector → Canvas → Render Mode:
   - Screen Space - Overlay (thường không có vấn đề)
   - Screen Space - Camera (kiểm tra Camera được assign đúng)
   - World Space (kiểm tra vị trí Canvas)

---

## 📋 CHECKLIST

```
[ ] CollisionDebugger đã được cập nhật (có check Application.isPlaying)
[ ] Console không còn warning "Screen position out of view frustum"
[ ] Play game → Thấy debug info nếu Show Debug GUI = ✓
[ ] Stop game → Không có warning trong Console
[ ] Script hoạt động bình thường
```

---

## 🎉 KẾT LUẬN

Warning đã được fix! Script giờ chỉ vẽ GUI khi:
1. ✅ Game đang chạy (`Application.isPlaying`)
2. ✅ `Show Debug GUI` được bật

Bạn có thể yên tâm dùng script debug mà không lo warning! 🚀

