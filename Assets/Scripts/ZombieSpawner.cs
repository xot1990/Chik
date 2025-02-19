using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    private List<WaveData> waves;
    private int currentWaveIndex;
    private bool isSpawning = false;
    private Coroutine currentSpawnCoroutine;
    
    private bool waveEnd;
    private int currentZombieCount;

    void OnEnable()
    {
        EventBus.OnWaveEnd += HandleWaveEnd;
        EventBus.OnWaveStart += HandleWaveStart;
        EventBus.OnZombieDied += CheckWin;
        EventBus.OnGameExit += ExitGame;
    }

    void OnDisable()
    {
        EventBus.OnWaveEnd -= HandleWaveEnd;
        EventBus.OnWaveStart -= HandleWaveStart;
        EventBus.OnZombieDied -= CheckWin;
        EventBus.OnGameExit -= ExitGame;
        if (currentSpawnCoroutine != null)
        {
            StopCoroutine(currentSpawnCoroutine);
        }
    }

    void Start()
    {
        
    }

    public void ExitGame()
    {
        waveEnd = false;
        isSpawning = false;
        if(currentSpawnCoroutine != null)
            StopCoroutine(currentSpawnCoroutine);
        currentZombieCount = 0;
    }

    public void SetWaveData(List<WaveData> waveData)
    {
        waves = waveData;
        currentWaveIndex = -1;
    }

    public void StartSpawning()
    {
        waveEnd = false;
        isSpawning = true;
        StartNextWave();
    }


    public void StartNextWave()
    {
        if (!isSpawning) return;

        if (currentWaveIndex + 1 >= waves.Count)
        {
            Debug.Log("��� ����� ��������!");
            waveEnd = true;
             isSpawning = false;
             EventBus.RaiseOnWaveEnd(); // �������� ������� ��������� ������
            return;
        }

        currentWaveIndex++;
        WaveData waveData = waves[currentWaveIndex];
        if (currentSpawnCoroutine != null)
        {
             StopCoroutine(currentSpawnCoroutine);
        }
         currentSpawnCoroutine = StartCoroutine(SpawnWave(waveData));

         EventBus.RaiseOnWaveStart();
    }

    IEnumerator SpawnWave(WaveData waveData)
    {
       for (int i = 0; i < waveData.zombieCount; i++)
        {
            SpawnZombie(waveData.zombiePrefab);
            yield return new WaitForSeconds(waveData.zombieSpawnDelay);
        }
        currentSpawnCoroutine = null;
         EventBus.RaiseOnWaveEnd();

    }

    private void CheckWin(GameObject gameObject)
    {
        currentZombieCount--;
        
        if (currentZombieCount <= 0 && waveEnd)
            EventBus.RaiseOnLevelEnd(true);
    }

    private void SpawnZombie(GameObject zombiePrefab)
    {
        if (zombiePrefab == null)
        {
            Debug.LogError("������ ����� �� ������!");
            return;
        }
         Instantiate(zombiePrefab, spawnPoints[Random.Range(0,spawnPoints.Count)].position, Quaternion.identity);
         currentZombieCount++;
    }
   private void HandleWaveEnd()
    {
          if(isSpawning) {
                 StartNextWave();
             }
       }

     private void HandleWaveStart()
       {
           Debug.Log("������ �����!");
       }
}