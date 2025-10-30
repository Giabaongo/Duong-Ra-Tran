# 🔍 DEBUG: Tại Sao Đạn Vẫn Xuyên Tường?

## 📊 THÔNG TIN BẠN CUNG CẤP

✅ **Đã làm đúng:**
- Bullet Player: Layer = Default ✓
- Bullet Enemy: Layer = Default ✓
- Building: Layer = Default ✓
- Building: Tag = "Wall" ✓

⚠️ **LƯU Ý:**
- Sorting Layer (Foreground, Building) → CHỈ ảnh hưởng RENDER, KHÔNG ảnh hưởng COLLISION!
- Layer (Default) → Ảnh hưởng COLLISION!

---

## 🎯 NGUYÊN NHÂN CÓ THỂ

### 1. Building CHƯA CÓ Collider (80% khả năng)
### 2. Collider bị tắt (Enabled = false)
### 3. Rigidbody2D sai Body Type
### 4. Prefab changes chưa được apply

---

## ✅ KIỂM TRA NGAY (TRONG UNITY)

### **BƯỚC 1: Kiểm tra Building Components**

```
1. Hierarchy → Grid → Building
2. Inspector → Xem có những components này không:

   Building GameObject:
   ├─ Transform ✓
   ├─ Tilemap ✓
   ├─ Tilemap Renderer ✓
   ├─ TilemapCollider2D ← PHẢI CÓ!
   ├─ CompositeCollider2D ← PHẢI CÓ!
   └─ Rigidbody2D ← PHẢI CÓ!

   Nếu THIẾU bất kỳ cái nào → ĐÓ LÀ VẤN ĐỀ!
```

### **BƯỚC 2: Kiểm tra Collider Settings**

```
Nếu ĐÃ CÓ TilemapCollider2D:

TilemapCollider2D:
├─ Enabled: ✓ (PHẢI check)
├─ Is Trigger: ✗ (KHÔNG check)
└─ Used By Composite: ✓ (PHẢI check nếu có CompositeCollider2D)

CompositeCollider2D:
├─ Enabled: ✓ (PHẢI check)
├─ Is Trigger: ✗ (KHÔNG check)
└─ Geometry Type: Polygons

Rigidbody2D:
├─ Body Type: Static (PHẢI là Static!)
└─ Simulated: ✓ (PHẢI check)
```

### **BƯỚC 3: Kiểm tra Bullet Collider**

```
Project → Prefabs → Bullet_Player:

Bullet_Player Prefab:
├─ Layer: Default ✓
├─ CircleCollider2D:
│  ├─ Enabled: ✓
│  └─ Is Trigger: ✓ (phải TRUE để gọi OnTriggerEnter2D)
├─ Rigidbody2D:
│  ├─ Body Type: Dynamic
│  └─ Simulated: ✓
└─ PlayerBullet (script): ✓

Làm tương tự với Bullet_Enemy!
```

---

## 🔧 FIX THEO TỪNG TRƯỜNG HỢP

### **TRƯỜNG HỢP 1: Building THIẾU Collider**

❌ **Nếu Building KHÔNG CÓ TilemapCollider2D:**

```
1. Hierarchy → Grid → Building
2. Inspector → Add Component
3. Gõ: "Tilemap Collider 2D" → Enter
4. Add Component
5. Gõ: "Composite Collider 2D" → Enter
6. Trong TilemapCollider2D: ✓ Check "Used By Composite"
7. Rigidbody2D (tự động thêm): Body Type = Static
8. Save scene (Ctrl+S)
```

### **TRƯỜNG HỢP 2: Collider bị tắt**

❌ **Nếu TilemapCollider2D Enabled = unchecked:**

```
1. Click vào checkbox "Enabled" bên cạnh TilemapCollider2D
2. Click vào checkbox "Enabled" bên cạnh CompositeCollider2D
3. Save scene
```

### **TRƯỜNG HỢP 3: Rigidbody2D sai Body Type**

❌ **Nếu Rigidbody2D Body Type = Dynamic:**

```
1. Hierarchy → Grid → Building
2. Inspector → Rigidbody2D
3. Body Type: Static (PHẢI là Static!)
4. Save scene
```

### **TRƯỜNG HỢP 4: Is Trigger sai**

❌ **Nếu TilemapCollider2D Is Trigger = checked:**

```
1. Hierarchy → Grid → Building
2. TilemapCollider2D → Is Trigger: ✗ (bỏ check)
3. CompositeCollider2D → Is Trigger: ✗ (bỏ check)
4. Save scene
```

---

## 🎮 TEST NHANH

### **Test 1: Xem Collider trong Scene View**

```
1. Scene View (tab bên cạnh Game View)
2. Gizmos button (góc trên phải) → Bật ON
3. Click vào Building trong Hierarchy
4. Xem có viền xanh lá quanh tòa nhà không?
   
   CÓ viền xanh lá = Có collider ✓
   KHÔNG có viền = Thiếu collider ❌
```

### **Test 2: Dùng CollisionDebugger**

```
1. Hierarchy → Main Camera (hoặc GameObject nào cũng được)
2. Inspector → Add Component
3. Gõ: "Collision Debugger" → Enter
4. Play game ▶
5. Xem góc trên trái màn hình:
   
   Building: ✓ TC, ✓ CC, ✓ RB = OK ✓
   Building: ✗ TC, ✗ CC, ✗ RB = THIẾU ❌
```

---

## 🐛 DEBUG CODE (THÊM VÀO PlayerBullet.cs)

Thêm debug log để xem đạn có va chạm gì không:

```csharp
private void OnTriggerEnter2D(Collider2D collision)
{
    // ★ THÊM DÒNG NÀY ĐỂ DEBUG
    Debug.Log($"★★★ BULLET HIT: {collision.gameObject.name}, Layer: {collision.gameObject.layer}, Tag: {collision.tag}");
    
    // Ignore collision with player
    if (collision.CompareTag("Player"))
    {
        Debug.Log("→ Ignored player");
        return;
    }
    
    // ... code còn lại
}
```

Thêm vào **EnemyBullet.cs** tương tự:

```csharp
private void OnTriggerEnter2D(Collider2D collision)
{
    // ★ THÊM DÒNG NÀY
    Debug.Log($"★★★ ENEMY BULLET HIT: {collision.gameObject.name}, Layer: {collision.gameObject.layer}, Tag: {collision.tag}");
    
    // Check if hit player
    if (collision.CompareTag("Player"))
    {
        // ... code
    }
}
```

**Sau đó:**
1. Save scripts
2. Play game ▶
3. Bắn vào tường
4. Xem Console → Nếu KHÔNG có log "BULLET HIT" = Không va chạm!

---

## 📸 GỬI TÔI THÔNG TIN NÀY

Để tôi giúp bạn chính xác, hãy chụp screenshot và gửi:

### **Screenshot 1: Building Inspector**
```
Hierarchy → Grid → Building (click vào)
→ Chụp toàn bộ Inspector panel
→ Phải thấy TẤT CẢ components
```

### **Screenshot 2: Bullet_Player Inspector**
```
Project → Prefabs → Bullet_Player (click vào)
→ Chụp toàn bộ Inspector
→ Phải thấy Layer, Collider, Rigidbody
```

### **Screenshot 3: Console Logs**
```
Play game → Bắn vào tường
→ Chụp Console window
→ Phải thấy logs (nếu có)
```

---

## 🎯 CÂU HỎI NHANH

**Hãy trả lời giúp tôi:**

1. Building CÓ **TilemapCollider2D** component không? (Có/Không)
2. Building CÓ **CompositeCollider2D** component không? (Có/Không)
3. Building CÓ **Rigidbody2D** component không? (Có/Không)
4. Rigidbody2D **Body Type** là gì? (Dynamic/Kinematic/Static)
5. Trong Scene View, bạn CÓ THẤY viền xanh lá quanh tòa nhà không? (Có/Không)

---

## 💡 GHI NHỚ

```
Sorting Layer (Foreground, Building) = CHỈ cho RENDER (hiển thị)
Layer (Default, Layer 8) = Cho PHYSICS (va chạm)

Đạn xuyên tường = PHYSICS vấn đề
→ Kiểm tra LAYER và COLLIDERS!
```

---

Trả lời 5 câu hỏi trên hoặc gửi screenshots giúp tôi, tôi sẽ biết chính xác vấn đề! 🔍

