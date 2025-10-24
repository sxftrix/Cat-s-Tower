using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float baseMoveDuration = 6f;       // Base duration for the full path
    [HideInInspector] public float speedMultiplier = 1f; // Used for slows/haste

    private float timer = 0f;
    private Animator anim;
    private bool isDead = false;

    private Vector3 startPoint;
    private Vector3 preCurvePoint;
    private Vector3 controlPoint;
    private Vector3 postCurvePoint;
    private Vector3 endPoint;

    private float totalMoveDuration;

    public enum PathType { Top, Bottom }
    public PathType pathChoice;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("isMoving", true);

        // Randomly choose top or bottom path
        bool topPath = Random.value > 0.5f;

        if (topPath)
        {
            startPoint = new Vector3(-12f, 1.25f, 0f);
            preCurvePoint = new Vector3(-4f, 1.25f, 0f);
            controlPoint = new Vector3(0f, 2.35f, 0f);
            postCurvePoint = new Vector3(4f, 1.25f, 0f);
            endPoint = new Vector3(12f, 1.25f, 0f);
        }
        else
        {
            startPoint = new Vector3(-12f, -2.25f, 0f);
            preCurvePoint = new Vector3(-4f, -2.25f, 0f);
            controlPoint = new Vector3(0f, -6.40f, 0f);
            postCurvePoint = new Vector3(4f, -1.15f, 0f);
            endPoint = new Vector3(12f, -1.15f, 0f);
        }

        // Keep prefab scale but face right
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;

        transform.position = startPoint;
        transform.rotation = Quaternion.identity;

        // Read EnemyStats.speed if available
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null && health.stats != null)
            totalMoveDuration = baseMoveDuration / health.stats.speed;
        else
            totalMoveDuration = baseMoveDuration;
    }

    void Update()
    {
        if (isDead) return;

        timer += (Time.deltaTime * speedMultiplier) / totalMoveDuration;
        timer = Mathf.Clamp01(timer);

        // Phase 1: start → preCurve
        if (timer < 0.33f)
        {
            float t = timer / 0.33f;
            transform.position = Vector3.Lerp(startPoint, preCurvePoint, t);
        }
        // Phase 2: Bezier curve (preCurve → postCurve via control)
        else if (timer < 0.66f)
        {
            float t = (timer - 0.33f) / 0.33f;
            transform.position = Mathf.Pow(1 - t, 2) * preCurvePoint +
                                 2 * (1 - t) * t * controlPoint +
                                 Mathf.Pow(t, 2) * postCurvePoint;
        }
        // Phase 3: postCurve → end
        else
        {
            float t = (timer - 0.66f) / 0.34f;
            transform.position = Vector3.Lerp(postCurvePoint, endPoint, t);
        }

        transform.rotation = Quaternion.identity;
    }

    public void ApplySpeedModifier(float multiplier, float duration)
    {
        StartCoroutine(SpeedModifier(multiplier, duration));
    }

    private System.Collections.IEnumerator SpeedModifier(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
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
    }
}
