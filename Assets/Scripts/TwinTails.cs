using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinTails : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Tower Settings")]
    public int level = 1;
    public float range = 6f;
    public float fireInterval = 1f;

    [Header("Damage and Slow Effect")]
    public float baseDamage = 0.5f;   // Level 1
    public float level2Damage = 1f;   // Level 2+
    public float slowChance = 0.2f;   // 20% chance at Level 3
    public float slowAmount = 0.25f;  // 25% slow
    public float slowDuration = 2f;   // 2 seconds

    [Header("Upgrade Costs")]
    public int costLevel2 = 50;
    public int costLevel3 = 200;

    private float fireTimer;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        // Find enemies in range
        RenameEnemy[] enemies = FindObjectsOfType<RenameEnemy>();
        List<RenameEnemy> validTargets = new List<RenameEnemy>();

        foreach (RenameEnemy e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist <= range)
                validTargets.Add(e);
        }

        // Sort by distance (closest first)
        validTargets.Sort((a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        // Select max targets based on tower level
        int maxTargets = (level == 1) ? 2 : 3;
        int fired = 0;

        foreach (RenameEnemy e in validTargets)
        {
            if (fired >= maxTargets) break;
            SpawnProjectile(e);
            fired++;
        }
    }

    void SpawnProjectile(RenameEnemy target)
    {
        if (projectilePrefab == null || target == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        TailProjectile bullet = proj.GetComponent<TailProjectile>();

        if (bullet != null)
        {
            float dmg = (level == 1) ? baseDamage : level2Damage;
            bool canSlow = (level >= 3 && Random.value <= slowChance);

            bullet.Initialize(target, dmg, canSlow, slowAmount, slowDuration);
        }
    }

    public void Upgrade()
    {
        if (level < 3)
            level++;
    }
}