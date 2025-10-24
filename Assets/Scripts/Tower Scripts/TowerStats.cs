using UnityEngine;

[System.Serializable]
public class TowerStats
{
    [Header("Base Info")]
    public string towerName;
    public string description;

    [Header("Base Stats")]
    public float baseDamage;
    public float baseFireRate;
    public float baseRange;

    [Header("Level 2 Upgrades")]
    public float level2Damage;
    public float level2FireRate;
    public float level2Range;
    public int costLevel2;

    [Header("Level 3 Upgrades")]
    public float level3Damage;
    public float level3FireRate;
    public float level3Range;
    public int costLevel3;

    [Header("Special Effects (optional)")]
    public bool hasSlowEffect;
    [Range(0f, 1f)] public float slowChance;
    [Range(0f, 1f)] public float slowPercent;
    public float slowDuration;
}
