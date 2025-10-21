using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Game/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    public EnemyStats streetRat = new EnemyStats
    {
        enemyName = "Street Rat",
        baseHP = 5f,
        hpGrowthPerWave = 1.5f,
        speed = 1f,
        baseGold = 2
    };

    public EnemyStats sewerRunner = new EnemyStats
    {
        enemyName = "Sewer Runner",
        baseHP = 3f,
        hpGrowthPerWave = 1f,
        speed = 2.5f,
        baseGold = 1
    };

    public EnemyStats trashTank = new EnemyStats
    {
        enemyName = "Trash Tank",
        baseHP = 10f,
        hpGrowthPerWave = 2f,
        speed = 0.75f,
        baseGold = 3
    };

    public EnemyStats kingRat = new EnemyStats
    {
        enemyName = "King Rat",
        baseHP = 20f,
        hpGrowthPerWave = 0f, // No scaling
        speed = 0.75f,
        baseGold = 5
    };
}
