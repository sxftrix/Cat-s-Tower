using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Database")]
    public EnemyDatabase enemyDB;

    [Header("Enemy Prefabs")]
    public GameObject streetRatPrefab;
    public GameObject sewerRunnerPrefab;
    public GameObject trashTankPrefab;
    public GameObject kingRatPrefab;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public Transform spawnPoint; // optional spawn location
    public float minSpawnDelay = 0.5f;
    public float maxSpawnDelay = 1.5f;

    void Start()
    {

    }

    public IEnumerator SpawnWave(int wave)
    {
        currentWave = wave; // optional, for internal use

        // Determine counts per enemy type (proportional to wave)
        int streetCount = Mathf.Max(1, wave + 2);
        int sewerCount = wave >= 3 ? Mathf.Max(0, wave - 2) : 0;
        int trashCount = wave >= 6 ? Mathf.Max(0, wave - 4) : 0;

        // Spawn Street Rats
        for (int i = 0; i < streetCount; i++)
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
        if (wave == 10)
            SpawnEnemy(kingRatPrefab, enemyDB.kingRat);
    }


    void SpawnEnemy(GameObject prefab, EnemyStats stats)
    {
        if (prefab == null || stats == null) return;

        // Grid parent
        Transform gridParent = GameObject.Find("Grid")?.transform;

        // Spawn position
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;

        // Instantiate enemy
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity, gridParent);

        // Reset rotation
        enemy.transform.rotation = Quaternion.identity;

        // Set sorting order
        foreach (var sr in enemy.GetComponentsInChildren<SpriteRenderer>())
            sr.sortingOrder = 3;

        // Assign stats and wave
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.stats = stats;
            controller.currentWave = currentWave;
        }

        // Randomly choose Top or Bottom path
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            bool useTopPath = Random.value > 0.5f;
            movement.pathChoice = useTopPath ? EnemyMovement.PathType.Top : EnemyMovement.PathType.Bottom;
        }
    }

    // Optional helper to estimate total enemies for a wave
    public int EstimateEnemyCountForWave(int wave)
    {
        int streetCount = Mathf.Max(1, wave + 2);
        int sewerCount = wave >= 3 ? Mathf.Max(0, wave - 2) : 0;
        int trashCount = wave >= 6 ? Mathf.Max(0, wave - 4) : 0;
        int kingCount = wave == 10 ? 1 : 0;

        return streetCount + sewerCount + trashCount + kingCount;
    }
}
