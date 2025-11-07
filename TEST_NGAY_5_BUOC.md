# ⚡ TEST NGAY - 5 BƯỚC XÁC ĐỊNH VẤN ĐỀ

## 🎯 MỤC TIÊU
Tìm ra **CHÍNH XÁC** tại sao đạn xuyên tường!

---

## 📝 BƯỚC 1: KIỂM TRA BUILDING (30 GIÂY)

```
Unity Editor:
1. Hierarchy → Grid → Building (click vào)
2. Inspector → Xem có 3 components này KHÔNG:

   ☐ TilemapCollider2D
   ☐ CompositeCollider2D  
   ☐ Rigidbody2D

3. GHI LẠI KẾT QUẢ:
   - Có đủ 3 cái → Ghi "CÓ ĐỦ"
   - Thiếu → Ghi "THIẾU: ___"
```

**KẾT QUẢ CỦA BẠN:** _____________

---

## 🔍 BƯỚC 2: XEM COLLIDER TRONG SCENE VIEW (15 GIÂY)

```
1. Click vào Building trong Hierarchy
2. Chuyển sang Scene View (tab bên cạnh Game)
3. Gizmos (góc trên phải) → Bật ON (màu xanh)
4. Nhìn vào tòa nhà:

   Có viền XANH LÁ quanh tòa nhà? (Có/Không)
```

**KẾT QUẢ CỦA BẠN:** _____________

---

## 🎮 BƯỚC 3: TEST TRONG GAME (30 GIÂY)

```
1. Save tất cả (Ctrl+S)
2. Play game ▶
3. Bắn 1 phát vào tường
4. Stop game ■
5. Xem Console window (Ctrl+Shift+C để mở)

   Console có dòng nào bắt đầu với "★★★" KHÔNG?
   
   Có → Ghi lại NỘI DUNG: _______________
   Không → Ghi "KHÔNG CÓ LOG"
```

**KẾT QUẢ CỦA BẠN:** _____________

---

## 📊 BƯỚC 4: PHÂN TÍCH KẾT QUẢ

### **Nếu Console KHÔNG CÓ LOG "★★★":**
```
→ Đạn KHÔNG va chạm với BẤT KỲ thứ gì!
→ Vấn đề: LAYERS hoặc COLLIDER SETTINGS
→ Đi đến BƯỚC 5
```

### **Nếu Console CÓ LOG nhưng KHÔNG có "Building":**
```
★★★ PLAYER BULLET HIT: Ground, Layer: 0, Tag: ''
                        ^^^^^^ 
                        Tên GameObject bị trúng

→ Đạn VA CHẠM nhưng KHÔNG PHẢI với Building!
→ Vấn đề: Có thể Ground có collider, Building KHÔNG có
→ Đi đến BƯỚC 5
```

### **Nếu Console CÓ LOG "Building":**
```
★★★ PLAYER BULLET HIT: Building, Layer: 0, Tag: 'Wall'

→ Đạn ĐÃ VA CHẠM với Building!
→ Vấn đề: Code KHÔNG DESTROY đạn
→ Kiểm tra tag "Wall" trong code
```

---

## 🔧 BƯỚC 5: FIX THEO KẾT QUẢ

### **Fix A: Nếu Building THIẾU Collider**

```
Hierarchy → Grid → Building
→ Add Component → "Tilemap Collider 2D" → Enter
→ Add Component → "Composite Collider 2D" → Enter
→ TilemapCollider2D: ✓ Check "Used By Composite"
→ Rigidbody2D (tự động): Body Type = Static
→ Save (Ctrl+S)
→ Quay lại BƯỚC 3 test lại
```

### **Fix B: Nếu Không có LOG "★★★"**

```
Vấn đề: Layer Collision Matrix

1. Edit → Project Settings → Physics 2D
2. Cuộn xuống "Layer Collision Matrix"
3. Tìm hàng "Default" và cột "Default"
4. Ô giao nhau PHẢI có ✓ (checked)
5. Nếu không có → Click vào để check
6. Close Project Settings
7. Save scene (Ctrl+S)
8. Quay lại BƯỚC 3 test lại
```

### **Fix C: Nếu có LOG nhưng đạn không biến mất**

```
Vấn đề: Tag không khớp hoặc code logic

Kiểm tra PlayerBullet.cs line 93-96:
```

```csharp
// Destroy if hit wall or obstacle
if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle")
{
    Destroy(gameObject);
}
```

```
Building Tag PHẢI là "Wall" (đúng chữ hoa chữ thường!)

Hierarchy → Grid → Building
→ Inspector → Tag: Wall (PHẢI khớp với code!)
```

---

## 📋 CHECKLIST ĐẦY ĐỦ

```
☐ Building có TilemapCollider2D
☐ Building có CompositeCollider2D
☐ Building có Rigidbody2D (Body Type: Static)
☐ Scene View thấy viền xanh lá quanh Building
☐ Console có log "★★★ BULLET HIT" khi bắn
☐ Console log có chứa "Building"
☐ Building Tag = "Wall" (chính xác)
☐ Bullet Layer = Default
☐ Building Layer = Default
☐ Layer Collision Matrix: Default ✓ Default
```

---

## 🎯 GỬI TÔI KẾT QUẢ

Copy và điền vào 3 câu hỏi này:

```
1. Building có đủ 3 components (TilemapCollider2D, CompositeCollider2D, Rigidbody2D)?
   → Trả lời: _____________

2. Scene View có thấy viền xanh lá quanh Building?
   → Trả lời: _____________

3. Console log khi bắn vào tường (copy y nguyên):
   → Trả lời: _____________
```

Gửi 3 câu trả lời này cho tôi, tôi sẽ biết CHÍNH XÁC vấn đề! 🔍

---

## 💡 GHI NHỚ

```
Console KHÔNG CÓ LOG = Không va chạm = COLLIDER/LAYER vấn đề
Console CÓ LOG = Có va chạm = CODE LOGIC vấn đề
```

---

Làm xong 5 bước và gửi kết quả nhé! 🚀

