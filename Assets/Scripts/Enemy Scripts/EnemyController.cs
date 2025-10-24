using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Information")]
    public EnemyStats stats;
    public int currentWave = 1;
    private int goldReward;

    private EnemyHealth health;
    private EnemyMovement movement;
    private bool isDead = false;

    public static event System.Action<EnemyController> OnEnemyKilled;

    void Start()
    {
        health = GetComponent<EnemyHealth>();
        movement = GetComponent<EnemyMovement>();

        if (stats != null)
            goldReward = stats.GetGoldReward(currentWave);
    }

    public void TakeDamage(float amount)
    {
        health?.TakeDamage(amount);
    }

    public void OnDeath()
    {
        if (isDead) return;
        isDead = true;

        // Reward player
        PlayerManager.Instance?.AddGold(goldReward);

        // Stop movement & play death animation
        movement?.Die();

        // Notify GameController
        OnEnemyKilled?.Invoke(this);

        // Destroy after delay
        Destroy(gameObject, 2f);
    }
}
