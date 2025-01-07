using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    private List<WaveData> waves;
    private int currentWaveIndex;
    private bool isSpawning = false;
    private Coroutine currentSpawnCoroutine;

    void OnEnable()
    {
        EventBus.OnWaveEnd += HandleWaveEnd;
        EventBus.OnWaveStart += HandleWaveStart;
    }

    void OnDisable()
    {
        EventBus.OnWaveEnd -= HandleWaveEnd;
        EventBus.OnWaveStart -= HandleWaveStart;
        if (currentSpawnCoroutine != null)
        {
            StopCoroutine(currentSpawnCoroutine);
        }
    }

    void Start()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("Не найдена точка спавна!");
            enabled = false;
        }
    }

    public void SetWaveData(List<WaveData> waveData)
    {
        waves = waveData;
        currentWaveIndex = -1;
    }

    public void StartSpawning()
    {
        isSpawning = true;
        StartNextWave();
    }


    public void StartNextWave()
    {
        if (!isSpawning) return;

        if (currentWaveIndex + 1 >= waves.Count)
        {
            Debug.Log("Все волны пройдены!");
             isSpawning = false;
             EventBus.RaiseOnWaveEnd(); // Вызываем событие окончания уровня
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

    private void SpawnZombie(GameObject zombiePrefab)
    {
        if (zombiePrefab == null)
        {
            Debug.LogError("Префаб зомби не указан!");
            return;
        }
         Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
    }
   private void HandleWaveEnd()
    {
          if(isSpawning) {
                 StartNextWave();
             }
       }

     private void HandleWaveStart()
       {
           Debug.Log("Начало волны!");
       }
}