# 🔧 FIX UNITY INSPECTOR VALUES - ĐỂ ÁP DỤNG CODE MỚI

## 🚨 VẤN ĐỀ

Code đã sửa nhưng **Unity Inspector vẫn giữ giá trị cũ**!

```
Code:      maxHealth = 20 ✅
Inspector: maxHealth = 5  ❌  ← Unity đang dùng cái này!
```

**Inspector value sẽ OVERRIDE code value!**

---

## ✅ FIX THỦ CÔNG (NHANH - 2 PHÚT)

### Bước 1: Fix Player Health
1. **Mở Unity Editor**
2. **Mở Scene `MauthanScene`**
3. **Tìm GameObject "Player" hoặc "LinhGiaiPhong1968" trong Hierarchy**
4. **Click vào Player**
5. **Xem Inspector → Tìm component `LinhGiaiPhong1968`**
6. **Tìm dòng `Max Health`**
7. **Thay đổi từ `5` → `20`**
8. **Ctrl+S để Save Scene**

### Bước 2: Fix Enemy Bullet Speed
1. **Tìm tất cả Enemy trong Hierarchy**
   - `Enemy (1)`, `Enemy (2)`, etc.
2. **Với mỗi Enemy:**
   - Click vào Enemy
   - Tìm component `Enermy1968Controller`
   - Tìm `Bullet Speed`
   - Thay đổi từ `10` → `6`
3. **Ctrl+S để Save**

### Bước 3: Fix Enemy Prefab (QUAN TRỌNG!)
1. **Vào thư mục `Assets/Prefabs/` (hoặc nơi có Enemy prefab)**
2. **Tìm Enemy prefab** (thường là file màu xanh)
3. **Click vào Enemy prefab**
4. **Xem Inspector → Component `Enermy1968Controller`**
5. **Thay đổi:**
   - `Bullet Speed`: `10` → `6`
   - `Initial Attack Delay`: Xem có chưa? Nếu chưa thì **PHẢI ADD!**
6. **Apply Changes**

---

## ✅ CÁCH 2: AUTO FIX BẰNG SCRIPT (KHUYÊN DÙNG)

Tạo script helper để tự động update tất cả!

### File: `FixGameBalance.cs`

```csharp
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FixGameBalance : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Fix Game Balance - Update All Values")]
    public static void FixAllValues()
    {
        Debug.Log("=== FIXING GAME BALANCE ===");
        
        int fixedCount = 0;
        
        // 1. Fix all Player instances
        LinhGiaiPhong1968[] players = FindObjectsOfType<LinhGiaiPhong1968>();
        foreach (var player in players)
        {
            SerializedObject so = new SerializedObject(player);
            SerializedProperty maxHealthProp = so.FindProperty("maxHealth");
            
            if (maxHealthProp != null && maxHealthProp.intValue != 20)
            {
                Debug.Log($"[Fix] Player '{player.name}' health: {maxHealthProp.intValue} → 20");
                maxHealthProp.intValue = 20;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(player);
                fixedCount++;
            }
        }
        
        // 2. Fix all Enemy instances
        Enermy1968Controller[] enemies = FindObjectsOfType<Enermy1968Controller>();
        foreach (var enemy in enemies)
        {
            SerializedObject so = new SerializedObject(enemy);
            SerializedProperty bulletSpeedProp = so.FindProperty("bulletSpeed");
            SerializedProperty initialDelayProp = so.FindProperty("initialAttackDelay");
            
            bool changed = false;
            
            if (bulletSpeedProp != null && bulletSpeedProp.floatValue != 6f)
            {
                Debug.Log($"[Fix] Enemy '{enemy.name}' bullet speed: {bulletSpeedProp.floatValue} → 6");
                bulletSpeedProp.floatValue = 6f;
                changed = true;
            }
            
            if (initialDelayProp != null && initialDelayProp.floatValue != 1f)
            {
                Debug.Log($"[Fix] Enemy '{enemy.name}' initial delay: {initialDelayProp.floatValue} → 1");
                initialDelayProp.floatValue = 1f;
                changed = true;
            }
            
            if (changed)
            {
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(enemy);
                fixedCount++;
            }
        }
        
        Debug.Log($"=== FIXED {fixedCount} OBJECTS ===");
        Debug.Log("✅ DONE! Save scene (Ctrl+S) to apply changes!");
        
        // Auto save scene
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );
    }
#endif
}
```

### Cách dùng:
1. Tạo file `Assets/Scripts/MauThan1968/FixGameBalance.cs`
2. Copy code trên vào
3. Quay lại Unity → Đợi compile
4. Click **`Tools → Fix Game Balance - Update All Values`** ở menu bar
5. Xem console log → Kiểm tra kết quả
6. **Save scene (Ctrl+S)**

---

## 🧪 KIỂM TRA SAU KHI FIX

### Test 1: Player Health
1. Play game
2. Bắn enemy
3. Xem log: `Health: X/20` ← Phải có `/20`!

### Test 2: Enemy Bullet Speed
1. Play game
2. Để enemy bắn
3. Xem log: `Speed: 6` ← Phải có `Speed: 6`!

### Test 3: Enemy Delay
1. Play game
2. Để enemy detect player
3. Xem log: `ENTERED Attack state - Initial delay: 1.0s`

---

## 💡 TẠI SAO CẦN FIX CẢ PREFAB?

**Khi spawn enemy mới:**
- Unity sẽ dùng **giá trị từ Prefab**
- Không phải từ code!
- Nên phải fix cả Prefab để enemy spawn sau cũng đúng!

---

## ⚠️ LƯU Ý

**Sau khi sửa Inspector values:**
- **PHẢI SAVE SCENE** (Ctrl+S)
- **PHẢI SAVE PREFAB** (Apply changes)
- **Không thì mất hết khi restart Unity!**

---

## 🎯 KẾT LUẬN

Unity **KHÔNG TỰ ĐỘNG** apply code changes vào Inspector!

**Phải làm 1 trong 2:**
1. Fix thủ công trong Inspector ✋
2. Dùng script helper tự động 🤖

**→ Sau khi fix → Test lại ngay!**

