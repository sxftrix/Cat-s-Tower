using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskersProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;

        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

}
