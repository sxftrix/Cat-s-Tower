using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStats stats;
    public int currentWave = 1;
    private float currentHP;
    private int goldReward;

    private EnemyMovement movement;

    void Start()
    {
        // Initialize from the EnemyStats data
        currentHP = stats.GetHP(currentWave);
        goldReward = stats.GetGoldReward(currentWave);
        movement = GetComponent<EnemyMovement>();

        Debug.Log($"{stats.enemyName} spawned! HP: {currentHP}, Gold: {goldReward}");
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        // TODO: Add gold to player wallet
        Debug.Log($"{stats.enemyName} defeated! +{goldReward} gold");

        // Optional: play death animation before destroy
        Destroy(gameObject);
    }
}
