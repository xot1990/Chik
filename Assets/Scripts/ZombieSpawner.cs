using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    private List<WaveData> waves;
    private int currentWaveIndex;
    private bool isSpawning = false;

    void OnEnable()
    {
        EventBus.OnWaveEnd += StartNextWave;
        EventBus.OnWaveStart += HandleWaveStart;
    }
    void OnDisable()
    {
        EventBus.OnWaveEnd -= StartNextWave;
        EventBus.OnWaveStart -= HandleWaveStart;
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
    }

    public void StartNextWave()
    {
        if (!isSpawning) return;

        if (currentWaveIndex + 1 >= waves.Count)
        {
            Debug.Log("Все волны пройдены!");
            EventBus.RaiseOnWaveEnd();
            isSpawning = false;
            return;
        }

        currentWaveIndex++;
        WaveData waveData = waves[currentWaveIndex];
        StartCoroutine(SpawnWave(waveData));
        EventBus.RaiseOnWaveStart();
    }

    IEnumerator SpawnWave(WaveData waveData)
    {

        for (int i = 0; i < waveData.zombieCount; i++)
        {
            SpawnZombie(waveData.zombiePrefab);
            yield return new WaitForSeconds(waveData.zombieSpawnDelay);
        }
        EventBus.RaiseOnWaveEnd();
    }
    private void HandleWaveStart()
    {
        Debug.Log("Начало волны!");
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
}