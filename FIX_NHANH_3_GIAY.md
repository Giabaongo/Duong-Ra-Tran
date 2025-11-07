# ⚡ FIX NGAY - Đạn Player Không Thấy

## 🎯 CHẠY TEST TRƯỚC

Để biết chính xác vấn đề:

```
1. Save (Ctrl+S)
2. Play game
3. Bắn 1 phát
4. Xem Console log - tìm dòng:
   ★ PLAYER BULLET spawned at (...), scale: (...)
```

## 📊 SAU ĐÓ FIX DỰA TRÊN LOG

### NẾU THẤY: scale: (0.01, 0.01, 0.01)

→ **Scale quá nhỏ!** Fix:

```
Unity:
1. Project → Prefabs → Bullet_Player
2. Transform → Scale: 
   X: 0.05 (hoặc 0.1)
   Y: 0.05 (hoặc 0.1)
   Z: 0.05 (hoặc 0.1)
3. Save
```

### NẾU KHÔNG THẤY DÒng ★ PLAYER BULLET

→ **Prefab không gắn!** Fix:

```
Unity:
1. Hierarchy → Player
2. Inspector → LinhGiaiPhong1968
3. Bullet Prefab: Kéo Bullet_Player vào
4. Save scene
```

### NẾU CÓ LOG NHƯNG VẪN KHÔNG THẤY ĐẠN

→ **Sorting Layer!** Fix:

```
Unity:
1. Project → Prefabs → Bullet_Player
2. Inspector → Sprite Renderer
3. Order in Layer: 10 (hoặc số cao)
4. Save
```

## 🎮 FIX NHANH NHẤT (KHÔNG CẦN XEM LOG)

Làm cả 2 việc sau:

### 1. Tăng Scale:
```
Bullet_Player prefab
→ Transform → Scale: 0.05, 0.05, 0.05
→ Save
```

### 2. Tăng Order:
```
Bullet_Player prefab
→ Sprite Renderer → Order in Layer: 10
→ Save
```

### 3. Test:
```
Play → Bắn → Thấy đạn chưa?
```

## ✅ SAU KHI THẤY ĐẠN

Có thể điều chỉnh:
- **Scale nhỏ hơn** nếu đạn quá to: 0.03 hoặc 0.02
- **Order nhỏ hơn** nếu đạn che các object khác: 5 hoặc 3

---

**Làm FIX NHANH NHẤT trước, 2 phút fix xong! ⚡**

