// 06/11/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public GameResultUI gameResultUI; // se duoc tim tu dong neu bo trong
    public int totalEnemies;          // so luong dich con lai trong scene
    public PlayerHealth playerHealth; // se duoc tim tu dong neu bo trong

    [Header("Level Flow")]
    [SerializeField] private string[] levelOrder = { "Map1", "Map2" };
    [SerializeField] private string fallbackWinScene = "ChonMan";

    [Header("Story Flow")]
    [SerializeField] private string storySceneName = "StoryScene";
    public string StorySceneName => storySceneName;
    public string PendingMissionScene { get; private set; }
    public string PendingMissionTitle { get; private set; }
    public string PendingMissionDescription { get; private set; }

    [Header("Music")]
    [SerializeField] private string musicObjectName = "MusikBG";
    [SerializeField] private bool forceMusicLoop = true;
    private AudioSource musicSource;

    private bool isGameplayScene;
    private bool missingResultWarningShown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void Start()
    {
        AssignSceneReferences();
    }

    private void Update()
    {
        CheckGameResult();
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignSceneReferences();
    }

    private void AssignSceneReferences()
    {
        if (Instance != this)
        {
            return;
        }

        Time.timeScale = 1f; // dam bao gameplay tiep tuc khi scene moi load

        string sceneName = SceneManager.GetActiveScene().name;

        EnsurePersistentMusic();
        CleanupSceneMusicDuplicates();

        totalEnemies = FindObjectsOfType<EnemyHealth>().Length;
        Debug.Log($"GameManager: scene '{sceneName}' has {totalEnemies} enemies.");

        isGameplayScene = Array.IndexOf(levelOrder, sceneName) >= 0;
        if (!isGameplayScene)
        {
            gameResultUI = null;
            playerHealth = null;
            missingResultWarningShown = false;
            return;
        }

        if (gameResultUI == null)
        {
            gameResultUI = FindObjectOfType<GameResultUI>(true);
        }

        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
    }

    private void CheckGameResult()
    {
        if (Instance != this)
        {
            return;
        }

        if (!isGameplayScene)
        {
            return;
        }

        if (gameResultUI == null)
        {
            if (!missingResultWarningShown)
            {
                Debug.LogWarning("GameManager: GameResultUI not assigned.");
                missingResultWarningShown = true;
            }
            return;
        }
        missingResultWarningShown = false;

        if (totalEnemies <= 0)
        {
            if (!gameResultUI.IsShowing)
            {
                gameResultUI.ShowWin();
            }
        }

        if (playerHealth != null && playerHealth.CurrentHealth <= 0)
        {
            if (!gameResultUI.IsShowing)
            {
                gameResultUI.ShowLose();
            }
        }
    }

    public void EnemyDefeated()
    {
        totalEnemies = Mathf.Max(0, totalEnemies - 1);
        Debug.Log("Enemy defeated! Remaining enemies: " + totalEnemies);
        CheckGameResult();
    }

    public void HandleContinueAfterWin()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int index = Array.IndexOf(levelOrder, currentScene);

        Time.timeScale = 1f;

        if (index >= 0 && index + 1 < levelOrder.Length)
        {
            string nextScene = levelOrder[index + 1];
            Debug.Log($"GameManager: advancing from {currentScene} to {nextScene}");
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log($"GameManager: no next level for {currentScene}, loading fallback {fallbackWinScene}");
            SceneManager.LoadScene(fallbackWinScene);
        }
    }

    public void QueueMission(string sceneName, string storyTitle, string storyDesc)
    {
        PendingMissionScene = sceneName;
        PendingMissionTitle = storyTitle;
        PendingMissionDescription = storyDesc;
    }

    public string ConsumePendingMissionScene(string fallbackScene)
    {
        string target = string.IsNullOrEmpty(PendingMissionScene) ? fallbackScene : PendingMissionScene;
        PendingMissionScene = null;
        PendingMissionTitle = null;
        PendingMissionDescription = null;
        return target;
    }

    public void ClearPendingMission()
    {
        PendingMissionScene = null;
        PendingMissionTitle = null;
        PendingMissionDescription = null;
    }

    private void EnsurePersistentMusic()
    {
        if (musicSource != null)
        {
            if (!musicSource.isPlaying && musicSource.clip != null)
            {
                musicSource.Play();
            }
            return;
        }

        AudioSource candidate = null;

        if (!string.IsNullOrEmpty(musicObjectName))
        {
            GameObject musicGO = GameObject.Find(musicObjectName);
            if (musicGO != null)
            {
                candidate = musicGO.GetComponent<AudioSource>();
            }
        }

        if (candidate == null)
        {
            AudioSource[] sceneSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource src in sceneSources)
            {
                if (src == null || src.clip == null)
                {
                    continue;
                }

                if (src.loop || src.playOnAwake)
                {
                    candidate = src;
                    break;
                }
            }
        }

        if (candidate != null)
        {
            musicSource = candidate;
            DontDestroyOnLoad(musicSource.gameObject);

            if (forceMusicLoop)
            {
                musicSource.loop = true;
            }

            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("GameManager: could not find any AudioSource to use as background music.");
        }
    }

    private void CleanupSceneMusicDuplicates()
    {
        if (musicSource == null || string.IsNullOrEmpty(musicObjectName))
        {
            return;
        }

        AudioSource[] sceneSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource src in sceneSources)
        {
            if (src == null || src == musicSource)
            {
                continue;
            }

            if (src.gameObject.name == musicObjectName)
            {
                Destroy(src.gameObject);
            }
        }
    }
}
