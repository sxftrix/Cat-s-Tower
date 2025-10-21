using System.Text;
using UnityEngine;

public class HairballLauncher : MonoBehaviour
{
    [Header("Tower Settings")]
    public int level = 1;
    public float range = 5f;
    public float fireRate = 1f; // Level 1 fire rate
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Upgrade Settings")]
    public float level2FireRate = 1.5f; // shoots faster
    public float level3FireRate = 1.5f; // same, but has special effect
    public float damageLevel1 = 1f;
    public float damageLevel2 = 1.5f;
    public float damageLevel3 = 1.5f;
    public int costLevel2 = 50;
    public int costLevel3 = 200;

    private float fireCooldown = 0f;
    private int shotCount = 0; // used for every-5th-shot effect

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        RenameEnemy target = FindClosestEnemy();

        Debug.Log($"target: {target != null} fire: {fireCooldown <= 0}");
        if (target != null && fireCooldown <= 0f)
        {
            Debug.Log(target.name);
            Fire(target);
            fireCooldown = fireRate;
        }

    }

    RenameEnemy FindClosestEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        var sb = new StringBuilder();
        foreach (var e in enemies)
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            sb.AppendLine($"{e.name} {distance}");
            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                closest = e;
            }
        }
        
        Debug.Log(sb.ToString());

        if(closest != null && closest.TryGetComponent<RenameEnemy>(out var enemy))
        {
            return enemy;
        }

        return null;
    }

    void Fire(RenameEnemy target)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        HairballProjectile p = proj.GetComponent<HairballProjectile>();
        if (p != null)
        {
            float dmg = GetDamage();
            p.SetTarget(target.transform, dmg);
        }

        shotCount++;
    }

    float GetDamage()
    {
        // Base damage by level
        float dmg = damageLevel1;
        if (level == 2) dmg = damageLevel2;
        if (level == 3) dmg = damageLevel3;

        // Level 3 special: every 5th shot = double damage
        if (level == 3 && shotCount % 5 == 4)
        {
            dmg *= 2f;
        }

        return dmg;
    }

    public void Upgrade()
    {
        if (level == 1)
        {
            level = 2;
            fireRate = level2FireRate;
            Debug.Log("Upgraded to Level 2!");
        }
        else if (level == 2)
        {
            level = 3;
            fireRate = level3FireRate;
            Debug.Log("Upgraded to Level 3!");
        }
        else
        {
            Debug.Log("Already at max level!");
        }
    }
}
