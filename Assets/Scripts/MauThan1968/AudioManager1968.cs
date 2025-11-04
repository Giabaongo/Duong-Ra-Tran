using UnityEngine;

/// <summary>
/// Quản lý nhạc nền và sound effects cho game
/// Singleton - tồn tại xuyên suốt các scenes
/// </summary>
public class AudioManager1968 : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Music Clips")]
    [SerializeField] private AudioClip menuMusic;      // Nhạc menu/start
    [SerializeField] private AudioClip gameMusic;      // Nhạc trong game
    [SerializeField] private AudioClip victoryMusic;   // Nhạc khi thắng
    [SerializeField] private AudioClip gameOverMusic;  // Nhạc khi thua
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip enemyHitSound;
    [SerializeField] private AudioClip playerHitSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip buttonClickSound;
    
    [Header("Settings")]
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float sfxVolume = 0.7f;
    [SerializeField] private bool playOnAwake = true;
    
    private static AudioManager1968 instance;
    
    public static AudioManager1968 Instance => instance;
    
    private void Awake()
    {
        // Singleton pattern - chỉ tồn tại 1 instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không destroy khi chuyển scene
            
            InitializeAudioSources();
            
            Debug.Log("[AudioManager1968] Initialized");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void InitializeAudioSources()
    {
        // Tạo AudioSource cho music nếu chưa có
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        
        // Tạo AudioSource cho SFX nếu chưa có
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        
        // Set volume
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }
    
    #region Music Control
    
    /// <summary>
    /// Chơi nhạc menu
    /// </summary>
    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }
    
    /// <summary>
    /// Chơi nhạc game
    /// </summary>
    public void PlayGameMusic()
    {
        PlayMusic(gameMusic);
    }
    
    /// <summary>
    /// Chơi nhạc victory
    /// </summary>
    public void PlayVictoryMusic()
    {
        PlayMusic(victoryMusic);
    }
    
    /// <summary>
    /// Chơi nhạc game over
    /// </summary>
    public void PlayGameOverMusic()
    {
        PlayMusic(gameOverMusic);
    }
    
    /// <summary>
    /// Chơi nhạc bất kỳ
    /// </summary>
    private void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[AudioManager1968] Audio clip is null!");
            return;
        }
        
        // Nếu đang chơi clip khác, chuyển sang clip mới
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
            Debug.Log($"[AudioManager1968] Playing music: {clip.name}");
        }
    }
    
    /// <summary>
    /// Dừng nhạc
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    /// <summary>
    /// Pause nhạc
    /// </summary>
    public void PauseMusic()
    {
        musicSource.Pause();
    }
    
    /// <summary>
    /// Resume nhạc
    /// </summary>
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
    
    #endregion
    
    #region Sound Effects
    
    /// <summary>
    /// Chơi sound effect bắn
    /// </summary>
    public void PlayShootSound()
    {
        PlaySFX(shootSound);
    }
    
    /// <summary>
    /// Chơi sound effect enemy bị đánh
    /// </summary>
    public void PlayEnemyHitSound()
    {
        PlaySFX(enemyHitSound);
    }
    
    /// <summary>
    /// Chơi sound effect player bị đánh
    /// </summary>
    public void PlayPlayerHitSound()
    {
        PlaySFX(playerHitSound);
    }
    
    /// <summary>
    /// Chơi sound effect enemy chết
    /// </summary>
    public void PlayEnemyDeathSound()
    {
        PlaySFX(enemyDeathSound);
    }
    
    /// <summary>
    /// Chơi sound effect click button
    /// </summary>
    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSound);
    }
    
    /// <summary>
    /// Chơi sound effect bất kỳ
    /// </summary>
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    /// <summary>
    /// Chơi SFX với volume tùy chỉnh
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
    
    #endregion
    
    #region Volume Control
    
    /// <summary>
    /// Set volume cho music
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }
    
    /// <summary>
    /// Set volume cho SFX
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }
    
    /// <summary>
    /// Mute/Unmute music
    /// </summary>
    public void ToggleMusicMute()
    {
        musicSource.mute = !musicSource.mute;
    }
    
    /// <summary>
    /// Mute/Unmute SFX
    /// </summary>
    public void ToggleSFXMute()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    
    #endregion
}

