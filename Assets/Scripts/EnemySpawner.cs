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

    void Start()
    {
        SpawnEnemy(streetRatPrefab, enemyDB.streetRat);
        SpawnEnemy(sewerRunnerPrefab, enemyDB.sewerRunner);
        SpawnEnemy(trashTankPrefab, enemyDB.trashTank);
        SpawnEnemy(kingRatPrefab, enemyDB.kingRat);
    }

    void SpawnEnemy(GameObject prefab, EnemyStats stats)
    {
        GameObject enemy = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        EnemyController controller = enemy.GetComponent<EnemyController>();

        controller.stats = stats;
        controller.currentWave = currentWave;
    }
}
    