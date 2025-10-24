using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public EnemySpawner spawner;
    private int enemiesAlive = 0;
    private int currentWave = 1;

    void OnEnable()
    {
        EnemyController.OnEnemyKilled += EnemyDied;
    }

    void OnDisable()
    {
        EnemyController.OnEnemyKilled -= EnemyDied;
    }

    void Start()
    {
        StartCoroutine(StartWave(currentWave));
    }

    IEnumerator StartWave(int wave)
    {
        currentWave = wave;

        // Count enemies for this wave
        enemiesAlive = spawner.EstimateEnemyCountForWave(wave);

        yield return spawner.SpawnWave(wave); // Spawn enemies

        // Wait until all enemies die
        while (enemiesAlive > 0)
            yield return null;

        // All enemies dead → next wave
        StartCoroutine(StartWave(currentWave + 1));
    }

    void EnemyDied(EnemyController enemy)
    {
        enemiesAlive--;
    }
}
