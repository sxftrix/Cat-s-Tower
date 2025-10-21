using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float totalMoveDuration = 6f;
    private float timer = 0f;

    private Animator anim;
    private bool isDead = false;

    private Vector3 startPoint;
    private Vector3 preCurvePoint;
    private Vector3 postCurvePoint;
    private Vector3 endPoint;
    private Vector3 controlPoint;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("isMoving", true);

        // Path points
        startPoint = new Vector3(-12f, 0f, 0f);
        preCurvePoint = new Vector3(-5f, 0f, 0f);
        postCurvePoint = new Vector3(5f, 0f, 0f);
        endPoint = new Vector3(12f, 0f, 0f);

        // Random curve up or down
        bool goUp = Random.value > 0.5f;
        float controlY = goUp ? Random.Range(3f, 6f) : Random.Range(-6f, -3f);
        controlPoint = new Vector3(0f, controlY, 0f);

        // Keep prefab scale but ensure facing right
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;

        transform.position = startPoint;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (isDead) return;

        timer += Time.deltaTime / totalMoveDuration;
        timer = Mathf.Clamp01(timer);

        // Divide movement phases:
        if (timer < 0.33f)
        {
            // Phase 1: straight start → pre-curve
            float t = timer / 0.33f;
            transform.position = Vector3.Lerp(startPoint, preCurvePoint, t);
        }
        else if (timer < 0.66f)
        {
            // Phase 2: curved section (-5 → +5)
            float t = (timer - 0.33f) / 0.33f;

            Vector3 m1 = Vector3.Lerp(preCurvePoint, controlPoint, t);
            Vector3 m2 = Vector3.Lerp(controlPoint, postCurvePoint, t);
            transform.position = Vector3.Lerp(m1, m2, t);
        }
        else
        {
            // Phase 3: straight post-curve → end
            float t = (timer - 0.66f) / 0.34f;
            transform.position = Vector3.Lerp(postCurvePoint, endPoint, t);
        }

        // Always face right and upright
        transform.rotation = Quaternion.identity;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (anim != null)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isDead", true);
        }

        Destroy(gameObject, 2f);
    }
}
