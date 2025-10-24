using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinTailProjectile : MonoBehaviour
{
    public float speed = 10f;
    private EnemyHealth target;
    private float damage;
    private bool applySlow;
    private float slowPercent;
    private float slowDuration;

    public void Initialize(EnemyHealth t, float dmg, bool slow, float sPercent, float sDuration)
    {
        target = t;
        damage = dmg;
        applySlow = slow;
        slowPercent = sPercent;
        slowDuration = sDuration;

        Destroy(gameObject, 3f);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // Hit detection (no physics)
        if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)
        {
            target.TakeDamage(damage);
            if (applySlow)
                target.ApplySlow(slowPercent, slowDuration);

            Destroy(gameObject);
        }
    }
}
