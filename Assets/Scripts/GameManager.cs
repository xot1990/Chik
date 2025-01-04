using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState
{
    MainMenu,
    InGame,
    LevelComplete,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState gameState;
    public int currentLevel = 0;
    public List<LevelData> levels;

    private ObjectPlacer objectPlacer;
    private ZombieSpawner zombieSpawner;

    void OnEnable()
    {
        EventBus.OnLevelEnd += OnLevelEnd;
         EventBus.OnGameStart += HandleGameStart;
          EventBus.OnGameOver += HandleGameOver;
          EventBus.OnLevelRestart += HandleLevelRestart;
    }
    void OnDisable()
    {
        EventBus.OnLevelEnd -= OnLevelEnd;
        EventBus.OnGameStart -= HandleGameStart;
       EventBus.OnGameOver -= HandleGameOver;
       EventBus.OnLevelRestart -= HandleLevelRestart;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        objectPlacer = FindObjectOfType<ObjectPlacer>();
        if (objectPlacer == null)
        {
            Debug.LogError("�� ������ ObjectPlacer � �����!");
            enabled = false;
        }
        zombieSpawner = FindObjectOfType<ZombieSpawner>();
        if (zombieSpawner == null)
        {
            Debug.LogError("�� ������ ZombieSpawner � �����!");
            enabled = false;
        }
        gameState = GameState.MainMenu;
        StartGame();
    }

    public void StartGame()
    {
        gameState = GameState.InGame;
        LoadLevel(currentLevel);
        EventBus.RaiseOnGameStart();
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError("������� �� ������!");
            return;
        }
        objectPlacer.RestartLevel();
        currentLevel = levelIndex;
        LevelData currentLevelData = levels[levelIndex];

        zombieSpawner.SetWaveData(currentLevelData.waves);
        zombieSpawner.StartSpawning();
        StartCoroutine(StartFirstWave(currentLevelData.waveStartDelay));
    }

    IEnumerator StartFirstWave(float delay)
    {
        yield return new WaitForSeconds(delay);
        zombieSpawner.StartNextWave();
    }

    private void OnLevelEnd(bool win)
    {
        if (win)
        {
            LevelComplete();
        }
        else
        {
            GameOver();
        }
    }

    public void LevelComplete()
    {
        gameState = GameState.LevelComplete;
        Debug.Log("������� �������");
        objectPlacer.EndLevel();
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3f);
        currentLevel++;
        if (currentLevel >= levels.Count)
        {
            Debug.Log("��� ������ ��������!");
            gameState = GameState.MainMenu;
            yield break;
        }
        StartGame();
    }
      private void HandleGameOver()
    {
         gameState = GameState.GameOver;
        Debug.Log("���� ��������");
       objectPlacer.EndLevel();
         EventBus.RaiseOnGameOver();
    }
    private void HandleLevelRestart()
    {
          Debug.Log("���������� ������");
    }
       private void HandleGameStart()
    {
         Debug.Log("������ ����");
    }

    public void GameOver()
    {
          EventBus.RaiseOnGameOver();
    }


    public bool IsGameActive()
    {
        return gameState == GameState.InGame;
    }
}

[System.Serializable]
public class LevelData
{
    public List<WaveData> waves;
    public float waveStartDelay;
}

[System.Serializable]
public class WaveData
{
    public int zombieCount;
    public GameObject zombiePrefab;
    public float zombieSpawnDelay;
}