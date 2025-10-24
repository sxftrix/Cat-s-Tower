using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Data")]
    public EnemyDatabase enemyDatabase;
    public string enemyType; // e.g. "Street Rat", "Sewer Runner"
    public int currentWave = 1;
    public EnemyStats stats;

    private float currentHP;
    private float maxHP;

    [Header("UI")]
    public GameObject hpBarPrefab;
    private Image hpBarFill;
    private Transform hpCanvas;

    [Header("Visual Feedback")]
    public Renderer enemyRenderer;
    private Material originalMaterial;
    private Color originalColor;
    public Color hitColor = Color.red;
    public float flashDuration = 0.15f;

    private Animator anim;
    private bool isDead = false;
    private EnemyController controller; // reference to notify
    private Coroutine slowRoutine;

    void Start()
    {
        controller = GetComponent<EnemyController>();
        anim = GetComponent<Animator>();

        LoadStatsFromDatabase();
        Initialize(maxHP);

        SetupHPBar();

        if (enemyRenderer == null)
            enemyRenderer = GetComponentInChildren<Renderer>();

        if (enemyRenderer != null)
        {
            originalMaterial = enemyRenderer.material;
            originalColor = originalMaterial.color;
        }
    }

    private void SetupHPBar()
    {
        if (hpBarPrefab != null)
        {
            GameObject bar = Instantiate(hpBarPrefab, transform);
            hpCanvas = bar.transform;

            Canvas canvasComp = hpCanvas.GetComponent<Canvas>();
            if (canvasComp != null)
            {
                canvasComp.worldCamera = Camera.main;
                canvasComp.planeDistance = 0.1f;
            }

            hpCanvas.localRotation = Quaternion.identity;
            hpCanvas.localScale = new Vector3(
                0.04f / transform.localScale.x,
                0.04f / transform.localScale.y,
                0.04f / transform.localScale.z
            );

            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                hpCanvas.localPosition = new Vector3(
                    0f,
                    (sr.bounds.size.y / transform.localScale.y) + 0.04f,
                    0f
                );
            }

            Transform fillTransform = hpCanvas.Find("HPBarFill");
            if (fillTransform == null)
                fillTransform = hpCanvas.Find("HPBar/HPBarFill");

            if (fillTransform != null)
            {
                hpBarFill = fillTransform.GetComponent<Image>();
                Debug.Log($"HP bar fill found for {name}: {hpBarFill.name}");
            }
            else
            {
                Debug.LogError($"HPBarFill not found under {hpCanvas.name}! Check prefab path.");
            }

            // Force sorting
            Canvas hpCanvasComp = hpCanvas.GetComponent<Canvas>();
            if (hpCanvasComp != null)
            {
                hpCanvasComp.overrideSorting = true;
                hpCanvasComp.sortingOrder = 3;
            }
        }
    }

    void Update()
    {
        if (hpCanvas != null)
        {
            hpCanvas.localRotation = Quaternion.identity;
            hpCanvas.localScale = new Vector3(
                0.04f / transform.localScale.x,
                0.04f / transform.localScale.y,
                0.04f / transform.localScale.z
            );
        }
    }

    private void LoadStatsFromDatabase()
    {
        if (enemyDatabase == null)
        {
            Debug.LogError($"No EnemyDatabase assigned for {gameObject.name}");
            return;
        }

        switch (enemyType)
        {
            case "Street Rat": stats = enemyDatabase.streetRat; break;
            case "Sewer Runner": stats = enemyDatabase.sewerRunner; break;
            case "Trash Tank": stats = enemyDatabase.trashTank; break;
            case "King Rat": stats = enemyDatabase.kingRat; break;
            default:
                Debug.LogWarning($"Unknown enemy type: {enemyType}");
                stats = enemyDatabase.streetRat;
                break;
        }

        maxHP = stats.GetHP(currentWave);
    }

    public void Initialize(float hp)
    {
        maxHP = hp;
        currentHP = hp;
        UpdateHPBar();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHP -= amount;
        UpdateHPBar();
        StartCoroutine(FlashOnHit());

        Debug.Log($"{enemyType} took {amount} damage. HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
            Die();
    }

    public void ApplySlow(float percent, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowEffect(percent, duration));
    }

    private IEnumerator SlowEffect(float percent, float duration)
    {
        EnemyMovement move = GetComponent<EnemyMovement>();
        if (move == null) yield break;

        float newMultiplier = 1f - percent;
        move.speedMultiplier = newMultiplier;

        yield return new WaitForSeconds(duration);

        move.speedMultiplier = 1f; // reset
    }

    void UpdateHPBar()
    {
        if (hpBarFill != null)
        {
            float fill = Mathf.Clamp01(currentHP / maxHP);
            hpBarFill.fillAmount = fill;
        }
    }

    private IEnumerator LerpHP()
    {
        float start = hpBarFill.fillAmount;
        float target = Mathf.Clamp01(currentHP / maxHP);
        float t = 0f;

        while (t < 0.2f)
        {
            t += Time.deltaTime;
            hpBarFill.fillAmount = Mathf.Lerp(start, target, t / 0.2f);
            yield return null;
        }

        hpBarFill.fillAmount = target;
    }

    private IEnumerator FlashOnHit()
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = hitColor;
            yield return new WaitForSeconds(flashDuration);
            enemyRenderer.material.color = originalColor;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (anim != null)
            anim.SetBool("isDead", true);

        controller?.OnDeath(); // Pass control
    }
}
