# ⚡ SETUP NHANH CAMERA SHAKE (2 PHÚT!)

## 🎯 **LÀM THEO 2 BƯỚC:**

### **1️⃣ TÌM CAMERA CỦA BẠN:**

#### **Nếu có Cinemachine (camera tracking player):**

```
Hierarchy → Tìm "CM vcam1" hoặc "VirtualCamera"
→ Add Component → CameraShake
→ Use Cinemachine: ☑️ CHECK!
→ Add Extension → CinemachineBasicMultiChannelPerlin
→ Noise Profile: "6D Shake"
```

#### **Nếu dùng Main Camera thường:**

```
Hierarchy → Main Camera
→ Add Component → CameraShake
→ Use Cinemachine: ☐ UNCHECK!
```

---

### **2️⃣ PLAY VÀ TEST:**

```
Play → Chờ B52 bay qua → Bom nổ
→ 💥 TIẾNG NỔ TO! (Volume 80)
→ 📳 CAMERA RUNG!
```

---

## ✅ **XONG!**

**Nếu rung quá yếu:**
```
Project → B52_Bomb.prefab
→ Shake Intensity: 1.2 → 2.0
```

**Nếu rung quá mạnh:**
```
Chọn Camera
→ CameraShake component
→ Max Shake Magnitude: 0.5 → 0.3
```

---

## 📊 **THAY ĐỔI:**

| Thay đổi | Trước | SAU |
|----------|-------|-----|
| Tiếng nổ | 50 | **80** (+60%) |
| Camera shake | ❌ Không có | ✅ **CÓ!** |

---

**🎯 ĐƠN GIẢN VẬY THÔI! 📳💥**

