using UnityEngine;

/// <summary>
/// Script tự động phát nhạc khi scene load
/// Attach vào GameObject trong mỗi scene
/// </summary>
public class SceneMusicTrigger : MonoBehaviour
{
    [Header("Music Type")]
    [SerializeField] private MusicType musicType = MusicType.Game;
    [SerializeField] private bool playOnStart = true;
    
    public enum MusicType
    {
        Menu,       // Nhạc menu/start
        Game,       // Nhạc trong game
        Victory,    // Nhạc khi thắng
        GameOver    // Nhạc khi thua
    }
    
    private void Start()
    {
        if (playOnStart)
        {
            PlaySceneMusic();
        }
    }
    
    /// <summary>
    /// Phát nhạc theo loại scene
    /// </summary>
    public void PlaySceneMusic()
    {
        if (AudioManager1968.Instance == null)
        {
            Debug.LogWarning("[SceneMusicTrigger] AudioManager1968 not found in scene!");
            return;
        }
        
        switch (musicType)
        {
            case MusicType.Menu:
                AudioManager1968.Instance.PlayMenuMusic();
                Debug.Log("[SceneMusicTrigger] Playing Menu Music");
                break;
                
            case MusicType.Game:
                AudioManager1968.Instance.PlayGameMusic();
                Debug.Log("[SceneMusicTrigger] Playing Game Music");
                break;
                
            case MusicType.Victory:
                AudioManager1968.Instance.PlayVictoryMusic();
                Debug.Log("[SceneMusicTrigger] Playing Victory Music");
                break;
                
            case MusicType.GameOver:
                AudioManager1968.Instance.PlayGameOverMusic();
                Debug.Log("[SceneMusicTrigger] Playing Game Over Music");
                break;
        }
    }
    
    /// <summary>
    /// Dừng nhạc
    /// </summary>
    public void StopMusic()
    {
        if (AudioManager1968.Instance != null)
        {
            AudioManager1968.Instance.StopMusic();
        }
    }
}

