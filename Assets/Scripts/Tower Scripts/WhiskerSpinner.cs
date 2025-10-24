using UnityEngine;
using System.Collections;

public class WhiskerSpinner : MonoBehaviour
{
    public TowerDatabase towerDatabase;
    public GameObject whiskerPrefab;
    public int level = 1;

    private TowerStats stats;
    private float timer;
    private bool isBuffed = false;
    private int whiskerCount = 4;

    void Start()
    {
        stats = towerDatabase.whiskerSpinner;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= GetInterval())
        {
            Fire();
            timer = 0f;
        }
    }

    void Fire()
    {
        for (int i = 0; i < whiskerCount; i++)
        {
            float angle = (2 * Mathf.PI * i) / whiskerCount;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            GameObject whisker = Instantiate(whiskerPrefab, transform.position, Quaternion.identity);
            whisker.GetComponent<WhiskersProjectile>().Initialize(dir);
        }
    }

    float GetInterval()
    {
        float interval = (level == 1) ? stats.baseFireRate : stats.level2FireRate;
        if (isBuffed) interval *= 0.5f;
        return interval;
    }

    public void Upgrade()
    {
        level++;
        if (level == 2)
            whiskerCount = 8;
        else if (level == 3)
            StartCoroutine(FireRateBuff());
    }

    IEnumerator FireRateBuff()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            isBuffed = true;
            yield return new WaitForSeconds(3f);
            isBuffed = false;
        }
    }

    public int GetUpgradeCost() => level == 1 ? stats.costLevel2 : stats.costLevel3;
}
