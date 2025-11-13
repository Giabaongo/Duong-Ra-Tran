# 🎮 HƯỚNG DẪN TEST FIX - RESTART & BOMB KILL

## ✅ ĐÃ FIX XONG 3 VẤN ĐỀ:

1. ✅ **Enemy spawn lại khi restart game**
2. ✅ **Enemy kill count tăng khi bị bomb B52 giết**
3. ✅ **Victory screen hiện ra khi kill hết enemy**

---

## 🧪 TEST NGAY:

### **Test 1: RESTART GAME (3 phút)**

```bash
1. Play game trong Unity
2. Giết 1-2 enemy (bằng súng hoặc bomb)
3. Nhấn ESC → Pause Menu → "Chơi lại" / "Restart"
4. ✅ Kiểm tra: CÓ 6 ENEMY xuất hiện lại không?
```

**✅ PASS nếu:**
- Thấy 6 enemy spawn lại trong scene
- Enemy kill count reset về 0
- Console logs: `[EnemyHealth] 🔄 Cleared X dead enemy IDs for game restart!`

**❌ FAIL nếu:**
- Không thấy enemy nào
- Console logs: `⚠️ đã chết trước đó! DESTROYING RESPAWN CLONE!`

---

### **Test 2: BOMB GIẾT ENEMY (5 phút)**

```bash
1. Play game mới
2. Chờ 5 giây → B52 bay qua (nghe tiếng máy bay)
3. Đứng gần 1 enemy để bomb rơi trúng
4. ✅ Kiểm tra: Enemy có chết và kill count có tăng không?
```

**✅ PASS nếu:**
- Enemy chết ngay khi bomb nổ trúng (animation chết + biến mất)
- UI "Enemy Killed" tăng lên: `1/6`, `2/6`, v.v.
- Console logs: `💀💀💀 [EnemyHealth] Enemy has been killed!`
- Console logs: `[GameManager1968] ⚔️ Enemy killed! Progress: X/6`

**❌ FAIL nếu:**
- Bomb nổ trúng nhưng enemy không chết (chỉ nhấp nháy)
- Kill count không tăng
- Console logs: Không thấy `Enemy killed`

---

### **Test 3: VICTORY (10 phút)**

```bash
1. Play game mới
2. Giết hết 6 enemy (bằng súng hoặc bomb)
3. ✅ Kiểm tra: Victory UI có hiện ra không?
```

**✅ PASS nếu:**
- Khi kill enemy thứ 6 → Victory panel hiện ra ngay lập tức
- Console logs: `🎉🎉🎉 === VICTORY! === 🎉🎉🎉`
- Console logs: `[GameManager1968] Victory condition met: 6 >= 6`

**❌ FAIL nếu:**
- Kill hết 6 enemy nhưng không Victory
- Console logs: Kill count không tăng đủ

---

## 🔧 NẾU CÓ VẤN ĐỀ:

### **Vấn đề: Enemy vẫn không spawn lại**
```
→ Check: GameManager1968 có trong scene không?
→ Check: Console có log "Cleared X dead enemy IDs" không?
→ Fix: Restart Unity Editor và test lại
```

### **Vấn đề: Bomb không giết được enemy**
```
→ Check: Bomb có nổ trúng enemy không? (xem bán kính đỏ khi bomb nổ)
→ Check: Console logs có "Friendly fire! Enemy hit" không?
→ Fix: Mở B52_Bomb.prefab → B52 Bomb (Script) → Damage To Enemies = 50
```

### **Vấn đề: Victory không hiện**
```
→ Check: GameManager1968.Instance có null không? (xem Console)
→ Check: Victory UI có được assign trong GameManager không?
→ Fix: Hierarchy → GameManager → Inspector → Victory UI = "VictoryPanel"
```

---

## 📊 CHỈ SỐ QUAN TRỌNG:

### **Bomb Damage:**
- `damageToPlayer = 5` (B52 target chính là player)
- `damageToEnemies = 50` ✅ ĐỦ ĐỂ GIẾT 1 HIT!
- `explosionRadius = 3` (bán kính nổ)

### **Enemy Health:**
- `maxHealth = 20` (default)
- → Bomb 50 damage > 20 HP = Chết ngay! ✅

---

## ✅ KẾT QUẢ MONG ĐỢI:

```
1. Play game → 6 enemy spawn ✅
2. Bomb nổ → Enemy chết → Kill count +1 ✅
3. Kill 6/6 enemy → Victory UI hiện ✅
4. Restart → 6 enemy spawn lại ✅
5. Repeat!
```

---

## 🎯 HOÀN THÀNH!

**Nếu 3 test đều PASS → FIX THÀNH CÔNG! 🎉**

Bạn có thể:
- Chơi game bình thường
- Restart bao nhiêu lần cũng được
- Bomb B52 giờ có thể giết enemy
- Victory screen hoạt động đúng!

---

**🐛 Nếu có bug mới → Report ngay để fix tiếp!**

