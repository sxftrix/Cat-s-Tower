using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyDatabase enemyDB;

    [Header("Enemy Prefabs")]
    public GameObject streetRatPrefab;
    public GameObject sewerRunnerPrefab;
    public GameObject trashTankPrefab;
    public GameObject kingRatPrefab;

    public int currentWave = 1;
    public Transform spawnPoint; // optional spawn location

    [Header("Spawn Settings")]
    public float minSpawnDelay = 0.5f;
    public float maxSpawnDelay = 1.5f;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        // Determine counts per enemy type (proportional to wave)
        int streetCount = Mathf.Max(1, currentWave + 2);
        int sewerCount = currentWave >= 3 ? Mathf.Max(0, currentWave - 2) : 0;
        int trashCount = currentWave >= 6 ? Mathf.Max(0, currentWave - 4) : 0;

        // Spawn Street Rats
        for (int i = 1; i < streetCount; i++)
        {
            SpawnEnemy(streetRatPrefab, enemyDB.streetRat);
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }

        // Spawn Sewer Runners if unlocked
        for (int i = 0; i < sewerCount; i++)
        {
            SpawnEnemy(sewerRunnerPrefab, enemyDB.sewerRunner);
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }

        // Spawn Trash Tanks if unlocked
        for (int i = 0; i < trashCount; i++)
        {
            SpawnEnemy(trashTankPrefab, enemyDB.trashTank);
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }

        // Spawn King Rat if final wave
        if (currentWave == 10) // or last wave
        {
            SpawnEnemy(kingRatPrefab, enemyDB.kingRat);
        }
    }

    void SpawnEnemy(GameObject prefab, EnemyStats stats)
    {
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;

        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        EnemyController controller = enemy.GetComponent<EnemyController>();
        controller.stats = stats;
        controller.currentWave = currentWave;
    }
}
