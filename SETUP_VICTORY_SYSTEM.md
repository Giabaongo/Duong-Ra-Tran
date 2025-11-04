# HUONG DAN SETUP HE THONG THANG (VICTORY SYSTEM)

## DA TAO 4 FILE MOI:
1. **GameManager1968.cs** - Da cap nhat them Victory system
2. **EnemyHealth1968.cs** - Enemy co the bi tieu diet
3. **EnemyCounterUI.cs** - Hien thi so enemy da tieu diet
4. **PlayerBullet1968.cs** - Bullet cua player

---

## CACH SETUP (6 BUOC):

### BUOC 1: Setup Enemies (Cac Enemy)
**LAM VOI TAT CA 6 ENEMY:**

1. Chon moi Enemy GameObject (Enemy 1, 2, 3, 4, 5, 6)
2. **Add Component**: EnemyHealth1968
3. Cai dat trong Inspector:
   ```
   Health Settings:
   - Max Health: 50 (hoac tuy chon)
   
   Visual Feedback:
   - Sprite Renderer: Keo Sprite Renderer vao
   - Damage Color: Mau do
   ```
4. Dam bao Enemy co **Tag = "Enemy"**
5. Dam bao Enemy da co **Enemy1968** component

### BUOC 2: Setup Bullet cua Player
1. Tim Bullet Prefab cua Player (hoac GameObject Bullet)
2. **Add Component**: PlayerBullet1968
3. Cai dat:
   ```
   Bullet Settings:
   - Damage: 25 (2 phat la chet neu enemy co 50 HP)
   - Speed: 10
   - Lifetime: 5
   
   Target Settings:
   - Destroy On Impact: Bat (check)
   ```
4. Dat **Tag cua Bullet = "PlayerBullet"** hoac "Bullet"
5. Dam bao Bullet co:
   - Rigidbody2D (Gravity Scale = 0)
   - Collider2D (hoac Trigger)

### BUOC 3: Tao Tag "PlayerBullet"
1. Chon bat ky GameObject nao
2. Click Tag dropdown (o tren cung Inspector)
3. Click "Add Tag..."
4. Click dau **+**
5. Nhap ten: **PlayerBullet**
6. Save
7. Quay lai Bullet prefab, set Tag = "PlayerBullet"

### BUOC 4: Setup Victory UI Panel
1. **TAO VICTORY PANEL:**
   - Chuot phai trong Canvas
   - UI -> Panel
   - Doi ten: "VictoryPanel"

2. **THEM TEXT "VICTORY!":**
   - Chuot phai vao VictoryPanel
   - UI -> Text (TextMeshPro) hoac Text
   - Doi text: "VICTORY!"
   - Canh giua, phong to chu, mau vang

3. **THEM BUTTON "PLAY AGAIN":**
   - Chuot phai vao VictoryPanel
   - UI -> Button
   - Doi text: "Play Again"
   - Setup Button On Click():
     * Click **+**
     * Keo **GameManager** vao o None
     * Chon: GameManager1968.RestartGame

4. **TAT PANEL BAN DAU:**
   - Chon VictoryPanel
   - Tat dau check (de an di ban dau)
   - Se tu hien khi thang

### BUOC 5: Setup GameManager
1. Chon GameObject co **GameManager1968**
2. Trong Inspector:
   ```
   Victory Settings:
   - Total Enemies: 6 (se tu dong dem)
   
   UI References:
   - Game Over UI: Keo GameOverPanel vao
   - Victory UI: Keo VictoryPanel vao day!
   ```

### BUOC 6: Setup Enemy Counter UI (Tuy chon)
**Hien thi "Enemies: 3/6" tren man hinh:**

1. Tao UI Text:
   - Chuot phai vao Canvas
   - UI -> Text (TextMeshPro)
   - Doi ten: "EnemyCounterText"
   - Vi tri: O goc tren ben trai

2. **Add Component**: EnemyCounterUI

3. Cai dat:
   ```
   UI References:
   - Counter Text: Keo chinh no vao (EnemyCounterText)
   - Text Format: "Enemies: {0}/{1}"
   ```

---

## CACH HOAT DONG:

```
Player ban bullet
    |
    v
Bullet cham Enemy
    |
    v
EnemyHealth1968.TakeDamage() duoc goi
    |
    v
Enemy HP giam
    |
    v
Neu HP <= 0:
    EnemyHealth1968.Die()
    |
    v
    GameManager1968.EnemyKilled()
    |
    v
    Dem enemy: 1/6, 2/6, 3/6...
    |
    v
Khi 6/6:
    GameManager1968.Victory()
    |
    v
    Victory UI hien len!
```

---

## CHECKLIST:

- [ ] Tat ca 6 Enemy co EnemyHealth1968
- [ ] Tat ca 6 Enemy co tag "Enemy"
- [ ] Bullet co PlayerBullet1968
- [ ] Bullet co tag "PlayerBullet"
- [ ] Bullet co Rigidbody2D va Collider2D
- [ ] Victory Panel da tao trong Canvas
- [ ] Victory Panel da keo vao GameManager (Victory UI)
- [ ] Button Play Again da link den RestartGame
- [ ] GameManager co trong scene

---

## TEST:

1. **Chay game**
2. **Ban vao enemy** (xem Console co log "Enemy took damage")
3. **Tieu diet het 6 enemy**
4. **Victory UI phai hien len!**
5. Console se hien:
   ```
   [EnemyHealth] Enemy took 25 damage...
   Enemy killed! 1/6
   Enemy killed! 2/6
   ...
   Enemy killed! 6/6
   === VICTORY! ===
   Victory UI is now VISIBLE
   ```

---

## DIEU CHINH DAMAGE:

**Neu enemy kho chet qua:**
- Tang damage cua bullet (PlayerBullet1968: Damage = 50)
- Hoac giam HP cua enemy (EnemyHealth1968: Max Health = 25)

**Neu enemy de chet qua:**
- Giam damage cua bullet (Damage = 10)
- Hoac tang HP cua enemy (Max Health = 100)

---

## NEU KHONG THANG:

1. Kiem tra Console co log "Enemy killed!" khong
2. Kiem tra so enemy: Console se hien "Total enemies: 6"
3. Kiem tra Victory UI da duoc assign vao GameManager chua
4. Kiem tra tat ca enemy co tag "Enemy"
5. Kiem tra bullet co tag "PlayerBullet"

---

## XONG!

Bay gio ban co:
- ✅ He thong tieu diet enemy
- ✅ He thong dem enemy
- ✅ Victory UI khi thang
- ✅ Game Over UI khi thua
- ✅ Bullet gay sat thuong cho enemy

CHUC BAN VIET GAME THANH CONG! 🎮🎉

