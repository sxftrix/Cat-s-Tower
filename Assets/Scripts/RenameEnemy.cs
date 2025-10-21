using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenameEnemy : MonoBehaviour
{
    public float hp = 10f;
    public float speed = 2f;

    private Coroutine slowRoutine;

    void Update()
    {
        // Example: move forward
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ApplySlow(float percent, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowEffect(percent, duration));
    }

    IEnumerator SlowEffect(float percent, float duration)
    {
        float originalSpeed = speed;
        speed = originalSpeed * (1f - percent);
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
    }
}
