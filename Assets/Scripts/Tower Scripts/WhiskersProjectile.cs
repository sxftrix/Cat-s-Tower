using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskersProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float maxLifeTime = 3f;
    public float damage = 1f;

    private Vector2 direction;

    public void Initialize(Vector2 dir, float dmg = 1f)
    {
        direction = dir.normalized;
        damage = dmg;

        Destroy(gameObject, maxLifeTime);
    }

    void Update()
    {
        // Move the projectile
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Check for enemies in range
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
        foreach (EnemyHealth enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < 0.3f) // hit radius
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
                break; // stop after hitting one enemy
            }
        }
    }
}
