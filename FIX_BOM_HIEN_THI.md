# ⚡ FIX: BOM KHÔNG HIỂN THI & EXPLOSION RADIUS

**Vấn đề:**
1. ❌ Không thấy bom khi rơi
2. ❌ Explosion radius quá lớn → Player xa vẫn bị damage

**Fix:** 2 phút

---

## 🔧 **FIX 1: HIỂN THỊ BOM (1 phút)**

### **Mở B52_Bomb prefab:**

1. **Project** → `Assets/Prefabs/Effects/B52_Bomb.prefab`
2. **Double click** mở prefab

### **A. Check Sprite:**

```
Component: Sprite Renderer
→ Sprite: Phải có sprite! (Kéo explosion frame đầu vào)
→ Color: White (255, 255, 255, 255)
→ Sorting Layer: Default
→ Order in Layer: 50 ⬅️ Cao để thấy!
```

### **B. Check Scale:**

```
Transform:
→ Scale: (0.4, 0.4, 0.4) hoặc lớn hơn ⬅️
   
   KHÔNG NHỎ HƠN 0.3!
```

**GỢI Ý SCALE:**
- **Nhỏ:** (0.4, 0.4, 0.4) - Realistic
- **Vừa:** (0.6, 0.6, 0.6) - Dễ thấy
- **Lớn:** (0.8, 0.8, 0.8) - Rất rõ

### **C. Check Collider:**

```
Component: Circle Collider 2D
→ Is Trigger: ✅ CHECKED
→ Radius: 2-3 (tùy scale)
```

### **D. Save**

Ctrl + S → **Save prefab!**

---

## 🔧 **FIX 2: EXPLOSION RADIUS (30 giây)**

### **Vấn đề:**
```
Explosion Radius = 3-4 units
→ Quá lớn!
→ Player xa vẫn bị damage
```

### **Fix trong B52_Bomb prefab:**

```
Component: B52 Bomb

💣 BOM SETTINGS:
→ Explosion Radius: 2.5 ⬅️ Giảm xuống!
   (Từ 3-4 → 2.5)

→ Damage To Player: 5-10 (Đã OK)
→ Damage To Enemies: 1-5 (Đã OK)
```

**GỢI Ý EXPLOSION RADIUS:**
- **Nhỏ (Realistic):** 1.5-2.0 - Phải gần mới chết
- **Vừa (Balanced):** 2.5-3.0 - Cân bằng
- **Lớn (Dangerous):** 3.5-4.5 - Rải thảm kinh hoàng

### **Save**

Ctrl + S

---

## 🎮 **TEST NGAY:**

1. **Click Play** ▶️

2. **Đợi B52 bay qua**

3. **Quan sát:**
   - ✅ **THẤY BOM RƠI XUỐNG!** (Sprite hiển thị rõ!)
   - ✅ Bom rơi + xoay tròn
   - ✅ Chạm đất → NỔ!
   - ✅ **Explosion radius nhỏ hơn** (chỉ bị damage khi GẦN)

4. **Scene View:**
   - Chọn bomb đang rơi
   - Thấy **Gizmos đỏ** = Bán kính nổ
   - Đo xem player có trong bán kính không

---

## 🎨 **TÙY CHỈNH VISUAL:**

### **Bom to hơn (dễ nhìn):**

```
B52_Bomb prefab:
→ Scale: (0.8, 0.8, 0.8)
→ Sorting Order: 60
```

### **Bom có màu sắc:**

```
Sprite Renderer:
→ Color: 
   - Đỏ (255, 100, 100) - Napalm
   - Xanh (100, 200, 255) - Thường
   - Vàng (255, 255, 100) - High explosive
```

### **Explosion nhỏ hơn (hard mode):**

```
B52 Bomb component:
→ Explosion Radius: 1.5
→ Damage To Player: 15-20 (Tăng damage, giảm radius)
```

---

## 🐛 **TROUBLESHOOTING:**

### **Vẫn không thấy bom:**

✅ **Check 1: Sprite có assigned?**
```
B52_Bomb prefab → Sprite Renderer → Sprite: PHẢI CÓ!
```

✅ **Check 2: Sorting Order:**
```
Order in Layer: >= 50
Nếu < 50 → Bị che bởi tilemap/ground
```

✅ **Check 3: Scale:**
```
Transform → Scale: >= (0.4, 0.4, 0.4)
Nếu < 0.3 → Quá nhỏ, khó thấy
```

✅ **Check 4: Camera:**
```
Bom có ở trong camera view không?
Xem Scene view khi play!
```

### **Vẫn bị damage dù xa:**

✅ **Check Explosion Radius:**
```
B52_Bomb prefab → B52 Bomb → Explosion Radius
Nếu = 4-5 → Quá lớn!
→ Giảm xuống 2.0-2.5
```

✅ **Visual Debug:**
```
Khi play:
1. Pause ngay khi bom nổ
2. Chọn bomb trong Hierarchy (trước khi destroy)
3. Xem Gizmos đỏ (explosion radius)
4. So sánh với vị trí Player
```

✅ **Distance Check:**
```
Console logs:
[Bomb] 💥 EXPLOSION at (x, y)
[B52Bomb] 💥 B52 hit Player!

→ Tính khoảng cách từ explosion đến player
→ Nếu > Explosion Radius mà vẫn hit = BUG!
```

---

## 📊 **SETTINGS ĐỀ XUẤT:**

### **🎮 Easy (Dễ né):**
```
Explosion Radius: 2.0 (Nhỏ)
Damage To Player: 5-10 (Nhẹ)
Bomb Scale: (0.8, 0.8, 0.8) (Lớn - dễ thấy)
Sorting Order: 60 (Rất cao)
```

### **⚔️ Normal (Cân bằng):**
```
Explosion Radius: 2.5 (Vừa)
Damage To Player: 10-15 (Vừa)
Bomb Scale: (0.6, 0.6, 0.6) (Vừa)
Sorting Order: 50
```

### **💀 Hard (Nguy hiểm!):**
```
Explosion Radius: 3.5 (Lớn!)
Damage To Player: 20-30 (Chết nhanh!)
Bomb Scale: (0.4, 0.4, 0.4) (Nhỏ - khó thấy!)
Sorting Order: 50
Fall Speed: 15 (Rơi nhanh!)
```

---

## 💡 **BONUS: VISUAL FEEDBACK TỐT HƠN**

### **1. Warning Circle (Unity):**

Tạo child object trong B52_Bomb:
```
1. Add child: "Danger_Circle"
2. Add Sprite Renderer
3. Sprite: Circle (hoặc tạo sprite đỏ tròn)
4. Color: Red (255, 0, 0, 100) - Trong suốt
5. Scale: (Explosion Radius * 2, Explosion Radius * 2, 1)
   VD: Radius = 2.5 → Scale (5, 5, 1)
6. Sorting Order: 49 (Dưới bom)
```

→ Khi bom rơi, thấy **vòng tròn đỏ** = vùng nguy hiểm!

### **2. Trail Effect:**

```
B52_Bomb prefab:
→ Add Component: Trail Renderer
→ Time: 0.5
→ Width: 0.1 → 0.05
→ Color: Orange/Yellow
→ Material: Default-Particle
```

→ Bom có đuôi khói khi rơi!

---

## ✅ **CHECKLIST:**

**ĐÃ FIXED IN CODE:**
- [x] Không tự động set scale = 0.3 ✅
- [x] Dùng scale từ prefab ✅
- [x] Set sorting order = 50 ✅
- [x] Improved collision detection ✅
- [x] Better gizmos visualization ✅

**CẦN LÀM IN UNITY:**
- [ ] Mở B52_Bomb.prefab
- [ ] Set Scale >= (0.4, 0.4, 0.4)
- [ ] Set Order in Layer = 50+
- [ ] Assign Sprite (nếu chưa có)
- [ ] Giảm Explosion Radius = 2.5
- [ ] Save prefab
- [ ] Test!

---

## 🎬 **KẾT QUẢ MONG ĐỢI:**

```
BEFORE (Lỗi):
❌ Không thấy bom
❌ Player xa vẫn bị damage
❌ Không biết bán kính nổ

AFTER (Fixed):
✅ Thấy bom rơi rõ ràng!
✅ Bom xoay tròn đẹp mắt
✅ Explosion radius hợp lý (2.5 units)
✅ Chỉ bị damage khi GẦN thật sự
✅ Visual feedback rõ ràng (Gizmos)
✅ Gameplay FAIR và THRILLING!
```

---

**LÀM THEO 2 FIX TRÊN VÀ ENJOY REALISTIC BOMBING! 💣💥✈️**

