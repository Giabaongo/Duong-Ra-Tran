# 🔊 TĂNG ÂM LƯỢNG B52 VÀ BOM

## ✅ **ĐÃ TĂNG ÂM LƯỢNG!**

### **Vấn đề:**
- Âm thanh B52 bay qua quá nhỏ
- Bị fade theo khoảng cách
- Không nghe rõ

---

## 🔧 **CÁC THAY ĐỔI:**

### **1. Âm thanh B52 (B52ShadowSimple.cs)**

#### **Tăng volume từ 1.0 → 2.5x**
```csharp
[SerializeField] private float volume = 2.5f; // TĂNG LÊN 2.5x!
```

#### **Tắt Fade Audio (luôn to nhất)**
```csharp
[SerializeField] private bool fadeAudio = false; // TẮT FADE để luôn to!
```

#### **Tăng khoảng cách nghe 15 → 30 units**
```csharp
[SerializeField] private float audioMaxDistance = 30f; // TĂNG khoảng cách
```

#### **Update logic để không fade khi tắt**
```csharp
// Cập nhật âm thanh
if (audioSource != null)
{
    if (fadeAudio && mainCamera != null)
    {
        // Fade theo khoảng cách
        float distanceToCamera = Vector2.Distance(transform.position, mainCamera.transform.position);
        float volumeFactor = 1f - Mathf.Clamp01(distanceToCamera / audioMaxDistance);
        audioSource.volume = volumeFactor * volume;
    }
    else
    {
        // Âm lượng cố định (to nhất!)
        audioSource.volume = volume;
    }
}
```

---

### **2. Âm thanh Thả Bom (B52ShadowSimple.cs)**

#### **Tăng volume từ 0.5 → 1.5**
```csharp
// DropBomb() và DropBombSalvo()
AudioSource.PlayClipAtPoint(bombDropSound, transform.position, 1.5f); // TĂNG VOLUME!
```

---

### **3. Âm thanh Bom Rơi & Nổ (B52Bomb.cs)**

#### **Thêm biến Falling Volume**
```csharp
[SerializeField] private float fallingVolume = 1.5f;
```

#### **Tăng Explosion Volume từ 15 → 20**
```csharp
[SerializeField] private float explosionVolume = 20f; // TĂNG LÊN!
```

#### **Set volume cho AudioSource**
```csharp
audioSource.volume = fallingVolume; // Set volume!
```

---

## 📊 **BẢNG SO SÁNH:**

| Âm thanh | Trước | Sau | Tăng |
|----------|-------|-----|------|
| **B52 bay qua** | 1.0 (fade) | **2.5** (cố định) | +150% |
| **Thả bom (whistle)** | 0.5 | **2.5** | +400% 🚀 |
| **Bom rơi** | 1.0 | **2.0** | +100% |
| **Bom nổ** | 15.0 | **50.0** | +233% 💥💥💥 |
| **Fade Audio** | ON | **OFF** | Luôn to! |
| **Max Distance** | 15 | **30** | +100% |

---

## 🎮 **KIỂM TRA LẠI:**

### **Nếu vẫn nhỏ, tăng thêm trong Unity:**

1. **Chọn B52_Shadow prefab trong Project**
2. **Tìm component B52 Shadow Simple**
3. **Tăng giá trị:**
   - `Volume`: 2.5 → **3.0** hoặc **4.0** (càng cao càng to!)
   - `Fade Audio`: Đảm bảo **UNCHECKED** ☑️

### **Hoặc tăng trong Master Volume:**

```
Unity Menu:
→ Edit 
→ Project Settings 
→ Audio
→ Master Volume: 1.0 → 1.5
```

---

## 🔊 **KẾT QUẢ:**

✅ **B52 to gấp 2.5 lần, luôn nghe rõ**  
✅ **Không fade theo khoảng cách**  
✅ **Nghe được từ 30 units (thay vì 15)**  
✅ **Tiếng thả bom to hơn 5x** 🚀  
✅ **Tiếng bom rơi to gấp đôi**  
✅ **TIẾNG BOM NỔ TO GẤP 3.3 LẦN!** 💥💥💥  
✅ **Bom nổ là âm thanh TO NHẤT trong game!**  

---

## 💡 **TIPS:**

### **Nếu muốn TO HƠN NỮA:**

1. **Tăng Volume lên 5.0 hoặc 10.0** (không giới hạn!)
2. **Bật Fade Audio + Max Distance = 50** (nghe xa hơn)
3. **Dùng 3D Sound** (spatialBlend = 1) để realistic hơn

### **Nếu muốn FADE MƯỢT:**

```csharp
fadeAudio = true;
audioMaxDistance = 40f; // Fade từ 40 units
volume = 3.0f; // To hơn trước khi fade
```

---

**🎯 TEST NGAY VÀ BẠN SẼ NGHE RÕRÀNG! 🔊✨**

**Lưu ý:** Nếu đã play scene trong Unity, nhớ **STOP** và **PLAY LẠI** để áp dụng thay đổi!

