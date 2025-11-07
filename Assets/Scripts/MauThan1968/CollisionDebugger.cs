using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Script debug để hiển thị collision boundaries trong Game View
/// Đính script này vào bất kỳ GameObject nào (ví dụ: Main Camera)
/// </summary>
public class CollisionDebugger : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private bool showPlayerCollider = true;
    [SerializeField] private bool showEnemyCollider = true;
    [SerializeField] private bool showBuildingCollider = true;
    [SerializeField] private bool showGroundCollider = false;
    [SerializeField] private bool showDebugGUI = true;
    
    [Header("Colors")]
    [SerializeField] private Color playerColor = Color.green;
    [SerializeField] private Color enemyColor = Color.red;
    [SerializeField] private Color buildingColor = Color.yellow;
    [SerializeField] private Color groundColor = Color.cyan;

    private GameObject player;
    private GameObject[] enemies;
    private GameObject buildingTilemap;
    private GameObject groundTilemap;

    void Start()
    {
        // Tìm GameObjects
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        // Tìm tilemaps
        GameObject grid = GameObject.Find("Grid");
        if (grid != null)
        {
            Transform buildingTransform = grid.transform.Find("Building");
            Transform groundTransform = grid.transform.Find("Ground");
            
            if (buildingTransform != null)
                buildingTilemap = buildingTransform.gameObject;
            
            if (groundTransform != null)
                groundTilemap = groundTransform.gameObject;
        }

        Debug.Log("=== COLLISION DEBUGGER ===");
        Debug.Log($"Player found: {player != null}");
        Debug.Log($"Enemies found: {enemies.Length}");
        Debug.Log($"Building tilemap found: {buildingTilemap != null}");
        Debug.Log($"Ground tilemap found: {groundTilemap != null}");
    }

    void OnDrawGizmos()
    {
        // Draw Player Collider
        if (showPlayerCollider && player != null)
        {
            DrawColliders(player, playerColor);
        }

        // Draw Enemy Colliders
        if (showEnemyCollider && enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                    DrawColliders(enemy, enemyColor);
            }
        }

        // Draw Building Collider
        if (showBuildingCollider && buildingTilemap != null)
        {
            DrawTilemapColliders(buildingTilemap, buildingColor);
        }

        // Draw Ground Collider (usually none)
        if (showGroundCollider && groundTilemap != null)
        {
            DrawTilemapColliders(groundTilemap, groundColor);
        }
    }

    private void DrawColliders(GameObject obj, Color color)
    {
        Gizmos.color = color;

        // BoxCollider2D
        BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Vector2 pos = obj.transform.position;
            Vector2 offset = boxCollider.offset;
            Vector2 size = boxCollider.size;
            Vector2 scale = obj.transform.localScale;

            Vector2 center = pos + offset;
            Vector2 halfSize = new Vector2(size.x * scale.x, size.y * scale.y) / 2f;

            // Draw box
            Vector3 topLeft = new Vector3(center.x - halfSize.x, center.y + halfSize.y, 0);
            Vector3 topRight = new Vector3(center.x + halfSize.x, center.y + halfSize.y, 0);
            Vector3 bottomLeft = new Vector3(center.x - halfSize.x, center.y - halfSize.y, 0);
            Vector3 bottomRight = new Vector3(center.x + halfSize.x, center.y - halfSize.y, 0);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }

        // CapsuleCollider2D
        CapsuleCollider2D capsuleCollider = obj.GetComponent<CapsuleCollider2D>();
        if (capsuleCollider != null)
        {
            Vector2 pos = obj.transform.position;
            Vector2 offset = capsuleCollider.offset;
            Vector2 size = capsuleCollider.size;
            Vector2 scale = obj.transform.localScale;

            Vector2 center = pos + offset;
            float width = size.x * scale.x;
            float height = size.y * scale.y;

            // Draw capsule as simplified box
            Gizmos.DrawWireSphere(center, Mathf.Max(width, height) / 2f);
        }

        // CircleCollider2D
        CircleCollider2D circleCollider = obj.GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            Vector2 pos = obj.transform.position;
            Vector2 offset = circleCollider.offset;
            float radius = circleCollider.radius;
            Vector2 scale = obj.transform.localScale;

            Vector2 center = pos + offset;
            float scaledRadius = radius * Mathf.Max(scale.x, scale.y);

            Gizmos.DrawWireSphere(center, scaledRadius);
        }
    }

    private void DrawTilemapColliders(GameObject tilemap, Color color)
    {
        Gizmos.color = color;

        // CompositeCollider2D
        CompositeCollider2D compositeCollider = tilemap.GetComponent<CompositeCollider2D>();
        if (compositeCollider != null)
        {
            // Draw composite collider paths
            for (int i = 0; i < compositeCollider.pathCount; i++)
            {
                Vector2[] path = new Vector2[compositeCollider.GetPathPointCount(i)];
                compositeCollider.GetPath(i, path);

                for (int j = 0; j < path.Length; j++)
                {
                    Vector2 worldPoint1 = tilemap.transform.TransformPoint(path[j]);
                    Vector2 worldPoint2 = tilemap.transform.TransformPoint(path[(j + 1) % path.Length]);
                    Gizmos.DrawLine(worldPoint1, worldPoint2);
                }
            }
        }

        // TilemapCollider2D (if not using composite)
        TilemapCollider2D tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();
        if (tilemapCollider != null && compositeCollider == null)
        {
            // Draw simple bounds
            Bounds bounds = tilemapCollider.bounds;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }

    // GUI hiển thị thông tin
    void OnGUI()
    {
        // Chỉ hiện GUI khi game đang chạy và được bật
        if (!Application.isPlaying || !showDebugGUI)
            return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 14;
        style.normal.textColor = Color.white;
        style.padding = new RectOffset(10, 10, 10, 10);

        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("=== COLLISION DEBUG ===", style);
        
        if (player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            Collider2D col = player.GetComponent<Collider2D>();
            style.normal.textColor = (rb != null && col != null) ? Color.green : Color.red;
            GUILayout.Label($"Player: {(rb != null ? "✓" : "✗")} RB, {(col != null ? "✓" : "✗")} Collider", style);
        }
        else
        {
            style.normal.textColor = Color.red;
            GUILayout.Label("Player: ✗ Not Found", style);
        }

        if (buildingTilemap != null)
        {
            TilemapCollider2D tc = buildingTilemap.GetComponent<TilemapCollider2D>();
            CompositeCollider2D cc = buildingTilemap.GetComponent<CompositeCollider2D>();
            Rigidbody2D rb = buildingTilemap.GetComponent<Rigidbody2D>();
            
            style.normal.textColor = (tc != null && cc != null && rb != null) ? Color.green : Color.red;
            GUILayout.Label($"Building: {(tc != null ? "✓" : "✗")} TC, {(cc != null ? "✓" : "✗")} CC, {(rb != null ? "✓" : "✗")} RB", style);
            
            if (rb != null)
            {
                style.normal.textColor = (rb.bodyType == RigidbodyType2D.Static) ? Color.green : Color.yellow;
                GUILayout.Label($"  Body Type: {rb.bodyType}", style);
            }
        }
        else
        {
            style.normal.textColor = Color.red;
            GUILayout.Label("Building: ✗ Not Found", style);
        }

        style.normal.textColor = Color.cyan;
        GUILayout.Label($"\nEnemies: {enemies?.Length ?? 0} found", style);

        GUILayout.EndArea();
    }
}

