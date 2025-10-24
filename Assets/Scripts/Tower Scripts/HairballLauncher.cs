using UnityEngine;
using System.Text;

public class HairballLauncher : MonoBehaviour
{
    public TowerDatabase towerDatabase;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int level = 1;

    private TowerStats stats;
    private float fireCooldown;
    private int shotCount = 0;

    void Start()
    {
        stats = towerDatabase.hairballLauncher;
        fireCooldown = stats.baseFireRate;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;
        EnemyHealth target = FindClosestEnemy();

        if (target != null && fireCooldown <= 0f)
        {
            Fire(target);
            fireCooldown = GetFireRate();
        }
    }

    EnemyHealth FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float shortest = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < shortest && d <= GetRange())
            {
                shortest = d;
                closest = e;
            }
        }

        if (closest && closest.TryGetComponent(out EnemyHealth eh))
            return eh;
        return null;
    }

    void Fire(EnemyHealth target)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        if (proj.TryGetComponent(out HairballProjectile p))
            p.SetTarget(target.transform, GetDamage());

        shotCount++;
    }

    float GetDamage()
    {
        float dmg = level switch
        {
            1 => stats.baseDamage,
            2 => stats.level2Damage,
            _ => stats.level3Damage
        };

        if (level == 3 && shotCount % 5 == 4)
            dmg *= 2f; // every 5th shot double damage
        return dmg;
    }

    float GetFireRate() => level switch
    {
        1 => stats.baseFireRate,
        2 => stats.level2FireRate,
        _ => stats.level3FireRate
    };

    float GetRange() => level switch
    {
        1 => stats.baseRange,
        2 => stats.level2Range,
        _ => stats.level3Range
    };

    public void Upgrade() { if (level < 3) level++; }
    public int GetUpgradeCost() => level == 1 ? stats.costLevel2 : stats.costLevel3;
}
