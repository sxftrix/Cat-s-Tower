using UnityEngine;

public class HairballProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private float damage = 1f;

    public void SetTarget(Transform t, float dmg)
    {
        target = t;
        damage = dmg;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // Hit detection
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            EnemyHealth e = target.GetComponent<EnemyHealth>();
            if (e != null) e.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
