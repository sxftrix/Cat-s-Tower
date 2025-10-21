using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    //[HideInInspector] 
    //public PlayerController ownerPlayer;

    [Header("Data")]
    public EnemyDatabase enemyDatabase;
    public string enemyType; // e.g. "Street Rat", "Sewer Runner"
    public int currentWave = 1;

    private EnemyStats stats;
    private float currentHP;
    private float maxHP;
    //private int goldReward;

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

    void Start()
    {
        anim = GetComponent<Animator>();

        LoadStatsFromDatabase();
        currentHP = maxHP;

        if (hpBarPrefab != null)
        {
            // Instantiate parented to enemy
            GameObject bar = Instantiate(hpBarPrefab, transform);
            hpCanvas = bar.transform;

            // Assign camera for World Space Canvas
            Canvas canvasComp = hpCanvas.GetComponent<Canvas>();
            if (canvasComp != null)
            {
                canvasComp.worldCamera = Camera.main;
                canvasComp.planeDistance = 0.1f;
            }

            // Keep rotation upright
            hpCanvas.localRotation = Quaternion.identity;

            // Scale compensation to keep fixed world size
            hpCanvas.localScale = new Vector3(
                0.04f / transform.localScale.x,
                0.04f / transform.localScale.y,
                0.04f / transform.localScale.z
            );

            // Position above sprite using bounds
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                hpCanvas.localPosition = new Vector3(
                    0f,
                    (sr.bounds.size.y / transform.localScale.y) + 0.04f,
                    0f
                );
            }

            // Get fill image for updates
            hpBarFill = hpCanvas.Find("HPBarFill").GetComponent<Image>();
        }

        // Renderer setup for hit flash
        if (enemyRenderer == null)
            enemyRenderer = GetComponentInChildren<Renderer>();

        if (enemyRenderer != null)
        {
            originalMaterial = enemyRenderer.material;
            originalColor = originalMaterial.color;
        }
    }

    void Update()
    {
        if (hpCanvas != null)
        {
            // Keep HP bar upright
            hpCanvas.localRotation = Quaternion.identity;

            // Maintain fixed scale if enemy scales dynamically
            hpCanvas.localScale = new Vector3(
                0.04f / transform.localScale.x,
                0.04f / transform.localScale.y,
                0.04f / transform.localScale.z
            );

            // Reposition above sprite in case enemy moves
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                hpCanvas.localPosition = new Vector3(
                    0f,
                    (sr.bounds.size.y / transform.localScale.y) + 0.04f,
                    0f
                );
            }
        }
    }

    private void LoadStatsFromDatabase()
    {
        if (enemyDatabase == null)
        {
            Debug.LogError($"No EnemyDatabase assigned for {gameObject.name}");
            return;
        }

        // Match enemy type from database
        switch (enemyType)
        {
            case "Street Rat":
                stats = enemyDatabase.streetRat;
                break;
            case "Sewer Runner":
                stats = enemyDatabase.sewerRunner;
                break;
            case "Trash Tank":
                stats = enemyDatabase.trashTank;
                break;
            case "King Rat":
                stats = enemyDatabase.kingRat;
                break;
            default:
                Debug.LogWarning($"Unknown enemy type: {enemyType}");
                stats = enemyDatabase.streetRat;
                break;
        }

        // Calculate HP scaling
        maxHP = stats.baseHP + stats.hpGrowthPerWave * (currentWave - 1);

        // ✅ Gold scaling formula
        //goldReward = Mathf.RoundToInt((maxHP / 4f) * (1f + 0.05f * (currentWave - 1)));
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHP -= amount;
        UpdateHPBar();
        StartCoroutine(FlashOnHit());

        if (currentHP <= 0)
            Die();
    }

    void UpdateHPBar()
    {
        if (hpBarFill != null)
        {
            float fill = Mathf.Clamp01(currentHP / maxHP);
            hpBarFill.fillAmount = fill;
        }
    }

    IEnumerator FlashOnHit()
    {
        if (enemyRenderer != null)
        {
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / flashDuration;
                enemyRenderer.material.color = Color.Lerp(hitColor, originalColor, t);
                yield return null;
            }
            enemyRenderer.material.color = originalColor;
        }
    }

    void Die()
    {
        isDead = true;
        if (anim != null)
            anim.SetBool("isDead", true);

        // Reward player
        /*if (ownerPlayer != null)
        {
            ownerPlayer.AddGold(goldReward);
        }
        else
        {
            Debug.LogWarning($"{enemyType} died but has no ownerPlayer assigned!");
        } */

        Destroy(gameObject, 1.5f);
    }
}
