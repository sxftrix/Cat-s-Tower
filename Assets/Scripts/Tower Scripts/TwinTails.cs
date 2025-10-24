using UnityEngine;
using System.Collections.Generic;

public class TwinTails : MonoBehaviour
{
    public TowerDatabase towerDatabase;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int level = 1;

    private TowerStats stats;
    private float fireTimer;

    void Start()
    {
        stats = towerDatabase.twinTails;
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= GetFireInterval())
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        List<EnemyHealth> targets = new List<EnemyHealth>();

        foreach (var e in enemies)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d <= GetRange()) targets.Add(e);
        }

        targets.Sort((a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        int maxTargets = (level == 1) ? 2 : 3;
        int count = 0;
        foreach (var e in targets)
        {
            if (count >= maxTargets) break;
            SpawnProjectile(e);
            count++;
        }
    }

    void SpawnProjectile(EnemyHealth target)
    {
        if (!projectilePrefab || !target) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        TwinTailProjectile p = proj.GetComponent<TwinTailProjectile>();
        if (!p) return;

        float dmg = level switch
        {
            1 => stats.baseDamage,
            2 => stats.level2Damage,
            _ => stats.level3Damage
        };

        bool canSlow = (level >= 3 && stats.hasSlowEffect && Random.value <= stats.slowChance);
        p.Initialize(target, dmg, canSlow, stats.slowPercent, stats.slowDuration);
    }

    float GetFireInterval() => level switch
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
