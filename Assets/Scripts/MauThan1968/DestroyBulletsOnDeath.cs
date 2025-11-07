using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Optional: Destroy all bullets fired by enemy when it dies
/// Attach this to Enemy prefab if you want bullets to disappear when enemy dies
/// </summary>
public class DestroyBulletsOnDeath : MonoBehaviour
{
    private List<GameObject> firedBullets = new List<GameObject>();
    
    /// <summary>
    /// Call this when enemy fires a bullet to track it
    /// </summary>
    public void TrackBullet(GameObject bullet)
    {
        if (bullet != null)
        {
            firedBullets.Add(bullet);
            Debug.Log($"[DestroyBulletsOnDeath] Tracking bullet: {bullet.name}");
        }
    }
    
    /// <summary>
    /// Call this when enemy dies to destroy all tracked bullets
    /// </summary>
    public void DestroyAllBullets()
    {
        int destroyedCount = 0;
        
        foreach (var bullet in firedBullets)
        {
            if (bullet != null)
            {
                Debug.Log($"[DestroyBulletsOnDeath] 💥 Destroying bullet: {bullet.name}");
                Destroy(bullet);
                destroyedCount++;
            }
        }
        
        firedBullets.Clear();
        
        if (destroyedCount > 0)
        {
            Debug.Log($"[DestroyBulletsOnDeath] ✅ Destroyed {destroyedCount} bullet(s) from {gameObject.name}");
        }
    }
    
    private void OnDestroy()
    {
        // Auto cleanup when enemy is destroyed
        DestroyAllBullets();
    }
}

