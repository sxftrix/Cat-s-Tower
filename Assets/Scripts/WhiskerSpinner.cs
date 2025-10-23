using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskerSpinner : MonoBehaviour
{
    public GameObject whiskerPrefab;
    public float baseInterval = 1.5f;
    public int whiskerCount = 4;
    public int maxWhiskers = 8;

    public int level = 1;
    private float timer;
    private bool isBuffed = false;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= GetCurrentInterval())
        {
            FireRockets();
            timer = 0f;
        }
    }

    void FireRockets()
    {
        for (int i = 0; i < whiskerCount; i++)
        {
            float angle = (2 * Mathf.PI * i) / whiskerCount;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            GameObject rocket = Instantiate(whiskerPrefab, transform.position, Quaternion.identity);
            rocket.GetComponent<WhiskersProjectile>().Initialize(dir);
        }
    }

    float GetCurrentInterval()
    {
        float interval = baseInterval;
        if (isBuffed) interval *= 0.5f; // 2x fire rate
        return interval;
    }

    public void UpgradeTower()
    {
        level++;
        if (level == 2)
        {
            whiskerCount = 8; // add diagonals
        }
        else if (level == 3)
        {
            StartCoroutine(FireRateBuff());
        }
    }

    IEnumerator FireRateBuff()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // every 10 seconds
            isBuffed = true;
            yield return new WaitForSeconds(3f); // 3s of double speed
            isBuffed = false;
        }
    }
}
